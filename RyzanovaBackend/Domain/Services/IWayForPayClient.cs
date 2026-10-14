namespace Domain.Services;

public interface IWayForPayClient
{
    Task<string> CreateInvoiceAsync(
        string orderReference,
        decimal amount,
        string productName,
        string returnUrl,
        CancellationToken ct = default);

    bool VerifyWebhookSignature(WayForPayWebhookData data);

    string SignWebhookResponse(string orderReference, string status, long time);
}

public record WayForPayWebhookData(
    string OrderReference,
    string Amount,
    string Currency,
    string AuthCode,
    string CardPan,
    string TransactionStatus,
    string ReasonCode,
    string MerchantSignature
);