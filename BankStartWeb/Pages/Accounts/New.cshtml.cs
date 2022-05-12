using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace BankStartWeb.Pages.Accounts
{
    [Authorize(Roles="Admin, Cashier")]
    public class NewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ISetListsService _setListsServices;
        private readonly IToastNotification _toastNotification;

        public NewModel(ApplicationDbContext context, ISetListsService setListsServices, IToastNotification toastNotification)
        {
            _context = context;
            _setListsServices = setListsServices;
            _toastNotification = toastNotification;
        }
        [BindProperty]
        public string AccountTypeId { get; set; }
        public DateTime Created { get; set; }
        public string Fullname { get; set; }
        [BindProperty]
        public int CustomerId { get; set; }
        [BindProperty]
        public List<SelectListItem> AllAccountTypes { get; set; }

        public int CreateAccount(int customerId)
        {
            var customer = _context.Customers.First(c => c.Id == customerId);

            var account = new Account();
            {
                account.AccountType = AccountTypeId;
                account.Created = DateTime.Now;
                account.Balance = 0;
            }
            customer.Accounts.Add(account);
            _context.SaveChanges();
            _toastNotification.AddSuccessToastMessage("Account created!");

            int id = account.Id;
            return id;
        }
        public void OnGet(int customerId)
        {
            var customer = _context.Customers.First(c => c.Id == customerId);

            CustomerId = customer.Id;
            Fullname = customer.Givenname + " " + customer.Surname;
            AllAccountTypes = _setListsServices.SetAllAccountTypes();
        }

        public IActionResult OnPost(int customerId)
        {
            var customer = _context.Customers.First(c => c.Id == customerId);

            CustomerId = customer.Id;
            Fullname = customer.Givenname + " " + customer.Surname;
            AllAccountTypes = _setListsServices.SetAllAccountTypes();

            if (ModelState.IsValid)
            {
                int id = CreateAccount(customerId);
                return RedirectToPage("AccountDetails", new { accountid = id });
            }
            AllAccountTypes = _setListsServices.SetAllAccountTypes();
            return Page();
        }
    }
}
