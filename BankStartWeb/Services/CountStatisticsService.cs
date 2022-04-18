using BankStartWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Services
{
    public interface ICountStatisticsService
    {
        int CountAccounts(string country);
        int CountCustomers(string country);
        decimal CountryBalance(string country);
    }

    public class CountStatisticsService : ICountStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public CountStatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CountCustomers(string countryCode)
        {
            return _context.Customers.Count(c => c.CountryCode == countryCode.ToUpper());
        }
        public int CountAccounts(string countryCode)
        {
            return _context.Customers
                .Include(c => c.Accounts)
                .Where(c => c.CountryCode == countryCode.ToUpper())
                .SelectMany(c => c.Accounts)
                .Count();
        }
        public decimal CountryBalance(string countryCode)
        {
            decimal tempContainer = 0;
            var countCountryBalance = _context.Customers
                .Include(c => c.Accounts)
                .Where(c => c.CountryCode == countryCode.ToUpper())
                .SelectMany(c => c.Accounts);
            foreach (var account in countCountryBalance)
            {
                tempContainer += account.Balance;
            }
            return tempContainer;
        }
    }
}
