using System;
using System.Collections.Generic;
using System.Linq;

namespace SuncoastBank
{
  class Program
  {
    static void Main(string[] args)
    {
      var bankAccount = new Account();
      var user = new User();
      user.UserName = "Sam";
      var errorMessage = $"Apologies, {user.UserName}, that is not a valid input. Please try again.";
      // Add test cases
      user.AddAccount("savings", 50.50);
      user.AddAccount("checkings", 100.50);
      var isRunning = true;
      while (isRunning)
      {
        // Display different accounts to user
        user.ShowAccounts();

        // Ask user what they would like to do
        Console.WriteLine("What would you like to do today?");
        Console.WriteLine("(DEPOSIT 'd'), (WITHDRAW 'w'), (TRANSFER 't'), or (QUIT 'q')");
        var input = Console.ReadLine().ToLower();
        // Add space
        Console.WriteLine();
        switch (input)
        {
          // * * * * * D E P O S I T * * * * *
          case "d":
            Console.WriteLine("Please select an account to deposit:");
            user.ShowAccounts();
            int depositAccount;
            int.TryParse(Console.ReadLine(), out depositAccount);

            // VALIDATE
            while (!user.Accounts.Any(acc => acc.AccountNumber == depositAccount))
            {
              Console.WriteLine(errorMessage);
              int.TryParse(Console.ReadLine(), out depositAccount);
            }

            Console.WriteLine();
            // Ask user how much to deposit
            Console.WriteLine($"How much would you like to deposit in {user.Accounts.First(acc => acc.AccountNumber == depositAccount).Name}?");
            // Initialize variable result 
            double amountToDeposit;
            // take in user input, parse to double, and output the result variable above as a double
            double.TryParse(Console.ReadLine(), out amountToDeposit);

            while (amountToDeposit <= 0)
            {
              Console.WriteLine(errorMessage);
              double.TryParse(Console.ReadLine(), out amountToDeposit);
            }
            // Call method to deposit money into account
            user.Deposit(depositAccount, amountToDeposit);
            break;

          // * * * * * W I T H D R A W * * * * *
          case "w":
            Console.WriteLine("Please select an account to withdraw:");
            user.ShowAccounts();
            // Initialize variable
            int withdrawAccount;
            int.TryParse(Console.ReadLine(), out withdrawAccount);

            while (!user.Accounts.Any(acc => acc.AccountNumber == withdrawAccount))
            {
              Console.WriteLine(errorMessage);
              int.TryParse(Console.ReadLine(), out depositAccount);
            }

            Console.WriteLine();
            // Ask user how much to deposit
            Console.WriteLine($"How much would you like to withdraw from {user.Accounts.First(acc => acc.AccountNumber == withdrawAccount).Name}?");
            // Initialize variable result 
            double amountToWithdraw;
            // take in user input, parse to double, and output the result variable above as a double
            double.TryParse(Console.ReadLine(), out amountToWithdraw);

            while (amountToWithdraw <= 0 || amountToWithdraw > user.Accounts.First(acc => acc.AccountNumber == withdrawAccount).Balance)
            {
              Console.WriteLine(errorMessage);
              double.TryParse(Console.ReadLine(), out amountToWithdraw);
            }

            // Call method to withdraw amount from account
            user.Withdraw(withdrawAccount, amountToWithdraw);
            break;

          // * * * * * T R A N S F E R * * * * *
          case "t":
            // ***** ASK USER WHICH ACCOUNT TO TRANSFER FROM
            Console.WriteLine("Please select an account to transfer from:");
            user.ShowAccounts();
            int transferAccount;
            int.TryParse(Console.ReadLine(), out transferAccount);
            // VALIDATE INPUT
            while (!user.Accounts.Any(acc => acc.AccountNumber == transferAccount))
            {
              Console.WriteLine(errorMessage);
              int.TryParse(Console.ReadLine(), out transferAccount);
            }
            Console.WriteLine();

            // **** WHICH ACCOUNT TO TRANSFER TO *****
            Console.WriteLine("Please select an account to transfer to:");
            user.ShowAccounts();
            int incomingAccount;
            int.TryParse(Console.ReadLine(), out incomingAccount);
            // VALIDATE INPUT
            while (!user.Accounts.Any(acc => acc.AccountNumber == incomingAccount) || transferAccount == incomingAccount)
            {
              Console.WriteLine(errorMessage);
              int.TryParse(Console.ReadLine(), out incomingAccount);
            }
            Console.WriteLine();

            // Ask user how much to deposit
            Console.WriteLine($"How much would you like to transfer from {user.Accounts.First(acc => acc.AccountNumber == transferAccount).Name} to {user.Accounts.First(acc => acc.AccountNumber == incomingAccount).Name}?");
            // Initialize variable result 
            double amountToTransfer;
            // take in user input, parse to double, and output the result variable above as a double
            double.TryParse(Console.ReadLine(), out amountToTransfer);

            while (amountToTransfer <= 0 || amountToTransfer > user.Accounts.First(acc => acc.AccountNumber == transferAccount).Balance)
            {
              Console.WriteLine(errorMessage);
              double.TryParse(Console.ReadLine(), out amountToTransfer);
            }
            // Call method to transfer
            user.Transfer(transferAccount, incomingAccount, amountToTransfer);
            break;

          // * * * * * Q U I T * * * * *
          case "q":
            isRunning = false;
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"Enjoy the rest of your day, {user.UserName}.");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            break;

          // DEFAULT
          default:
            Console.WriteLine(errorMessage);
            break;
        }


      }
    }
  }

}