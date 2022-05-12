using BankStartWeb.Data;
using BankStartWeb.Infrastructure.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.Accounts
{
    
    [Authorize(Roles="Admin, Cashier")]
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
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public void OnGet(int accountid)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions.OrderByDescending(c => c.Date))
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = customer.Accounts.First(a => a.Id == accountid);

            CustomerId = customer.Id;
            CustomerName = customer.Givenname + " " + customer.Surname;
            Id = accountid;
            AccountType = account.AccountType;
            Created = account.Created;
            Balance = account.Balance;
        }
        public IActionResult OnGetFetchMore(int accountid, int pageNo)
        {
            var query = _context.Accounts.Where(e => e.Id == accountid)
                .SelectMany(e => e.Transactions)
                .OrderByDescending(e => e.Date);
            var transaction = query.GetPaged(pageNo, 5);

            var list = transaction.Results.Select(e => new
            {
                e.Type,
                e.Operation,
                Date = e.Date.ToString("g"),
                e.Amount,
                e.NewBalance

            }).ToList();

            return new JsonResult(new { items = list });
        }
    }
}
