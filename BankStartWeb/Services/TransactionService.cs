using BankStartWeb.Data;

namespace BankStartWeb.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public ITransactionService.TransactionStatus MakeDeposit(int accountid, decimal amount)
        {
            if (CheckValidAmount(amount) == false)
                return ITransactionService.TransactionStatus.NotPositiveAmount;

            var account = _context.Accounts.First(a => a.Id == accountid);
            account.Balance += amount;

            var transaction = new Transaction
            {
                Type = "Debit",
                Operation = "Deposit cash",
                Date = DateTime.Now,
                Amount = amount,
                NewBalance = account.Balance,
            };
            account.Transactions.Add(transaction);
            _context.SaveChanges();

            return ITransactionService.TransactionStatus.Ok;
        }

        public ITransactionService.TransactionStatus MakeWithdrawl(int accountid, decimal amount)
        {
            if (CheckValidAmount(amount) == false)
                return ITransactionService.TransactionStatus.NotPositiveAmount;

            var account = _context.Accounts.First(a => a.Id == accountid);

            if (CheckEnoughBalance(accountid, amount) == false)
                return ITransactionService.TransactionStatus.InsufficientBalance;

            account.Balance -= amount;

            var transaction = new Transaction
            {
                Type = "Credit",
                Operation = "ATM withdrawl",
                Date = DateTime.Now,
                Amount = amount,
                NewBalance = account.Balance,
            };
            account.Transactions.Add(transaction);
            _context.SaveChanges();

            return ITransactionService.TransactionStatus.Ok;
        }

        public ITransactionService.TransactionStatus MakePayment(int accountid, decimal amount)
        {
            if (CheckValidAmount(amount) == false)
                return ITransactionService.TransactionStatus.NotPositiveAmount;

            var account = _context.Accounts.First(a => a.Id == accountid);

            if (CheckEnoughBalance(accountid, amount) == false)
                return ITransactionService.TransactionStatus.InsufficientBalance;

            account.Balance -= amount;

            var transaction = new Transaction
            {
                Type = "Credit",
                Operation = "Payment",
                Date = DateTime.Now,
                Amount = amount,
                NewBalance = account.Balance,
            };
            account.Transactions.Add(transaction);
            _context.SaveChanges();

            return ITransactionService.TransactionStatus.Ok;
        }

        public ITransactionService.TransactionStatus PaySalary(int accountid, decimal amount)
        {
            if (CheckValidAmount(amount) == false)
                return ITransactionService.TransactionStatus.NotPositiveAmount;

            var account = _context.Accounts.First(a => a.Id == accountid);
            account.Balance += amount;

            var transaction = new Transaction
            {
                Type = "Debit",
                Operation = "Salary",
                Date = DateTime.Now,
                Amount = amount,
                NewBalance = account.Balance + amount,
            };
            account.Transactions.Add(transaction);
            _context.SaveChanges();

            return ITransactionService.TransactionStatus.Ok;
        }

        public ITransactionService.TransactionStatus MakeTransfer(int accountid, decimal amount, int targetAccountId)
        {
            if (CheckValidAmount(amount) == false)
                return ITransactionService.TransactionStatus.NotPositiveAmount;

            var account = _context.Accounts.First(a => a.Id == accountid);

            if (CheckEnoughBalance(accountid, amount) == false)
                return ITransactionService.TransactionStatus.InsufficientBalance;

            account.Balance -= amount;

            var transaction = new Transaction
            {
                Type = "Credit",
                Operation = "Transfer",
                Date = DateTime.Now,
                Amount = amount,
                NewBalance = account.Balance,
            };
            account.Transactions.Add(transaction);

            if (CheckTargetAccountId(targetAccountId) == false)
                return ITransactionService.TransactionStatus.IncorrectTargetAccountId;

            var targetAccount = _context.Accounts.First(a => a.Id == targetAccountId);
            targetAccount.Balance += amount;

            var targetTransaction = new Transaction
            {
                Type = "Debit",
                Operation = "Transfer",
                Date = DateTime.Now,
                Amount = amount,
                NewBalance = targetAccount.Balance,
            };
            targetAccount.Transactions.Add(targetTransaction);
            _context.SaveChanges();

            return ITransactionService.TransactionStatus.Ok;
        }
        private bool CheckValidAmount(decimal amount)
        {
            if (amount > 0)
                return true;
            return false;
        }
        private bool CheckEnoughBalance(int accountid, decimal amount)
        {
            var account = _context.Accounts.First(a => a.Id == accountid);
            if (amount <= account.Balance)
                return true;
            return false;
        }
        private bool CheckTargetAccountId(int targetAccountId)
        {
            var targetAccount = _context.Accounts.FirstOrDefault(a => a.Id == targetAccountId);
            if (targetAccount != null)
                return true;
            return false;
        }
    }
}