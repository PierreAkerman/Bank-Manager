using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace BankStartWeb.Pages.Accounts
{
    public class WithdrawlModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public WithdrawlModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string Type { get; set; }
        public string Operation { get; set; }
        public DateTime Date { get; set; }
        [BindProperty]
        public decimal Amount { get; set; }
        public decimal NewBalance { get; set; }
        public string Fullname { get; set; }
        public int CustomerId { get; set; }

        public void OnGet(int accountid)
        {
            var customer = _context.Customers
               .Include(c => c.Accounts)
               .ThenInclude(c => c.Transactions.OrderByDescending(c => c.Date))
               .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = _context.Accounts
                   .Include(a => a.Transactions)
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
               .ThenInclude(c => c.Transactions.OrderByDescending(c => c.Date))
               .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = _context.Accounts
                   .Include(a => a.Transactions)
                   .First(a => a.Id == accountid);

            Id = account.Id;
            CustomerId = customer.Id;
            Fullname = customer.Givenname + " " + customer.Surname;
            AccountType = account.AccountType;
            Balance = account.Balance;

            if (amount <= 0)
            {
                ModelState.AddModelError(nameof(amount), "You can only withdraw a positive amount!");
            }
            if (amount > Balance)
            {
                ModelState.AddModelError(nameof(Balance), "Not enough money!");
            }
            if (ModelState.IsValid)
            {
                account.Balance -= amount;

                var transaction = new Transaction
                {
                    Type = "Credit",
                    Operation = "ATM withdrawls",
                    Date = DateTime.Now,
                    Amount = amount,
                    NewBalance = account.Balance,
                };
                account.Transactions.Add(transaction);
                _context.SaveChanges();

                return RedirectToPage("AccountDetails", new { accountid });
            }
            return Page();
        }
    }
}
