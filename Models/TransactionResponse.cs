namespace MyBankingAPI.Models
{
    public class TransactionResponse
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public bool Succeeded { get; set; }
    }
}
