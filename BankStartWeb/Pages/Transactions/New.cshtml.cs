using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankStartWeb.Pages.Transactions
{
    [BindProperties]
    public class NewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ISetListsService _setListsService;

        public NewModel(ApplicationDbContext context,
                        ISetListsService setListsService)
        {
            _context = context;
            _setListsService = setListsService;
        }
        [MaxLength(10)]
        public string Type { get; set; }
        public string TypeId { get; set; }
        [MaxLength(50)]
        public string Operation { get; set; }
        public string OperationId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal NewBalance { get; set; }
        
        public Account TargetAccount { get; set; }
        public List<SelectListItem> AllTransactionTypes { get; set; }
        public List<SelectListItem> AllTransactionOps { get; set; }

        public void OnGet(int accountid)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = customer.Accounts.First(a => a.Id == accountid);

            AllTransactionTypes = _setListsService.SetAllTransactionTypes();
            AllTransactionOps = _setListsService.SetAllTransactionOperations();
            

        }
        public IActionResult OnPost(int accountid)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = customer.Accounts.First(a => a.Id == accountid);

            var transaction = new Transaction
            {
                Type = TypeId,
                Operation = OperationId,
                Date = DateTime.Now,
                Amount = Amount,
                NewBalance = NewBalance
            };
            account.Transactions.Add(transaction);
            
            _context.SaveChanges();
            return RedirectToPage("CustomerList");
        }
    }
}

