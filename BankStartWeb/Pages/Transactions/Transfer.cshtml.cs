using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.Transactions
{
    public class TransferModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;

        public TransferModel(ApplicationDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }
        [BindProperty]
        public decimal Amount { get; set; }
        [BindProperty]
        public decimal Balance { get; set; }
        [BindProperty]
        public int TargetAccount { get; set; }
        [BindProperty]
        public int Id { get; set; }
        public string AccountType { get; set; }
        public int CustomerId { get; set; }
        public string Fullname { get; set; }

        public void OnGet(int accountid)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = _context.Accounts.First(a => a.Id == accountid);

            Id = account.Id;
            CustomerId = customer.Id;
            Fullname = customer.Givenname + " " + customer.Surname;
            AccountType = account.AccountType;
            Balance = account.Balance;
        }
        public IActionResult OnPost(int accountid, decimal amount, int targetAccount)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = _context.Accounts.First(a => a.Id == accountid);

            Balance = account.Balance;
            TargetAccount = targetAccount;

            if (ModelState.IsValid)
            {
                var result = _transactionService.MakeTransfer(accountid, amount, targetAccount);

                switch (result)
                {
                    case ITransactionService.TransactionStatus.Ok:
                        return RedirectToPage("/Accounts/AccountDetails", new { accountid });
                    case ITransactionService.TransactionStatus.NotPositiveAmount:
                        ModelState.AddModelError(nameof(amount), "You can only transfer a positive amount!");
                        break;
                    case ITransactionService.TransactionStatus.InsufficientBalance:
                        ModelState.AddModelError(nameof(Balance), "Not enough money!");
                        break;
                    case ITransactionService.TransactionStatus.IncorrectTargetAccountId:
                        ModelState.AddModelError(nameof(targetAccount), "Incorrect target account Id");
                        break;
                }
            }
            Id = account.Id;
            CustomerId = customer.Id;
            Fullname = customer.Givenname + " " + customer.Surname;
            AccountType = account.AccountType;
            Balance = account.Balance;
            return Page();
        }
    }
}
