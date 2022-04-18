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
        private readonly ICountStatisticsService _countStatisticsService;

        public IndexModel(ApplicationDbContext context, ICountStatisticsService countStatisticsService)
        {
            _context = context;
            _countStatisticsService = countStatisticsService;
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

            SwedishCustomers = _countStatisticsService.CountCustomers("se");
            FinnishCustomers = _countStatisticsService.CountCustomers("fi");
            NorweigianCustomers = _countStatisticsService.CountCustomers("no");

            SwedishAccounts = _countStatisticsService.CountAccounts("se");
            FinnishAccounts = _countStatisticsService.CountAccounts("fi");
            NorweigianAccounts = _countStatisticsService.CountAccounts("no");

            SwedishBalance = _countStatisticsService.CountryBalance("se");
            FinnishBalance = _countStatisticsService.CountryBalance("fi");
            NorweigianBalance = _countStatisticsService.CountryBalance("no");
        }
    }
}