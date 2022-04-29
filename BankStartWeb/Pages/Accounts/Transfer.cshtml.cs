using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace BankStartWeb.Pages.Accounts
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
        public int TargetAccount { get; set; }
        public string TransactionType { get; set; }
        public string TransactionOperation { get; set; }
        public DateTime Date { get; set; }
        [BindProperty]
        public decimal Amount { get; set; }
        [BindProperty]
        public decimal Balance { get; set; }
        public decimal NewBalance { get; set; }
        [BindProperty]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Fullname { get; set; }
        public string AccountType { get; set; }

        public void OnGet(int accountid)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = _context.Accounts.First(a => a.Id == accountid);

            Id = account.Id;
            CustomerId = customer.Id;
            Fullname = customer.Givenname + " " + customer.Surname;
            AccountType = account.AccountType;
            Balance = account.Balance;
        }
        public IActionResult OnPost(int accountid, decimal amount, int TargetAccount)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = _context.Accounts.First(a => a.Id == accountid);

            Balance = account.Balance;

            if (ModelState.IsValid)
            {
                var result = _transactionService.MakeTransfer(accountid, amount, TargetAccount);

                if(result == ITransactionService.TransactionStatus.Ok)
                    return RedirectToPage("AccountDetails", new { accountid });

                else if (result == ITransactionService.TransactionStatus.NotPositiveAmount)
                {
                    ModelState.AddModelError(nameof(amount), "You can only withdraw a positive amount!");
                }
                else if (result == ITransactionService.TransactionStatus.InsufficientBalance)
                {
                    ModelState.AddModelError(nameof(Balance), "Not enough money!");
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
