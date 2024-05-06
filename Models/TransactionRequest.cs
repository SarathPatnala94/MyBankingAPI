namespace MyBankingAPI.Models
{
    public class TransactionRequest
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
