using MyBankingAPI.Models;

namespace MyBankingAPI.Interfaces
{
    public interface IAccountRepository
    {
        Account GetAccount(int customerId, int accountId);
        void UpdateAccount(Account account);
    }

}
