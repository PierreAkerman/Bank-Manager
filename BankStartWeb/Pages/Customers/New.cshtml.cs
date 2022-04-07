using BankStartWeb.Data;
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

        public NewModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [Required]
        [MaxLength(50)] public string Givenname { get; set; }
        [Required]
        [MaxLength(50)] public string Surname { get; set; }
        [Required]
        [MaxLength(50)] public string Streetaddress { get; set; }
        [Required]
        [MaxLength(50)] public string City { get; set; }
        [Required]
        [MaxLength(10)] public string Zipcode { get; set; }
        [Required]
        [MaxLength(30)] public string Country { get; set; }
        [Required]
        [MaxLength(2)] public string CountryCode { get; set; }
        [Required]
        [MaxLength(20)] public string NationalId { get; set; }
        [Range(0, 9999)]
        public int TelephoneCountryCode { get; set; }
        public string Telephone { get; set; }
        [MaxLength(50)]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<SelectListItem> AllCountries { get; set; }
        public List<SelectListItem> AllCountryCodes { get; set; }
        public List<SelectListItem> AllTelCodes { get; set; }

        public string CountryId { get; set; }
        public string CountryCodeId { get; set; }
        public int TelCodeId { get; set; }

        public void SetAllCountries()
        {
            AllCountries = Enum.GetValues<Countries>().Select(c => new SelectListItem
            {
                Text = c.ToString(),
                Value = c.ToString()

            }).ToList();

            AllCountries.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Please select a country"
            });
        }
        public void SetAllCountryCodes()
        {
            AllCountryCodes = Enum.GetValues<CountryCodes>().Select(c => new SelectListItem
            {
                Text = c.ToString(),
                Value = c.ToString()

            }).ToList();

            AllCountryCodes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Please select a country code"
            });
        }
        public void SetAllTelCodes()
        {
            AllTelCodes = new TelCountryCode().TelCodes.Select(c => new SelectListItem
            {
                Text = c.ToString(),
                Value = c.ToString()

            }).ToList();

            AllTelCodes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Please select a country code"
            });
        }

        public void OnGet()
        {
            Birthday = DateTime.Now;
            SetAllCountries();
            SetAllTelCodes();
            SetAllCountryCodes();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var customer = new Data.Customer();
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

                _context.Customers.Add(customer);
                _context.SaveChanges();

                //Spara i databas
                return RedirectToPage("CustomerList");
            }
            //Visa felen och rita om formuläret
            SetAllCountries();
            SetAllTelCodes();
            SetAllCountryCodes();
            return Page();
        }
    }
}
