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
        public void OnGet()
        {
            TotalAccounts = _context.Accounts.Count();
            TotalCustomers = _context.Customers.Count();

            Accounts = _context.Accounts.ToList();
            foreach(var account in Accounts)
            {
                TotalBalance += account.Balance;
            }

            SwedishCustomers = _countStatistics.CountCustomers("se");
            FinnishCustomers = _countStatistics.CountCustomers("fi");
            NorweigianCustomers = _countStatistics.CountCustomers("no");

            SwedishAccounts = _countStatistics.CountAccounts("se");
            FinnishAccounts = _countStatistics.CountAccounts("fi");
            NorweigianAccounts = _countStatistics.CountAccounts("no");

            SwedishBalance = _countStatistics.CountryBalance("se");
            FinnishBalance = _countStatistics.CountryBalance("fi");
            NorweigianBalance = _countStatistics.CountryBalance("no");
        }
    }
}