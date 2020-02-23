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
    // SAVE DATA TO NEW FILE
    static void SaveData(Bank bank)
    {
      // var writer = new StreamWriter("bankData.csv");
      // var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
      // csvWriter.WriteRecords(Users);
      // writer.Flush();
      string json = JsonConvert.SerializeObject(bank);
      System.IO.File.WriteAllText(@"bankData.txt", json);
    }

    static void Main(string[] args)
    {
      var isRunning = true;
      var loggedIn = false;
      var bank = new Bank();
      bank = JsonConvert.DeserializeObject<Bank>(File.ReadAllText(@"bankData.txt"));
      var currentUser = new User();
      var errorMessage = $"Apologies, that is not a valid input. Please try again.";

      while (isRunning)
      {
        // if not logged in ask to log in or sign up
        while (!loggedIn)
        {
          // ask user to login or sign up
          System.Console.WriteLine("Greetings, would you like to (LOGIN) or (SIGN UP)?");
          // Get user input
          var userAnswer = Console.ReadLine().ToLower();
          while (userAnswer != "login" && userAnswer != "sign up")
          {
            System.Console.WriteLine(errorMessage);
            userAnswer = Console.ReadLine().ToLower();
          }

          if (userAnswer == "sign up")
          {
            System.Console.WriteLine("Thanks for choosing First Bank of Suncoast.");
            System.Console.WriteLine("Please enter a username:");
            var username = Console.ReadLine();
            // validate for username 
            while (bank.Users.Any(user => user.UserName == username))
            {
              System.Console.WriteLine("Username taken, please try again.");
              username = Console.ReadLine();
            }

            System.Console.WriteLine("Please enter a password:");
            var password = Console.ReadLine();
            // validate for password
            bank.CreateUser(username, password);
            SaveData(bank);
          }
          else if (userAnswer == "login")
          {
            // prompt for username
            System.Console.WriteLine("Please enter your username:");
            var username = Console.ReadLine();
            // validate for username 
            while (!bank.Users.Any(user => user.UserName == username))
            {
              System.Console.WriteLine("Username not found, please try again.");
              username = Console.ReadLine();
            }
            // prompt for password
            System.Console.WriteLine("Please enter your password:");
            var password = Console.ReadLine();
            // validate for password
            while (password != bank.Users.First(user => user.UserName == username).Password)
            {
              System.Console.WriteLine("Incorrect password. Please try again.");
              password = Console.ReadLine();
            }
            // change log in flagged to true
            loggedIn = true;
          }

          var input = "";
          if (!currentUser.Accounts.Any())
          {
            input = "u";
          }
          else
          {
            // Ask user what they would like to do
            Console.WriteLine("What would you like to do today?");
            Console.WriteLine("(VIEW 'v') accounts, (DEPOSIT 'd'), (WITHDRAW 'w'), (TRANSFER 't'), (USER 'u') settings, or (QUIT 'q')");
            input = Console.ReadLine().ToLower();
          }
          // Add space
          Console.WriteLine();

          // Create switch statement for different conditions depending on input
          switch (input)
          {
            // * * * * * VIEW ACCOUNTS * * * * *
            case "v":
              currentUser.ShowAccounts();
              break;
            // * * * * * D E P O S I T * * * * *
            case "d":
              Console.WriteLine("Please select an account to deposit to:");
              // Refresh the user which accounts there are currently
              currentUser.ShowAccounts();
              // Initialize variable result 
              int depositAccount;
              // take in user input, parse to double, and output the result to the variable above as a double
              int.TryParse(Console.ReadLine(), out depositAccount);
              // VALIDATE to make sure account number exists
              while (!currentUser.Accounts.Any(acc => acc.AccountNumber == depositAccount))
              {
                Console.WriteLine(errorMessage);
                int.TryParse(Console.ReadLine(), out depositAccount);
              }
              // Add space
              Console.WriteLine();

              // Ask user how much to deposit
              Console.WriteLine($"How much would you like to deposit in {currentUser.Accounts.First(acc => acc.AccountNumber == depositAccount).Name}?");
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
              currentUser.Deposit(depositAccount, amountToDeposit);
              Console.WriteLine($"You have successfully deposited ${amountToDeposit} into your {currentUser.Accounts.First(acc => acc.AccountNumber == depositAccount).Name} account.");
              // Save data to file 
              SaveData(bank);
              break;

            // * * * * * W I T H D R A W * * * * *
            case "w":
              Console.WriteLine("Please select an account to withdraw from:");
              currentUser.ShowAccounts();
              // Initialize variable
              int withdrawAccount;
              int.TryParse(Console.ReadLine(), out withdrawAccount);

              while (!currentUser.Accounts.Any(acc => acc.AccountNumber == withdrawAccount))
              {
                Console.WriteLine(errorMessage);
                int.TryParse(Console.ReadLine(), out depositAccount);
              }

              Console.WriteLine();
              // Ask user how much to deposit
              Console.WriteLine($"How much would you like to withdraw from {currentUser.Accounts.First(acc => acc.AccountNumber == withdrawAccount).Name}?");
              // Initialize variable result 
              double amountToWithdraw;
              // take in user input, parse to double, and output the result variable above as a double
              double.TryParse(Console.ReadLine(), out amountToWithdraw);

              while (amountToWithdraw <= 0 || amountToWithdraw > currentUser.Accounts.First(acc => acc.AccountNumber == withdrawAccount).Balance)
              {
                Console.WriteLine(errorMessage);
                double.TryParse(Console.ReadLine(), out amountToWithdraw);
              }

              // Call method to withdraw amount from account
              currentUser.Withdraw(withdrawAccount, amountToWithdraw);
              Console.WriteLine($"You have successfully withdrew ${amountToWithdraw} from your {currentUser.Accounts.First(acc => acc.AccountNumber == withdrawAccount).Name} account.");
              // Save data to file 
              SaveData(bank);
              break;

            // * * * * * T R A N S F E R * * * * *
            case "t":
              // ***** ASK USER WHICH ACCOUNT TO TRANSFER FROM
              Console.WriteLine("Please select an account to transfer from:");
              currentUser.ShowAccounts();
              int transferAccount;
              int.TryParse(Console.ReadLine(), out transferAccount);
              // VALIDATE INPUT
              while (!currentUser.Accounts.Any(acc => acc.AccountNumber == transferAccount))
              {
                Console.WriteLine(errorMessage);
                int.TryParse(Console.ReadLine(), out transferAccount);
              }
              Console.WriteLine();

              // **** WHICH ACCOUNT TO TRANSFER TO *****
              Console.WriteLine("Please select an account to transfer to:");
              currentUser.ShowAccounts();
              int incomingAccount;
              int.TryParse(Console.ReadLine(), out incomingAccount);
              // VALIDATE INPUT
              while (!currentUser.Accounts.Any(acc => acc.AccountNumber == incomingAccount) || transferAccount == incomingAccount)
              {
                Console.WriteLine(errorMessage);
                int.TryParse(Console.ReadLine(), out incomingAccount);
              }
              Console.WriteLine();

              // Ask user how much to deposit
              Console.WriteLine($"How much would you like to transfer from {currentUser.Accounts.First(acc => acc.AccountNumber == transferAccount).Name} to {currentUser.Accounts.First(acc => acc.AccountNumber == incomingAccount).Name}?");
              // Initialize variable result 
              double amountToTransfer;
              // take in user input, parse to double, and output the result variable above as a double
              double.TryParse(Console.ReadLine(), out amountToTransfer);

              while (amountToTransfer <= 0 || amountToTransfer > currentUser.Accounts.First(acc => acc.AccountNumber == transferAccount).Balance)
              {
                Console.WriteLine(errorMessage);
                double.TryParse(Console.ReadLine(), out amountToTransfer);
              }
              // Call method to transfer
              currentUser.Transfer(transferAccount, incomingAccount, amountToTransfer);
              Console.WriteLine($"You have successfully transferred ${amountToTransfer} from your {currentUser.Accounts.First(acc => acc.AccountNumber == transferAccount).Name} account into your {currentUser.Accounts.First(acc => acc.AccountNumber == incomingAccount).Name}.");
              // Save data to file 
              SaveData(bank);
              break;

            // * * * * * USER S E T T I N G S * * * * *
            case "u":
              var answer = "";
              if (!currentUser.Accounts.Any())
              {
                answer = "a";
              }
              else
              {
                Console.WriteLine("What would you like to do in settings?");
                Console.WriteLine("(ADD 'a') account, (CLOSE 'x') account, (CHANGE 'c') password, or (SIGN OUT 's')");
                answer = Console.ReadLine().ToLower();
              }

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
                    currentUser.AddAccount(newAccount, amount);
                  }
                  else if (newAccount == "SAVINGS")
                  {
                    // Create the account and deposit the amount
                    currentUser.AddAccount(newAccount, amount);
                  }
                  // Save data to file 
                  SaveData(bank);
                  break;

                // * * * * * C L O S E  ACCOUNT * * * * *
                case "x":
                  // Add space
                  System.Console.WriteLine();
                  Console.WriteLine($"{currentUser.UserName}, you have chosen to close an account.");
                  Console.WriteLine("Which account would you like to close? Please select the account number.");
                  currentUser.ShowAccounts();
                  int closeAccount;
                  int.TryParse(Console.ReadLine(), out closeAccount);
                  // VALIDATE INPUT
                  while (!currentUser.Accounts.Any(acc => acc.AccountNumber == closeAccount))
                  {
                    Console.WriteLine(errorMessage);
                    int.TryParse(Console.ReadLine(), out closeAccount);
                  }

                  // Make sure that account balance is = 0 before closing
                  while (currentUser.Accounts.First(acc => acc.AccountNumber == closeAccount).Balance > 0)
                  {
                    System.Console.WriteLine($"{currentUser.Accounts.First(acc => acc.AccountNumber == closeAccount).Name} must have a balance of 0 before you close the account.");
                    System.Console.WriteLine("Would you like to withdraw the remaining amount? (YES) or (NO)");
                    System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    var userInput = Console.ReadLine().ToLower();
                    // check user input
                    if (userInput == "yes")
                    {
                      currentUser.Withdraw(closeAccount, currentUser.Accounts.First(acc => acc.AccountNumber == closeAccount).Balance);
                    }
                  }
                  // Call method to remove entered account
                  currentUser.RemoveAccount(closeAccount);
                  // Add space
                  System.Console.WriteLine();
                  Console.WriteLine($"{currentUser.UserName}, you have successfully closed the account.");
                  // Save data to file 
                  SaveData(bank);
                  break;

                case "c":
                  System.Console.WriteLine("Please enter your old password:");
                  var oldPassword = Console.ReadLine();
                  System.Console.WriteLine("Please enter a new password:");
                  var newPassword = Console.ReadLine();
                  System.Console.WriteLine("Please confirm the new password:");
                  var confirmNewPassword = Console.ReadLine();

                  // Check that new passwords match
                  while (newPassword != confirmNewPassword)
                  {
                    System.Console.WriteLine("Those passwords do not match. Please try again");
                    System.Console.WriteLine("Please enter a new password:");
                    newPassword = Console.ReadLine();
                    System.Console.WriteLine("Please confirm the new password:");
                    confirmNewPassword = Console.ReadLine();
                  }
                  // Set the new password
                  currentUser.Password = confirmNewPassword;
                  break;
              }
              break;
            // * * * * * Q U I T * * * * *
            case "q":
              isRunning = false;
              Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
              Console.WriteLine("Thanks for banking with the First Bank of Suncoast!");
              Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
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
}