using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Pages.Customers
{
    [BindProperties]
    public class NewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ISetListsService _setListsService;

        public NewModel(ApplicationDbContext context, ISetListsService setListsService)
        {
            _context = context;
            _setListsService = setListsService;
        }
        [MaxLength(50)] public string Givenname { get; set; }
        [MaxLength(50)] public string Surname { get; set; }
        [MaxLength(50)] public string Streetaddress { get; set; }
        [MaxLength(50)] public string City { get; set; }
        [MaxLength(10)] public string Zipcode { get; set; }
        public string CountryId { get; set; } //public string Country { get; set; }
        public string CountryCodeId { get; set; } //public string CountryCode { get; set; }
        [MaxLength(20)] public string NationalId { get; set; }
        public int TelCodeId { get; set; } //public int TelephoneCountryCode { get; set; }
        [MaxLength(50)] public string Telephone { get; set; }
        [EmailAddress] public string EmailAddress { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public List<Account> Accounts { get; set; } 
        public List<SelectListItem> AllCountries { get; set; }
        public List<SelectListItem> AllCountryCodes { get; set; }
        public List<SelectListItem> AllTelCodes { get; set; }
        
        public void OnGet()
        {
            Birthday = DateTime.Now;
            AllCountries = _setListsService.SetAllCountries();
            AllCountryCodes = _setListsService.SetAllCountryCodes();
            AllTelCodes = _setListsService.SetAllTelCodes();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var customer = new Data.Customer
                {
                    Givenname = Givenname,
                    Surname = Surname,
                    Streetaddress = Streetaddress,
                    City = City,
                    Zipcode = Zipcode,
                    Country = CountryId,
                    CountryCode = CountryCodeId,
                    NationalId = NationalId,
                    TelephoneCountryCode = TelCodeId,
                    Telephone = Telephone,
                    EmailAddress = EmailAddress,
                    Birthday = Birthday
                };
                var account = new Data.Account
                {
                    AccountType = "Checking",
                    Created = DateTime.Now,
                    Balance = 0
                }; 
                customer.Accounts.Add(account);
               
                _context.Customers.Add(customer);
                _context.SaveChanges();
                int id = customer.Id;
                return RedirectToPage("CustomerDetails", new { customerid = id });
            }
            AllCountries = _setListsService.SetAllCountries();
            AllCountryCodes = _setListsService.SetAllCountryCodes();
            AllTelCodes = _setListsService.SetAllTelCodes();
            return Page();
        }
    }
}
