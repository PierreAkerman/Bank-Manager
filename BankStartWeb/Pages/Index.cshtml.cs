using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BankStartWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ICountStatistics _countStatistics;

        public IndexModel(ApplicationDbContext context, ICountStatistics countStatistics)
        {
            _context = context;
            _countStatistics = countStatistics;
        }

        ///public List<Customer> Customers { get; set; }
        public List<Account> Accounts { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalAccounts { get; set; }
        public decimal TotalBalance { get; set; }
        public int SwedishCustomers { get; set; }
        public int FinnishCustomers { get; set; }
        public int NorweigianCustomers { get; set; }
        public int SwedishAccounts { get; set; }
        public int FinnishAccounts { get; set; }
        public int NorweigianAccounts { get; set; }
        public decimal SwedishBalance { get; set; }
        public decimal FinnishBalance { get; set; }
        public decimal NorweigianBalance { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SearchCustId { get; set; }

        //public IActionResult OnPost(int? searchCustId)
        //{
        //    var c = _context.Customers.AsQueryable();

        //    if (searchCustId.HasValue)
        //    {
        //        var currentCustomer = c.Single(cust => cust.Id.Equals(searchCustId));
        //        return RedirectToPage("/CustomerDetails", new { searchCustId = currentCustomer.Id });
        //    }
        //    return Page();
        //}

        public void OnGet()
        {
            TotalAccounts = _context.Accounts.Count();
            TotalCustomers = _context.Customers.Count();

            Accounts = _context.Accounts.ToList();
            foreach(var account in Accounts)
            {
                TotalBalance += account.Balance;
            }

            SwedishCustomers = _countStatistics.CountCustomers("Sverige");
            FinnishCustomers = _countStatistics.CountCustomers("Finland");
            NorweigianCustomers = _countStatistics.CountCustomers("Norge");

            SwedishAccounts = _countStatistics.CountAccounts("Sverige");
            FinnishAccounts = _countStatistics.CountAccounts("Finland");
            NorweigianAccounts = _countStatistics.CountAccounts("Norge");

            SwedishBalance = _countStatistics.CountryBalance("Sverige");
            FinnishBalance = _countStatistics.CountryBalance("Finland");
            NorweigianBalance = _countStatistics.CountryBalance("Norge");
        }
    }
}