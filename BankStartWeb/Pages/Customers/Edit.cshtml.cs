
#nullable disable
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace BankStartWeb.Pages.Customers
{
    [Authorize(Roles = "Admin, Cashier")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ISetListsService _setListsService;

        public EditModel(ApplicationDbContext context, ISetListsService setListsService)
        {
            _context = context;
            _setListsService = setListsService;
        }

        [Required(ErrorMessage = "The Name field is required.")]
        [BindProperty]
        [MaxLength(50)]
        public string Givenname { get; set; }
        [Required(ErrorMessage = "The Surname field is required.")]
        [BindProperty]
        [MaxLength(50)]
        public string Surname { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [BindProperty]
        [MaxLength(50)]
        public string Streetaddress { get; set; }
        [Required]
        [BindProperty]
        [MaxLength(50)]
        public string City { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [BindProperty]
        [MaxLength(10)]
        public string Zipcode { get; set; }
        [Required(ErrorMessage = "The Country field is required.")]
        [BindProperty]
        public string CountryId { get; set; }
        [Required(ErrorMessage = "The Country Id field is required.")]
        [BindProperty]
        public string CountryCodeId { get; set; }
        [Required(ErrorMessage = "The National Id field is required.")]
        [BindProperty]
        [MinLength(8)]
        [MaxLength(13)]
        public string NationalId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [BindProperty]
        public int TelCodeId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [BindProperty]
        [MinLength(6)]
        [MaxLength(20)]
        public string Telephone { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "The Email field is required.")]
        [MaxLength(25)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "This field is required.")]
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
