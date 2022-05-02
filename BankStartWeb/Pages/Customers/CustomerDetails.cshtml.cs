using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace BankStartWeb.Pages.Customers
{
    public class CustomerDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CustomerDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fullname { get; set; }
        public string Streetaddress { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string NationalId { get; set; }
        public int TelephoneCountryCode { get; set; }
        public string Telephone { get; set; }
        public string EmailAddress { get; set; }
        public DateTime Birthday { get; set; }
        public List<Account> Accounts { get; set; }
        public decimal TotalSaldo { get; set; }

        public void OnGet(int customerid)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts.OrderByDescending(c => c.Created))
                .First(c => c.Id == customerid);

            Id = customer.Id;
            Name = customer.Givenname;
            Surname = customer.Surname;
            Streetaddress = customer.Streetaddress;
            City = customer.City;
            Zipcode = customer.Zipcode;
            Country = customer.Country;
            CountryCode = customer.CountryCode;
            NationalId = customer.NationalId;
            TelephoneCountryCode = customer.TelephoneCountryCode;
            Telephone = customer.Telephone;
            EmailAddress = customer.EmailAddress;
            Birthday = customer.Birthday;
            Accounts = customer.Accounts;
            Fullname = customer.Givenname + " " + customer.Surname;

            TotalSaldo = 0;
            foreach (var account in Accounts)
            {
                TotalSaldo += account.Balance;
            }
        }
    }
}

