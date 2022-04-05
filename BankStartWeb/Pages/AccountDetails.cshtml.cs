using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages
{
    public class AccountDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AccountDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Id { get; set; }
        public string AccountType { get; set; }
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }
        public string CustomerName { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int CustomerId { get; set; }

        public void OnGet(int accountid)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions.OrderByDescending(c => c.Date))
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = customer.Accounts.First(a => a.Id == accountid);

            CustomerId = customer.Id;
            CustomerName = customer.Givenname + " " + customer.Surname;
            Id = account.Id;
            AccountType = account.AccountType;
            Created = account.Created;
            Balance = account.Balance;
            Transactions = account.Transactions;
        }
    }
}
