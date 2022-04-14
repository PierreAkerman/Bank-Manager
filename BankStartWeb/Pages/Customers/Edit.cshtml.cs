using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Pages.Customers
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ISetListsServices _setListsServices;

        public EditModel(ApplicationDbContext context, ISetListsServices setListsServices)
        {
            _context = context;
            _setListsServices = setListsServices;
        }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string Streetaddress { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string CountryId { get; set; } //public string Country { get; set; }
        public string CountryCodeId { get; set; } //public string CountryCode { get; set; }
        public string NationalId { get; set; }
        public int TelCodeId { get; set; } //public int TelephoneCountryCode { get; set; }
        public string Telephone { get; set; }
        public string EmailAddress { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        //public List<Account> Accounts { get; set; } = new List<Account>();
        public List<SelectListItem> AllCountries { get; set; }
        public List<SelectListItem> AllCountryCodes { get; set; }
        public List<SelectListItem> AllTelCodes { get; set; }
        
        public void OnGet(int customerId)
        {
            var customer = _context.Customers.First(c => c.Id == customerId);
            Givenname = customer.Givenname;
            Surname = customer.Surname;
            Streetaddress = customer.Streetaddress;
            City = customer.City;
            Zipcode = customer.Zipcode;
            CountryId = customer.Country;
            CountryCodeId = customer.CountryCode;
            NationalId = customer.NationalId;
            TelCodeId = customer.TelephoneCountryCode;
            Telephone = customer.Telephone;
            EmailAddress = customer.EmailAddress;
            Birthday = customer.Birthday;

            AllCountries = _setListsServices.SetAllCountries();
            AllCountryCodes = _setListsServices.SetAllCountryCodes();
            AllTelCodes = _setListsServices.SetAllTelCodes();
        }
        public IActionResult OnPost(int customerId)
        {
            if (ModelState.IsValid)
            {
                var customer = _context.Customers.First(c => c.Id == customerId);
                customer.Givenname = Givenname;
                customer.Surname = Surname;
                customer.Streetaddress = Streetaddress;
                customer.City = City;
                customer.Zipcode = Zipcode;
                customer.Country = CountryId;
                customer.CountryCode = CountryCodeId;
                customer.NationalId = NationalId;
                customer.TelephoneCountryCode = TelCodeId;
                customer.Telephone = Telephone;
                customer.EmailAddress = EmailAddress;
                customer.Birthday = Birthday;

                _context.SaveChanges();
                return RedirectToPage("CustomerList");
            }
            AllCountries = _setListsServices.SetAllCountries();
            AllCountryCodes = _setListsServices.SetAllCountryCodes();
            AllTelCodes = _setListsServices.SetAllTelCodes();
            return Page();
        }
    }
}
