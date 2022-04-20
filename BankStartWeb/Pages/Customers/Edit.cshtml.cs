using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
#nullable disable
namespace BankStartWeb.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ISetListsService _setListsService;

        public EditModel(ApplicationDbContext context, ISetListsService setListsService)
        {
            _context = context;
            _setListsService = setListsService;
        }
        [BindProperty]
        [MaxLength(50)] public string Givenname { get; set; }
        [BindProperty]
        [MaxLength(50)] public string Surname { get; set; }
        [BindProperty]
        [MaxLength(50)] public string Streetaddress { get; set; }
        [BindProperty]
        [MaxLength(50)] public string City { get; set; }
        [BindProperty]
        [MaxLength(10)] public string Zipcode { get; set; }
        [BindProperty]
        public string CountryId { get; set; }
        [BindProperty]
        public string CountryCodeId { get; set; }
        [BindProperty]
        [MaxLength(20)] public string NationalId { get; set; }
        [BindProperty]
        public int TelCodeId { get; set; }
        [BindProperty]
        [MaxLength(50)] public string Telephone { get; set; }
        [BindProperty]
        [EmailAddress] public string EmailAddress { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        [BindProperty]
        public List<SelectListItem> AllCountries { get; set; }
        [BindProperty]
        public List<SelectListItem> AllCountryCodes { get; set; }
        [BindProperty]
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

            AllCountries = _setListsService.SetAllCountries();
            AllCountryCodes = _setListsService.SetAllCountryCodes();
            AllTelCodes = _setListsService.SetAllTelCodes();
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
                return RedirectToPage("CustomerDetails", new { customerid = customer.Id });
            }
            AllCountries = _setListsService.SetAllCountries();
            AllCountryCodes = _setListsService.SetAllCountryCodes();
            AllTelCodes = _setListsService.SetAllTelCodes();
            return Page();
        }
    }
}
