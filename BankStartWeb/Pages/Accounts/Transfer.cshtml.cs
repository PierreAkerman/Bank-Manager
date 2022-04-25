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

        public TransferModel(ApplicationDbContext context)
        {
            _context = context;
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

            var account = customer.Accounts.First(a => a.Id == accountid);

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

            var account = customer.Accounts.First(a => a.Id == accountid);

            Id = account.Id;
            CustomerId = customer.Id;
            Fullname = customer.Givenname + " " + customer.Surname;
            AccountType = account.AccountType;
            Balance = account.Balance;

            if (amount <= 0)
            {
                ModelState.AddModelError(nameof(amount), "Enter a positive amount, please!");
            }
            if (amount > Balance)
            {
                ModelState.AddModelError(nameof(Balance), "Not enough money!");
            }
            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                    Type = "Credit",
                    Operation = "Transfer",
                    Date = DateTime.Now,
                    Amount = amount,
                    NewBalance = Balance - amount,
                };
                account.Balance -= amount;
                account.Transactions.Add(transaction);

                var theTargetAccount = _context.Accounts.First(a => a.Id == TargetAccount);

                var targetTransaction = new Transaction
                {
                    Type = "Debit",
                    Operation = "Transfer",
                    Date = DateTime.Now,
                    Amount = amount,
                    NewBalance = theTargetAccount.Balance + amount,
                };
                theTargetAccount.Balance += amount;
                theTargetAccount.Transactions.Add(targetTransaction);

                _context.SaveChanges();
                return RedirectToPage("AccountDetails", new { accountid });
            }
            return Page();
        }
    }
}
