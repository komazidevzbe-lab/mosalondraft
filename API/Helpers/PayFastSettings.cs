namespace API.Helpers;

public class PayFastSettings
{
    public string MerchantId { get; set; } = string.Empty;

    public string MerchantKey { get; set; } = string.Empty;

    public string Passphrase { get; set; } = string.Empty;

    public string ProcessUrl { get; set; } = string.Empty;

    public string ReturnUrl { get; set; } = string.Empty;

    public string CancelUrl { get; set; } = string.Empty;

    public string NotifyUrl { get; set; } = string.Empty;

    public bool SandboxMode { get; set; } = true;
}