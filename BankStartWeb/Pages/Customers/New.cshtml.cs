using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using NToastNotify;

namespace BankStartWeb.Pages.Customers
{
    public class NewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ISetListsService _setListsService;
        private readonly IToastNotification _toastNotification;

        public NewModel(ApplicationDbContext context,
                         ISetListsService setListsService,
                            IToastNotification toastNotification)
        {
            _context = context;
            _setListsService = setListsService;
            _toastNotification = toastNotification;
        }

        [Required(ErrorMessage = "The Name field is required.")]
        [BindProperty]
        [MaxLength(50)] 
        public string Givenname { get; set; }
        [Required(ErrorMessage = "The Surname field is required.")]
        [BindProperty]
        [MaxLength(50)] 
        public string Surname { get; set; }
        [BindProperty]
        [MaxLength(50)] 
        public string? Streetaddress { get; set; }
        [BindProperty]
        [MaxLength(50)] 
        public string? City { get; set; }
        [BindProperty]
        [MaxLength(10)] 
        public string? Zipcode { get; set; }
        [Required(ErrorMessage = "The Country field is required.")]
        [BindProperty]
        public string CountryId { get; set; }
        [Required(ErrorMessage = "The Country Id field is required.")]
        [BindProperty]
        public string CountryCodeId { get; set; }
        [Required(ErrorMessage = "The National Id field is required.")]
        [BindProperty]
        [MinLength(8)]
        [MaxLength(12)] 
        public string NationalId { get; set; }
        [BindProperty]
        public int? TelCodeId { get; set; }
        [BindProperty]
        [MinLength(6)]
        [MaxLength(20)] 
        public string? Telephone { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "The Email field is required.")]
        [MaxLength(25)]
        [EmailAddress] 
        public string EmailAddress { get; set; }
        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
        
        [BindProperty]
        public List<SelectListItem> AllCountries { get; set; }
        [BindProperty]
        public List<SelectListItem> AllCountryCodes { get; set; }
        [BindProperty]
        public List<SelectListItem> AllTelCodes { get; set; }

        public int CreateCustomer()
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
                TelephoneCountryCode = (int)TelCodeId,
                Telephone = Telephone,
                EmailAddress = EmailAddress,
                Birthday = (DateTime)Birthday
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
            _toastNotification.AddSuccessToastMessage("Customer created!");
            int id = customer.Id;
            return id;
        }
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
                int id = CreateCustomer();
                return RedirectToPage("CustomerDetails", new { customerid = id });
            }
            AllCountries = _setListsService.SetAllCountries();
            AllCountryCodes = _setListsService.SetAllCountryCodes();
            AllTelCodes = _setListsService.SetAllTelCodes();
            return Page();
        }
    }
}
