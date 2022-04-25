using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Pages.Accounts
{
    public class SalaryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SalaryModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public int Id { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public string Type { get; set; }
        public string Operation { get; set; }
        public DateTime Date { get; set; }
        [Required]
        [BindProperty]
        public decimal Amount { get; set; }
        public decimal NewBalance { get; set; }
        public string Fullname { get; set; }
        public int CustomerId { get; set; }

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
        public IActionResult OnPost(int accountid, decimal amount)
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
            if (ModelState.IsValid)
            {
                var transaction = new Transaction
                {
                    Type = "Debit",
                    Operation = "Salary",
                    Date = DateTime.Now,
                    Amount = amount,
                    NewBalance = account.Balance + amount,
                };
                account.Balance += amount;
                account.Transactions.Add(transaction);

                _context.SaveChanges();
                return RedirectToPage("AccountDetails", new { accountid });
            }
            return Page();
        }
    }
}
