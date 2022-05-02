using System.ComponentModel.DataAnnotations;
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BankStartWeb.Pages.Transactions
{
    public class PaymentModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionService _transactionService;

        public PaymentModel(ApplicationDbContext context, ITransactionService transactionService)
        {
            _context = context;
            _transactionService = transactionService;
        }
        [BindProperty]
        public int Id { get; set; }
        public string AccountType { get; set; }
        [BindProperty]
        public decimal Balance  { get; set; }
        public string Type { get; set; }
        public string Operation { get; set; }
        public DateTime Date { get; set; }
        [Required]
        [BindProperty]
        public decimal Amount { get; set; }
        public decimal NewBalance { get; set; }
        public string Fullname { get; set; }
        public int CustomerId { get; set; }

        public void OnGet(int accountid)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = customer.Accounts.First(a => a.Id == accountid);

            Id = account.Id;
            CustomerId = customer.Id;
            Fullname = customer.Givenname + " " + customer.Surname;
            AccountType = account.AccountType;
            Balance = account.Balance;
        }
        public IActionResult OnPost(int accountid, decimal amount)
        {
            var customer = _context.Customers
                .Include(c => c.Accounts)
                .ThenInclude(c => c.Transactions)
                .First(c => c.Accounts.Any(a => a.Id == accountid));

            var account = _context.Accounts.First(a => a.Id == accountid);

            Balance = account.Balance;

            if (ModelState.IsValid)
            {
                var result = _transactionService.MakePayment(accountid, amount);

                if(result == ITransactionService.TransactionStatus.Ok)
                    return RedirectToPage("/Accounts/AccountDetails", new { accountid });

                else if (result == ITransactionService.TransactionStatus.NotPositiveAmount)
                {
                    ModelState.AddModelError(nameof(amount), "Enter a positive amount, please!");
                }
                else if (result == ITransactionService.TransactionStatus.InsufficientBalance)
                {
                    ModelState.AddModelError(nameof(Balance), "Not enough money!");
                }
            }
            Id = account.Id;
            CustomerId = customer.Id;
            Fullname = customer.Givenname + " " + customer.Surname;
            AccountType = account.AccountType;
            Balance = account.Balance;
            return Page();  
        }
    }
}
