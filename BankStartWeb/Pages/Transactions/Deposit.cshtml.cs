using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace BankStartWeb.Pages.Transactions
{
    public class DepositModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;
        private readonly IToastNotification _toastNotification;

        public DepositModel(ApplicationDbContext context, ITransactionService transactionService, IToastNotification toastNotification)
        {
            _context = context;
            _transactionService = transactionService;
            _toastNotification = toastNotification;
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
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = _context.Accounts
                .First(a => a.Id == accountid);

            if (ModelState.IsValid)
            {
                var result = _transactionService.MakeDeposit(accountid, amount);

                switch (result)
                {
                    case ITransactionService.TransactionStatus.Ok:
                        return RedirectToPage("/Accounts/AccountDetails", new { accountid });
                    case ITransactionService.TransactionStatus.NotPositiveAmount:
                        ModelState.AddModelError(nameof(amount), "You can only deposit a positive amount!");
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
