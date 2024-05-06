using MyBankingAPI.Interfaces;
using MyBankingAPI.Models;
using Newtonsoft.Json;

namespace MyBankingAPI.Repositories
{
    public class JsonCustomerRepository : ICustomerRepository
    {
        private readonly string _filePath;

        public JsonCustomerRepository(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public Customer GetCustomer(int customerId)
        {
            var customers = LoadCustomers();
            return customers.Find(c => c.CustomerId == customerId);
        }

        private List<Customer> LoadCustomers()
        {
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Customer>>(json);
        }
    }
}
