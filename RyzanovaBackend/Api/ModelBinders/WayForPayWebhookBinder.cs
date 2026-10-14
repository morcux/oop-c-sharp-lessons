using System.Text.Json;
using Domain.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.ModelBinders;

public class WayForPayWebhookBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var request = bindingContext.HttpContext.Request;
        var ct = bindingContext.HttpContext.RequestAborted;

        Dictionary<string, string> fields;

        if (request.HasFormContentType)
        {
            var form = await request.ReadFormAsync(ct);
            fields = form.ToDictionary(kv => kv.Key, kv => kv.Value.ToString());
        }
        else
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync(ct);
            using var doc = JsonDocument.Parse(body);
            fields = doc.RootElement.EnumerateObject()
                .ToDictionary(
                    p => p.Name,
                    p => p.Value.ValueKind == JsonValueKind.String
                        ? p.Value.GetString()!
                        : p.Value.GetRawText());
        }

        string Get(string key) => fields.GetValueOrDefault(key, string.Empty);

        var model = new WayForPayWebhookData(
            OrderReference: Get("orderReference"),
            Amount: Get("amount"),
            Currency: Get("currency"),
            AuthCode: Get("authCode"),
            CardPan: Get("cardPan"),
            TransactionStatus: Get("transactionStatus"),
            ReasonCode: Get("reasonCode"),
            MerchantSignature: Get("merchantSignature"));

        bindingContext.Result = ModelBindingResult.Success(model);
    }
}
