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
        public IActionResult MakeDeposit([FromBody] TransactionRequest request)
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

            return Ok(new TransactionResponse
            {
                CustomerId = request.CustomerId,
                AccountId = request.AccountId,
                Balance = account.Balance,
                Succeeded = true
            });
        }


        [HttpPost("Withdrawal")]
        public IActionResult MakeWithdrawal([FromBody] TransactionRequest request)
        {
            // Validate request
            if (request.Amount <= 0)
                return BadRequest("Withdrawal amount must be greater than 0.");

            var account = _accountRepository.GetAccount(request.CustomerId, request.AccountId);
            if (account == null)
                return NotFound("Account not found.");

            // Check if withdrawal is valid
            if (account.Balance < request.Amount)
                return BadRequest("Insufficient funds for withdrawal.");

            // Perform withdrawal
            account.Balance -= request.Amount;
            _accountRepository.UpdateAccount(account);

            return Ok(new TransactionResponse
            {
                CustomerId = request.CustomerId,
                AccountId = request.AccountId,
                Balance = account.Balance,
                Succeeded = true
            });
        }

        [HttpPost("CloseAccount")]
        public IActionResult CloseAccount([FromBody] CloseAccountRequest request)
        {
            // Validate request
            var account = _accountRepository.GetAccount(request.CustomerId, request.AccountId);
            if (account == null)
                return NotFound("Account not found.");

            if (account.Balance != 0)
                return BadRequest("Account balance must be exactly 0 to close the account.");

            // Close account by removing it
            _accountRepository.RemoveAccount(account);
            _customerRepository.RemoveCustomer(request.CustomerId);

            return Ok(new CloseAccountResponse
            {
                CustomerId = request.CustomerId,
                AccountId = request.AccountId,
                Succeeded = true
            });
        }
    }
}
