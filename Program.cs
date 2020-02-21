using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Globalization;
using Newtonsoft.Json;

namespace SuncoastBank
{
  class Program
  {
    static void Main(string[] args)
    {
      var bank = new Bank();
      var currentUser = new User();
      var user = new User();
      user.UserName = "Sam123";
      user.Password = "taco123";
      bank.Users.Add(user);
      var errorMessage = $"Apologies, {user.UserName}, that is not a valid input. Please try again.";
      // Add test cases
      user.AddAccount("Savings", 50.50);
      user.AddAccount("Checkings", 100.50);

      // var loggedIn = false;
      // while (loggedIn) {

      // }

      // SAVE DATA
      static void SaveData(Bank bank)
      {
        // var writer = new StreamWriter("bankData.csv");
        // var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
        // csvWriter.WriteRecords(Users);
        // writer.Flush();
        string json = JsonConvert.SerializeObject(bank);
        System.IO.File.WriteAllText(@"bankData.txt", json);
      }

      bank = JsonConvert.DeserializeObject<Bank>(File.ReadAllText(@"bankData.txt"));
      // READ THE DATA
      // var reader = new StreamReader("bankData.csv");
      // var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
      // bank.Users = csvReader.GetRecords<User>().ToList();

      var isRunning = true;
      while (isRunning)
      {
        // Display different accounts to user
        user.ShowAccounts();
        // Ask user what they would like to do
        Console.WriteLine("What would you like to do today?");
        Console.WriteLine("(DEPOSIT 'd'), (WITHDRAW 'w'), (TRANSFER 't'), (USER 'u') settings, or (QUIT 'q')");
        var input = Console.ReadLine().ToLower();
        // Add space
        Console.WriteLine();

        // Create switch statement for different conditions depending on input
        switch (input)
        {
          // * * * * * D E P O S I T * * * * *
          case "d":
            Console.WriteLine("Please select an account to deposit to:");
            // Refresh the user which accounts there are currently
            user.ShowAccounts();
            // Initialize variable result 
            int depositAccount;
            // take in user input, parse to double, and output the result to the variable above as a double
            int.TryParse(Console.ReadLine(), out depositAccount);
            // VALIDATE to make sure account number exists
            while (!user.Accounts.Any(acc => acc.AccountNumber == depositAccount))
            {
              Console.WriteLine(errorMessage);
              int.TryParse(Console.ReadLine(), out depositAccount);
            }
            // Add space
            Console.WriteLine();

            // Ask user how much to deposit
            Console.WriteLine($"How much would you like to deposit in {user.Accounts.First(acc => acc.AccountNumber == depositAccount).Name}?");
            // Initialize variable result 
            double amountToDeposit;
            // take in user input, parse to double, and output the result to the variable above as a double
            double.TryParse(Console.ReadLine(), out amountToDeposit);
            // Validate that amount user is depositing is greater than 0
            while (amountToDeposit <= 0)
            {
              Console.WriteLine(errorMessage);
              double.TryParse(Console.ReadLine(), out amountToDeposit);
            }
            // Call method to deposit money into account
            user.Deposit(depositAccount, amountToDeposit);
            Console.WriteLine($"You have successfully deposited ${amountToDeposit} into your {user.Accounts.First(acc => acc.AccountNumber == depositAccount).Name} account.");
            // Save data to file 
            SaveData(bank);
            break;

          // * * * * * W I T H D R A W * * * * *
          case "w":
            Console.WriteLine("Please select an account to withdraw from:");
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
            Console.WriteLine($"You have successfully withdrew ${amountToWithdraw} from your {user.Accounts.First(acc => acc.AccountNumber == withdrawAccount).Name} account.");
            // Save data to file 
            SaveData(bank);
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
            Console.WriteLine($"You have successfully transferred ${amountToTransfer} from your {user.Accounts.First(acc => acc.AccountNumber == transferAccount).Name} account into your {user.Accounts.First(acc => acc.AccountNumber == incomingAccount).Name}.");
            // Save data to file 
            SaveData(bank);
            break;

          // * * * * * USER S E T T I N G S * * * * *
          case "u":
            Console.WriteLine("What would you like to do in settings?");
            Console.WriteLine("(ADD 'a') account, (CLOSE 'x') account, (CHANGE 'c') password");
            var answer = Console.ReadLine().ToLower();
            while (answer != "a" && answer != "x" && answer != "c")
            {
              Console.WriteLine(errorMessage);
              answer = Console.ReadLine().ToLower();
            }
            // SWITCH STATEMENT FOR ANSWERS
            switch (answer)
            {
              // * * * * * A D D ACCOUNT * * * * *
              case "a":
                // Add space
                System.Console.WriteLine();
                Console.WriteLine($"{user.UserName}, you have chosen to add an account.");
                Console.WriteLine("What type of account would you like to add?");
                Console.WriteLine("(CHECKINGS) or (SAVINGS)?");
                var newAccount = Console.ReadLine().ToUpper();
                // Validate that newAccount answer
                while (newAccount != "CHECKINGS" && newAccount != "SAVINGS")
                {
                  Console.WriteLine(errorMessage);
                  newAccount = Console.ReadLine().ToUpper();
                }
                Console.WriteLine("How much would you like to deposit into this new account?");
                // Initialize variable result 
                double amount;
                // take in user input, parse to double, and output the result to the variable above as a double
                double.TryParse(Console.ReadLine(), out amount);
                // Validate that amount user is depositing is greater than 0
                while (amount <= 0)
                {
                  Console.WriteLine(errorMessage);
                  double.TryParse(Console.ReadLine(), out amountToDeposit);
                }
                // CHECK USER ANSWER
                if (newAccount == "CHECKINGS")
                {
                  // Create the account and deposit the amount
                  user.AddAccount(newAccount, amount);
                }
                else if (newAccount == "SAVINGS")
                {
                  // Create the account and deposit the amount
                  user.AddAccount(newAccount, amount);
                }
                // Save data to file 
                SaveData(bank);
                break;

              // * * * * * C L O S E  ACCOUNT * * * * *
              case "x":
                // Add space
                System.Console.WriteLine();
                Console.WriteLine($"{user.UserName}, you have chosen to close an account.");
                Console.WriteLine("Which account would you like to close? Please select the account number.");
                user.ShowAccounts();
                int closeAccount;
                int.TryParse(Console.ReadLine(), out closeAccount);
                // VALIDATE INPUT
                while (!user.Accounts.Any(acc => acc.AccountNumber == closeAccount))
                {
                  Console.WriteLine(errorMessage);
                  int.TryParse(Console.ReadLine(), out closeAccount);
                }

                // Make sure that account balance is = 0 before closing
                while (user.Accounts.First(acc => acc.AccountNumber == closeAccount).Balance > 0)
                {
                  System.Console.WriteLine($"{user.Accounts.First(acc => acc.AccountNumber == closeAccount).Name} must have a balance of 0 before you close the account.");
                  System.Console.WriteLine("Would you like to withdraw the remaining amount? (YES) or (NO)");
                  System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                  var userInput = Console.ReadLine().ToLower();
                  // check user input
                  if (userInput == "yes")
                  {
                    user.Withdraw(closeAccount, user.Accounts.First(acc => acc.AccountNumber == closeAccount).Balance);
                  }
                }
                // Call method to remove entered account
                user.RemoveAccount(closeAccount);
                // Add space
                System.Console.WriteLine();
                Console.WriteLine($"{user.UserName}, you have successfully closed the account.");
                // Save data to file 
                SaveData(bank);
                break;

              case "c":
                System.Console.WriteLine("Please enter your old password:");
                var oldPassword = Console.ReadLine().ToLower();
                System.Console.WriteLine("Please enter a new password:");
                var newPassword = Console.ReadLine().ToLower();
                System.Console.WriteLine("Please confirm the new password:");
                var confirmNewPassword = Console.ReadLine().ToLower();

                // Check that new passwords match
                while (newPassword != confirmNewPassword)
                {
                  System.Console.WriteLine("Those passwords do not match. Please try again");
                  System.Console.WriteLine("Please enter a new password:");
                  newPassword = Console.ReadLine().ToLower();
                  System.Console.WriteLine("Please confirm the new password:");
                  confirmNewPassword = Console.ReadLine().ToLower();
                }
                // Set the new password
                user.Password = confirmNewPassword;
                break;
            }
            break;
          // * * * * * Q U I T * * * * *
          case "q":
            isRunning = false;
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine($"Enjoy the rest of your day, {user.UserName}. Thanks for banking with the First Bank of Suncoast!");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            // Save data to file 
            SaveData(bank);
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