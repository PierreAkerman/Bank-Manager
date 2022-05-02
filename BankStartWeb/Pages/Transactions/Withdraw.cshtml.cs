#nullable disable
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.Transactions
{
    public class WithdrawModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;

        public WithdrawModel(ApplicationDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        [BindProperty]
        public decimal Amount { get; set; }
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
                var result = _transactionService.MakeWithdraw(accountid, amount);

                switch (result)
                {
                    case ITransactionService.TransactionStatus.Ok:
                        return RedirectToPage("/Accounts/AccountDetails", new { accountid });
                    case ITransactionService.TransactionStatus.NotPositiveAmount:
                        ModelState.AddModelError(nameof(amount), "You can only withdraw a positive amount!");
                        break;
                    case ITransactionService.TransactionStatus.InsufficientBalance:
                        ModelState.AddModelError(nameof(Balance), "Not enough money!");
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
