using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Pages.Accounts
{
    public class WithdrawlModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public WithdrawlModel(ApplicationDbContext context)
        {
            _context = context;
        }
        // AccountProps
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public string AccountType { get; set; }
        public List<Transaction> Transactions { get; set; }
        // TransactionProps
        public string Type { get; set; }
        public string Operation { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal NewBalance { get; set; }
        // CustomerProps
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
    }
}
