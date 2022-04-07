using BankStartWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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
            var custDetails = _context.Customers
                .Include(c => c.Accounts)
                .First(e => e.Id == customerid);
            Id = custDetails.Id;
            Name = custDetails.Givenname;
            Surname = custDetails.Surname;
            Streetaddress = custDetails.Streetaddress;
            City = custDetails.City;
            Zipcode = custDetails.Zipcode;
            Country = custDetails.Country;
            CountryCode = custDetails.CountryCode;
            NationalId = custDetails.NationalId;
            TelephoneCountryCode = custDetails.TelephoneCountryCode;
            Telephone = custDetails.Telephone;
            EmailAddress = custDetails.EmailAddress;
            Birthday = custDetails.Birthday;
            Accounts = custDetails.Accounts;
            Fullname = custDetails.Givenname + " " + custDetails.Surname;

            TotalSaldo = 0;
            foreach (var account in Accounts)
            {
                TotalSaldo += account.Balance;
            }
        }
    }
}

