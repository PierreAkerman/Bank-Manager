using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.Accounts
{
    public class TransferModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ISetListsService _setListsService;

        public TransferModel(ApplicationDbContext context,
                                ISetListsService setListsService)
        {
            _context = context;
            _setListsService = setListsService;
        }
        [BindProperty]
        public Account TargetAccount { get; set; }
        public string TransactionType { get; set; }
        public string TransactionOperation { get; set; }
        [BindProperty]
        public string OperationId { get; set; }
        public DateTime Date { get; set; }
        [BindProperty]
        public decimal Amount { get; set; }
        public decimal NewBalance { get; set; }
        [BindProperty]
        public List<SelectListItem> AllTransactionOps { get; set; }

        public void OnGet(int accountid)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = customer.Accounts.First(a => a.Id == accountid);

            AllTransactionOps = _setListsService.SetAllTransactionOperations();
        }
        public IActionResult OnPost(int accountid, decimal amount, int targetAccount)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = customer.Accounts.First(a => a.Id == accountid);
            

            if (OperationId == "Payment")
            {
                var transaction = new Transaction
                {
                    Type = "Credit",
                    Operation = OperationId,
                    Date = DateTime.Now,
                    Amount = amount,
                    NewBalance = account.Balance - amount,
                };
                account.Balance -= amount;
                account.Transactions.Add(transaction);

                _context.SaveChanges();
                return RedirectToPage("AccountDetails", new { accountid });
            }
            else if (OperationId == "Salary")
            {
                var transaction = new Transaction
                {
                    Type = "Debit",
                    Operation = OperationId,
                    Date = DateTime.Now,
                    Amount = amount,
                    NewBalance = account.Balance + amount,
                };
                account.Balance += amount;
                account.Transactions.Add(transaction);

                _context.SaveChanges();
                return RedirectToPage("AccountDetails", new { accountid });
            }
            else if (OperationId == "Transfer")
            {
                var transaction = new Transaction
                {
                    Type = "Credit",
                    Operation = OperationId,
                    Date = DateTime.Now,
                    Amount = amount,
                    NewBalance = account.Balance - amount,
                };
                account.Balance -= amount;
                account.Transactions.Add(transaction);

                TargetAccount = _context.Accounts.First(a => a.Id == targetAccount);

                var targetTransaction = new Transaction
                {
                    Type = "Debit",
                    Operation = OperationId,
                    Date = DateTime.Now,
                    Amount = amount,
                    NewBalance = TargetAccount.Balance + amount,
                }; 

                TargetAccount.Balance += amount;
                TargetAccount.Transactions.Add(targetTransaction);

                _context.SaveChanges();
                return RedirectToPage("AccountDetails", new { accountid });
            }
            AllTransactionOps = _setListsService.SetAllTransactionOperations();
            return Page();
        }
    }
}
