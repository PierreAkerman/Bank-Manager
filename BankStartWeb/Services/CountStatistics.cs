using BankStartWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Services
{
    public interface ICountStatistics
    {
        int CountAccounts(string country);
        int CountCustomers(string country);
        decimal CountryBalance(string country);
    }

    public class CountStatistics : ICountStatistics
    {
        private readonly ApplicationDbContext _context;

        public CountStatistics(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CountCustomers(string country)
        {
            return _context.Customers.Where(c => c.Country == country).Count();
        }
        public int CountAccounts(string country)
        {
            return _context.Customers
                .Include(c => c.Accounts)
                .Where(c => c.Country == country)
                .SelectMany(c => c.Accounts)
                .Count();
        }
        public decimal CountryBalance(string country)
        {
            decimal tempContainer = 0;
            var swedishAccountsBalance = _context.Customers
                .Include(c => c.Accounts)
                .Where(c => c.Country == country)
                .SelectMany(c => c.Accounts);
            foreach (var account in swedishAccountsBalance)
            {
                tempContainer += account.Balance;
            }
            return tempContainer;
        }
    }
}
