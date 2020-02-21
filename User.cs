using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;

namespace SuncoastBank
{
  public class User
  {
    // User has a list for each type of account
    public List<Account> Accounts { get; set; } = new List<Account>();

    // User has a name
    public string UserName { get; set; }

    // User has a unique password
    public string Password { get; set; }

    // UNIQUE ACCOUNT ID
    public int AccountNumberID { get; set; } = 1;

    // ***** METHODS *****
    // CREATE ACCOUNT
    public void AddAccount(string name, double balance)
    {
      var account = new Account()
      {
        Name = name,
        Balance = balance,
        AccountNumber = AccountNumberID,
      };
      Accounts.Add(account);
      AccountNumberID++;
    }

    // DISPLAY ACCOUNTS
    public void ShowAccounts()
    {
      foreach (var account in Accounts)
      {
        Console.WriteLine("---------------------------");
        Console.WriteLine($"Name: {account.Name}");
        Console.WriteLine($"Balance: {account.Balance}");
        Console.WriteLine($"Account number: {account.AccountNumber}");
        Console.WriteLine("---------------------------");
      }
    }

    // Method to deposit money in account
    public void Deposit(int accountNum, double amount)
    {
      Accounts.First(acc => acc.AccountNumber == accountNum).Balance += amount;
    }

    // Method to withdraw money from account
    public void Withdraw(int accountNum, double amount)
    {
      Accounts.First(acc => acc.AccountNumber == accountNum).Balance -= amount;
    }

    // TRANSFER
    public void Transfer(int accountNumToTransferFrom, int accountNumToTransferTo, double amount)
    {
      Withdraw(accountNumToTransferFrom, amount);
      Deposit(accountNumToTransferTo, amount);
    }

    // REMOVE ACCOUNT
    public void RemoveAccount(int accountNum)
    {
      var accountToRemove = Accounts.First(acc => acc.AccountNumber == accountNum);
      Accounts.Remove(accountToRemove);
    }

  }
}