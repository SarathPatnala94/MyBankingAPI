using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBankingAPI.Interfaces;
using MyBankingAPI.Models;

namespace MyBankingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomerRepository _customerRepository;

        public BankingController(IAccountRepository accountRepository, ICustomerRepository customerRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }


        [HttpPost("Deposit")]
        public IActionResult MakeDeposit([FromBody] DepositRequest request)
        {
            // Validate request
            if (request.Amount <= 0)
                return BadRequest("Deposit amount must be greater than 0.");

            var account = _accountRepository.GetAccount(request.CustomerId, request.AccountId);
            if (account == null)
                return NotFound("Account not found.");

            // Perform deposit
            account.Balance += request.Amount;
            _accountRepository.UpdateAccount(account);

            return Ok(new DepositResponse
            {
                CustomerId = request.CustomerId,
                AccountId = request.AccountId,
                Balance = account.Balance,
                Succeeded = true
            });
        }
    }
}
