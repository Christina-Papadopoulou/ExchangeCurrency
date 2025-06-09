namespace WalletAppication.Models
{
    public class CreateWalletRequest
    {
        public decimal InitialBalance { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}
