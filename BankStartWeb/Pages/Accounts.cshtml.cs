using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages
{
    public class AccountsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AccountsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Id { get; set; }
        public string AccountType { get; set; }
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }
        public string CustomerName { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<Account> Accounts { get; set; }
        public int CustomerId { get; set; }

        public void OnGet(int accountid)
        {
            var accountDetails = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions.OrderByDescending(c => c.Date))
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = accountDetails.Accounts.First(a => a.Id == accountid);

            CustomerName = accountDetails.Givenname + " " + accountDetails.Surname;
            Id = account.Id;
            AccountType = account.AccountType;
            Created = account.Created;
            Balance = account.Balance;
            Transactions = account.Transactions;
            CustomerId = accountDetails.Id;


            //var accountDetails = _context.Accounts
            //    .Include(c => c.Transactions.OrderByDescending(c => c.Date))
            //    .First(e => e.Id == accountid);
            //Id = accountDetails.Id;
            //AccountType = accountDetails.AccountType;
            //Created = accountDetails.Created;
            //Balance = accountDetails.Balance;
            //Transactions = accountDetails.Transactions;
        }
    }
}
