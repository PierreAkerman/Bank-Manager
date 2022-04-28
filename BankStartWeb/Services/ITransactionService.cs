namespace BankStartWeb.Services
{
    public interface ITransactionService
    {
        ITransactionService.TransactionStatus MakeDeposit(int accountid, decimal amount);
        ITransactionService.TransactionStatus MakeWithdrawl(int accountid, decimal amount);
        ITransactionService.TransactionStatus MakePayment(int accountid, decimal amount);
        ITransactionService.TransactionStatus PaySalary(int accountid, decimal amount);
        ITransactionService.TransactionStatus MakeTransfer(int accountid, decimal amount, int targetAccountId);

        public enum TransactionStatus
        {
            Ok,
            NotPositiveAmount,
            InsufficientBalance
        }
    }
}