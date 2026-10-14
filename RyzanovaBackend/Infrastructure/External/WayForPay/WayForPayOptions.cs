namespace Infrastructure.External.WayForPay;

public class WayForPayOptions
{
    public const string SectionName = "WayForPay";

    public string MerchantAccount { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string MerchantDomain { get; set; } = string.Empty;
    public string ServiceUrl { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = "https://api.wayforpay.com/api";
    public string Language { get; set; } = "ua";
    public int OrderTimeout { get; set; } = 600;
}
