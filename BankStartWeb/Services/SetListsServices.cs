using BankStartWeb.Data;
using BankStartWeb.Pages.Accounts;
using BankStartWeb.Pages.Customers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankStartWeb.Services
{
    public interface ISetListsServices
    {
        List<SelectListItem> SetAllCountries();
        List<SelectListItem> SetAllCountryCodes();
        List<SelectListItem> SetAllTelCodes();
        List<SelectListItem> SetAllAccountTypes();
    }

    public class SetListsServices : ISetListsServices
    {
        private readonly ApplicationDbContext _context;

        public SetListsServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<SelectListItem> SetAllCountries()
        {
            var allCountries = Enum.GetValues<Countries>().Select(c => new SelectListItem
            {
                Text = c.ToString(),
                Value = c.ToString()

            }).ToList();

            allCountries.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Please select a country"
            });
            return allCountries;
        }
        public List<SelectListItem> SetAllCountryCodes()
        {
            var allCountryCodes = Enum.GetValues<CountryCodes>().Select(c => new SelectListItem
            {
                Text = c.ToString(),
                Value = c.ToString()

            }).ToList();

            allCountryCodes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Please select a country code"
            });
            return allCountryCodes;
        }
        public List<SelectListItem> SetAllTelCodes()
        {
            var allTelCodes = new TelCountryCode().TelCodes.Select(c => new SelectListItem
            {
                Text = c.ToString(),
                Value = c.ToString()

            }).ToList();

            allTelCodes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Please select a telephone country code"
            });
            return allTelCodes;
        }
        public List<SelectListItem> SetAllAccountTypes()
        {
            var allAccountTypes = Enum.GetValues<AccountTypes>().Select(c => new SelectListItem
            {
                Text= c.ToString(),
                Value= c.ToString()
            }).ToList();

            allAccountTypes.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Please select account type"
            });
            return allAccountTypes;
        }
    }
}
