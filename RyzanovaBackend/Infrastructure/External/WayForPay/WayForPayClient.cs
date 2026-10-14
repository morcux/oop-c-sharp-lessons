using System.Globalization;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.External.WayForPay;

public class WayForPayClient : IWayForPayClient
{
    private readonly HttpClient _http;
    private readonly WayForPayOptions _options;
    private readonly ILogger<WayForPayClient> _logger;

    public WayForPayClient(HttpClient http, IOptions<WayForPayOptions> options, ILogger<WayForPayClient> logger)
    {
        _http = http;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> CreateInvoiceAsync(
        string orderReference, decimal amount, string productName, string returnUrl, CancellationToken ct = default)
    {
        var orderDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var amountStr = FormatAmount(amount);

        // merchantAccount;merchantDomainName;orderReference;orderDate;amount;currency;productName;productCount;productPrice
        var signature = HmacMd5(string.Join(";",
            _options.MerchantAccount,
            _options.MerchantDomain,
            orderReference,
            orderDate,
            amountStr,
            "UAH",
            productName,
            "1",
            amountStr));

        var payload = new Dictionary<string, object>
        {
            ["transactionType"] = "CREATE_INVOICE",
            ["merchantAccount"] = _options.MerchantAccount,
            ["merchantDomainName"] = _options.MerchantDomain,
            ["merchantSignature"] = signature,
            ["apiVersion"] = 1,
            ["language"] = _options.Language,
            ["serviceUrl"] = _options.ServiceUrl,
            ["returnUrl"] = returnUrl,
            ["orderReference"] = orderReference,
            ["orderDate"] = orderDate,
            ["orderTimeout"] = _options.OrderTimeout,
            ["amount"] = amount,
            ["currency"] = "UAH",
            ["productName"] = new[] { productName },
            ["productCount"] = new[] { 1 },
            ["productPrice"] = new[] { amount },
        };

        var response = await _http.PostAsJsonAsync(_options.ApiUrl, payload, ct);
        var body = await response.Content.ReadAsStringAsync(ct);
        _logger.LogInformation("WayForPay CREATE_INVOICE {OrderReference}: {Body}", orderReference, body);

        using var doc = JsonDocument.Parse(body);
        if (doc.RootElement.TryGetProperty("invoiceUrl", out var url) && url.ValueKind == JsonValueKind.String)
        {
            return url.GetString()!;
        }

        throw new InvalidOperationException($"WayForPay did not return invoiceUrl: {body}");
    }

    public bool VerifyWebhookSignature(WayForPayWebhookData d)
    {
        var expected = HmacMd5(string.Join(";",
            _options.MerchantAccount,
            d.OrderReference, d.Amount, d.Currency,
            d.AuthCode, d.CardPan, d.TransactionStatus, d.ReasonCode));

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected),
            Encoding.UTF8.GetBytes(d.MerchantSignature ?? string.Empty));
    }

    public string SignWebhookResponse(string orderReference, string status, long time)
        => HmacMd5($"{orderReference};{status};{time}");

    private string HmacMd5(string text)
    {
        using var hmac = new HMACMD5(Encoding.UTF8.GetBytes(_options.SecretKey));
        return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(text))).ToLowerInvariant();
    }

    private static string FormatAmount(decimal amount)
        => amount == decimal.Truncate(amount)
            ? ((long)amount).ToString(CultureInfo.InvariantCulture)
            : amount.ToString(CultureInfo.InvariantCulture);
}
