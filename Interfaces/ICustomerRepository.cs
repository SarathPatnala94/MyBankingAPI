using MyBankingAPI.Models;

namespace MyBankingAPI.Interfaces
{
    public interface ICustomerRepository
    {
        Customer GetCustomer(int customerId);
    }
}
