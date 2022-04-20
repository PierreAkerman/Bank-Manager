using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Pages.Accounts
{
    public class NewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ISetListsService _setListsServices;

        public NewModel(ApplicationDbContext context, ISetListsService setListsServices)
        {
            _context = context;
            _setListsServices = setListsServices;
        }
        public int Id { get; set; }
        [BindProperty]
        public string AccountTypeId { get; set; }
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }
        public string Fullname { get; set; }

        public List<Transaction> Transactions { get; set; }
        [BindProperty]
        public List<SelectListItem> AllAccountTypes { get; set; }
        public void OnGet(int customerId)
        {
            var customer = _context.Customers.First(c => c.Id == customerId);
            Fullname = customer.Givenname + " " + customer.Surname;
            AllAccountTypes = _setListsServices.SetAllAccountTypes();
        }
        public IActionResult OnPost(int customerId)
        {
            if (ModelState.IsValid)
            {
                var customer = _context.Customers.First(c => c.Id == customerId);
                if (customer == null)
                {
                    return NotFound();
                }
                var account = new Account();
                {
                    account.AccountType = AccountTypeId;
                    account.Created = DateTime.Now;
                    account.Balance = 0;
                }
                customer.Accounts.Add(account);
                _context.SaveChanges();
                int id = account.Id;
                return RedirectToPage("AccountDetails", new { accountid = id });
            }
            AllAccountTypes = _setListsServices.SetAllAccountTypes();
            return Page();
        }
    }
}
