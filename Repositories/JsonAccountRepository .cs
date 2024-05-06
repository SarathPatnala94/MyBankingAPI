using MyBankingAPI.Interfaces;
using MyBankingAPI.Models;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace MyBankingAPI.Repositories
{
    public class JsonAccountRepository : IAccountRepository
    {
        private readonly string _filePath;

        public JsonAccountRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public Account GetAccount(int customerId, int accountId)
        {
            var accounts = LoadAccounts();
            return accounts.Find(a => a.CustomerId == customerId && a.AccountId == accountId);
        }

        public void UpdateAccount(Account account)
        {
            var accounts = LoadAccounts();
            var index = accounts.FindIndex(a => a.AccountId == account.AccountId);
            if (index != -1)
            {
                accounts[index] = account;
                SaveAccounts(accounts);
            }
        }

        private List<Account> LoadAccounts()
        {
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Account>>(json);
        }

        private void SaveAccounts(List<Account> accounts)
        {
            var json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }

}
