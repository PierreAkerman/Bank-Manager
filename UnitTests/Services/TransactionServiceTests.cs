using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Services
{
    [TestClass]
    public class TransactionServiceTests
    {
        private ApplicationDbContext _context;
        private readonly TransactionService _sut;

        public TransactionServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _sut = new TransactionService(_context);
        }
        public void CreateAccount(decimal balance)
        {
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = balance,
                Transactions = new List<Transaction>()
            });
            _context.SaveChanges();
        }
        ///----------------------------------------------------------------------------
        ///--------------------------- DEPOSIT TESTS ----------------------------------
        ///----------------------------------------------------------------------------
        [TestMethod]
        public void When_positive_amount_Deposit_should_return_ok()
        {
            CreateAccount(0);

            var result = _sut.MakeDeposit(1, 1000);
            Assert.AreEqual(ITransactionService.TransactionStatus.Ok, result);
        }
        [TestMethod]
        public void When_not_positive_amount_Deposit_should_return_notPositiveAmount()
        {
            CreateAccount(0);

            var result = _sut.MakeDeposit(1, -500);
            Assert.AreEqual(ITransactionService.TransactionStatus.NotPositiveAmount, result);
        }
        [TestMethod]
        public void When_making_Deposit_should_create_Transaction()
        {
            CreateAccount(0);

            _sut.MakeDeposit(1, 1000);

            var account = _context.Accounts.Include(a => a.Transactions)
                .First(a => a.Id == 1);

            var transaction = account.Transactions.Last();
            Assert.AreEqual("Deposit cash", transaction.Operation);
        }
        ///----------------------------------------------------------------------------
        ///-------------------------- WITHDRAWL TESTS ---------------------------------
        ///----------------------------------------------------------------------------
        [TestMethod]
        public void When_positive_amount_Withdrawl_should_return_ok()
        {
            CreateAccount(2000);

            var result = _sut.MakeWithdrawl(1, 1000);
            Assert.AreEqual(ITransactionService.TransactionStatus.Ok, result);
        }
        [TestMethod]
        public void When_not_positive_amount_Withdrawl_should_return_notPositiveAmount()
        {
            CreateAccount(2000);

            var result = _sut.MakeWithdrawl(1, 0);
            Assert.AreEqual(ITransactionService.TransactionStatus.NotPositiveAmount, result);
        }
        [TestMethod]
        public void When_insufficient_balance_Withdrawl_should_return_insufficientBalance()
        {
            CreateAccount(2000);

            var result = _sut.MakeWithdrawl(1, 2500);
            Assert.AreEqual(ITransactionService.TransactionStatus.InsufficientBalance, result);
        }
        [TestMethod]
        public void When_making_Withdrawl_should_create_Transaction()
        {
            CreateAccount(1000);

            _sut.MakeWithdrawl(1, 500);

            var account = _context.Accounts.Include(a => a.Transactions)
                .First(a => a.Id == 1);

            var transaction = account.Transactions.Last();
            Assert.AreEqual("ATM withdrawls", transaction.Operation);
        }
        ///----------------------------------------------------------------------------
        ///--------------------------- PAYMENT TESTS ----------------------------------
        ///----------------------------------------------------------------------------
        [TestMethod]
        public void When_positive_amount_Payment_should_return_ok()
        {
            CreateAccount(2000);

            var result = _sut.MakePayment(1, 1000);
            Assert.AreEqual(ITransactionService.TransactionStatus.Ok, result);
        }
        [TestMethod]
        public void When_not_positive_amount_Payment_should_return_notPositiveAmount()
        {
            CreateAccount(2000);

            var result = _sut.MakePayment(1, 0);
            Assert.AreEqual(ITransactionService.TransactionStatus.NotPositiveAmount, result);
        }
        [TestMethod]
        public void When_insufficient_balance_Payment_should_return_insufficientBalance()
        {
            CreateAccount(2000);

            var result = _sut.MakePayment(1, 3000);
            Assert.AreEqual(ITransactionService.TransactionStatus.InsufficientBalance, result);
        }
        [TestMethod]
        public void When_making_Payment_should_create_Transaction()
        {
            CreateAccount(1000);

            _sut.MakePayment(1, 500);

            var account = _context.Accounts.Include(a => a.Transactions)
                .First(a => a.Id == 1);

            var transaction = account.Transactions.Last();
            Assert.AreEqual("Payment", transaction.Operation);
        }
        ///----------------------------------------------------------------------------
        ///--------------------------- SALARY TESTS -----------------------------------
        ///----------------------------------------------------------------------------
        [TestMethod]
        public void When_positive_amount_Salary_should_return_ok()
        {
            CreateAccount(0);

            var result = _sut.PaySalary(1, 1000);
            Assert.AreEqual(ITransactionService.TransactionStatus.Ok, result);
        }
        [TestMethod]
        public void When_not_positive_amount_Salary_should_return_notPositiveAmount()
        {
            CreateAccount(0);

            var result = _sut.PaySalary(1, 0);
            Assert.AreEqual(ITransactionService.TransactionStatus.NotPositiveAmount, result);
        }
        [TestMethod]
        public void When_making_PaySalary_should_create_Transaction()
        {
            CreateAccount(0);

            _sut.PaySalary(1, 1000);

            var account = _context.Accounts.Include(a => a.Transactions)
                .First(a => a.Id == 1);

            var transaction = account.Transactions.Last();
            Assert.AreEqual("Salary", transaction.Operation);
        }
        ///----------------------------------------------------------------------------
        ///-------------------------- TRANSFER TESTS ----------------------------------
        ///----------------------------------------------------------------------------
        [TestMethod]
        public void When_positive_amount_Transfer_should_return_ok()
        {
            CreateAccount(1000);
            CreateAccount(0);

            var result = _sut.MakeTransfer(1, 500, 2);
            Assert.AreEqual(ITransactionService.TransactionStatus.Ok, result);
        }
        [TestMethod]
        public void When_not_positive_amount_Transfer_should_return_notPositiveAmount()
        {
            CreateAccount(1000);
            CreateAccount(0);

            var result = _sut.MakeTransfer(1, -1000, 2);
            Assert.AreEqual(ITransactionService.TransactionStatus.NotPositiveAmount, result);
        }
        [TestMethod]
        public void When_insufficient_balance_Transfer_should_return_insufficientBalance()
        {
            CreateAccount(1000);
            CreateAccount(0);

            var result = _sut.MakeTransfer(1, 2000, 2);
            Assert.AreEqual(ITransactionService.TransactionStatus.InsufficientBalance, result);
        }
        [TestMethod]
        public void When_making_Transfer_should_create_Transaction()
        {
            CreateAccount(1000);
            CreateAccount(0);

            _sut.MakeTransfer(1, 500, 2);

            var sendingAccount = _context.Accounts.Include(a => a.Transactions)
                .First(a => a.Id == 1);
            var receivingAccount = _context.Accounts.Include(a => a.Transactions)
                .First(a => a.Id == 2);

            var senderTransaction = sendingAccount.Transactions.Last();
            Assert.AreEqual("Transfer", senderTransaction.Operation);
            Assert.AreEqual("Credit", senderTransaction.Type);

            var receiverTransaction = receivingAccount.Transactions.Last();
            Assert.AreEqual("Transfer", senderTransaction.Operation);
            Assert.AreEqual("Debit", receiverTransaction.Type);
        }
        [TestMethod]
        public void When_receiving_accountID_is_NOT_correct_Transfer_should_return_IncorrectTargetAccountId()
        {
            CreateAccount(1000);
            CreateAccount(0);

            var result = _sut.MakeTransfer(1, 500, 50000);
            Assert.AreEqual(ITransactionService.TransactionStatus.IncorrectTargetAccountId, result);
        }
    }
}