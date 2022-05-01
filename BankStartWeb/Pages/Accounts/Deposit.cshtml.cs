using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.Accounts
{
    public class DepositModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;

        public DepositModel(ApplicationDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }

        // AccountProps
        [BindProperty]
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        // TransactionProps
        [BindProperty]
        public decimal Amount { get; set; }
        // CustomerProps
        public string Fullname { get; set; }
        public int CustomerId { get; set; }

        public void OnGet(int accountid)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = _context.Accounts
                   .First(a => a.Id == accountid);

            Id = account.Id;
            CustomerId = customer.Id;
            Fullname = customer.Givenname + " " + customer.Surname;
            AccountType = account.AccountType;
            Balance = account.Balance;
        }
        public IActionResult OnPost(int accountid, decimal amount)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = _context.Accounts
                .First(a => a.Id == accountid);

            if (ModelState.IsValid)
            {
                var result = _transactionService.MakeDeposit(accountid, amount);

                if(result == ITransactionService.TransactionStatus.Ok)
                    return RedirectToPage("AccountDetails", new { accountid });
                else if (result == ITransactionService.TransactionStatus.NotPositiveAmount)
                {
                    ModelState.AddModelError(nameof(amount), "You can only deposit a positive amount!");
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
