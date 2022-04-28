﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankStartWeb.Data;
using BankStartWeb.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;
            _context = new ApplicationDbContext(options);
            _sut = new TransactionService(_context);
        }

        [TestMethod]
        public void When_positive_amount_Deposit_should_return_ok()
        {
            _context.Customers.Add(new Customer
            {
                Givenname = "Kalle",
                Surname = "Anka",
                Streetaddress = "Ankgatan",
                City = "Ankeborg",
                Zipcode = "12345",
                Country = "Sverige",
                CountryCode = "SE",
                NationalId = "19900102-1234",
                TelephoneCountryCode = 46,
                Telephone = "0707070707",
                EmailAddress = "kalle@anka@hotmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>(),
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 0,
                Transactions = new List<Transaction>()
            });
            _context.SaveChanges();

            var result = _sut.MakeDeposit(1,1000);
            Assert.AreEqual(ITransactionService.TransactionStatus.Ok, result);
        }
        [TestMethod]
        public void When_not_positive_amount_Deposit_should_return_notPositiveAmount()
        {
            _context.Customers.Add(new Customer
            {
                Givenname = "Kalle",
                Surname = "Anka",
                Streetaddress = "Streetaddress",
                City = "City",
                Zipcode = "12345",
                Country = "Sverige",
                CountryCode = "SE",
                NationalId = "19900102-1234",
                TelephoneCountryCode = 46,
                Telephone = "0707070707",
                EmailAddress = "kalle@anka@hotmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>(),
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 0,
                Transactions = new List<Transaction>()
            });
            _context.SaveChanges();

            var result = _sut.MakeDeposit(1, -500);
            Assert.AreEqual(ITransactionService.TransactionStatus.NotPositiveAmount, result);
        }
        [TestMethod]
        public void When_positive_amount_Withdrawl_should_return_ok()
        {
            _context.Customers.Add(new Customer
            {
                Givenname = "Kalle",
                Surname = "Anka",
                Streetaddress = "Ankgatan",
                City = "Ankeborg",
                Zipcode = "12345",
                Country = "Sverige",
                CountryCode = "SE",
                NationalId = "19900102-1234",
                TelephoneCountryCode = 46,
                Telephone = "0707070707",
                EmailAddress = "kalle@anka@hotmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>(),
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 2000,
                Transactions = new List<Transaction>()
            });
            _context.SaveChanges();

            var result = _sut.MakeWithdrawl(1, 1000);
            Assert.AreEqual(ITransactionService.TransactionStatus.Ok, result);
        }
        [TestMethod]
        public void When_not_positive_amount_Withdrawl_should_return_notPositiveAmount()
        {
            _context.Customers.Add(new Customer
            {
                Givenname = "Kalle",
                Surname = "Anka",
                Streetaddress = "Ankgatan",
                City = "Ankeborg",
                Zipcode = "12345",
                Country = "Sverige",
                CountryCode = "SE",
                NationalId = "19900102-1234",
                TelephoneCountryCode = 46,
                Telephone = "0707070707",
                EmailAddress = "kalle@anka@hotmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>(),
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 2000,
                Transactions = new List<Transaction>()
            });
            _context.SaveChanges();

            var result = _sut.MakeWithdrawl(1, 0);
            Assert.AreEqual(ITransactionService.TransactionStatus.NotPositiveAmount, result);
        }
        [TestMethod]
        public void When_insufficient_balance_Withdrawl_should_return_insufficientBalance()
        {
            _context.Customers.Add(new Customer
            {
                Givenname = "Kalle",
                Surname = "Anka",
                Streetaddress = "Ankgatan",
                City = "Ankeborg",
                Zipcode = "12345",
                Country = "Sverige",
                CountryCode = "SE",
                NationalId = "19900102-1234",
                TelephoneCountryCode = 46,
                Telephone = "0707070707",
                EmailAddress = "kalle@anka@hotmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>(),
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 2000,
                Transactions = new List<Transaction>()
            });
            _context.SaveChanges();

            var result = _sut.MakeWithdrawl(1, 2500);
            Assert.AreEqual(ITransactionService.TransactionStatus.InsufficientBalance, result);
        }
        [TestMethod]
        public void When_positive_amount_Transfer_should_return_ok()
        {
            _context.Customers.Add(new Customer
            {
                Givenname = "Kalle",
                Surname = "Anka",
                Streetaddress = "Streetaddress",
                City = "City",
                Zipcode = "12345",
                Country = "Sverige",
                CountryCode = "SE",
                NationalId = "19900102-1234",
                TelephoneCountryCode = 46,
                Telephone = "0707070707",
                EmailAddress = "kalle@anka@hotmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>(),
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 1000,
                Transactions = new List<Transaction>()
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 0,
                Transactions = new List<Transaction>()
            });
            _context.SaveChanges();

            var result = _sut.MakeTransfer(1, 500, 2);
            Assert.AreEqual(ITransactionService.TransactionStatus.Ok, result);
        }
        [TestMethod]
        public void When_not_positive_amount_Transfer_should_return_notPositiveAmount()
        {
            _context.Customers.Add(new Customer
            {
                Givenname = "Kalle",
                Surname = "Anka",
                Streetaddress = "Streetaddress",
                City = "City",
                Zipcode = "12345",
                Country = "Sverige",
                CountryCode = "SE",
                NationalId = "19900102-1234",
                TelephoneCountryCode = 46,
                Telephone = "0707070707",
                EmailAddress = "kalle@anka@hotmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>(),
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 1000,
                Transactions = new List<Transaction>()
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 0,
                Transactions = new List<Transaction>()
            });
            _context.SaveChanges();

            var result = _sut.MakeTransfer(1, -1000, 2);
            Assert.AreEqual(ITransactionService.TransactionStatus.NotPositiveAmount, result);
        }
        [TestMethod]
        public void When_insufficient_balance_Transfer_should_return_insufficientBalance()
        {
            _context.Customers.Add(new Customer
            {
                Givenname = "Kalle",
                Surname = "Anka",
                Streetaddress = "Streetaddress",
                City = "City",
                Zipcode = "12345",
                Country = "Sverige",
                CountryCode = "SE",
                NationalId = "19900102-1234",
                TelephoneCountryCode = 46,
                Telephone = "0707070707",
                EmailAddress = "kalle@anka@hotmail.com",
                Birthday = DateTime.Now,
                Accounts = new List<Account>(),
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 1000,
                Transactions = new List<Transaction>()
            });
            _context.Accounts.Add(new Account
            {
                AccountType = "Savings",
                Created = DateTime.Now,
                Balance = 0,
                Transactions = new List<Transaction>()
            });
            _context.SaveChanges();

            var result = _sut.MakeTransfer(1, 2000, 2);
            Assert.AreEqual(ITransactionService.TransactionStatus.InsufficientBalance, result);
        }
    }
}
