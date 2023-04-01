using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace FirstBankOfSuncoast
{
  class Program
  {
    static string PromptForString(string prompt)
    {
      //This method will always call for a prompt to print, then read what the user inputs into the console and returns the response
      Console.Write(prompt);
      var userInput = Console.ReadLine().ToUpper();

      return userInput;
    }
    static int PromptForInteger(string prompt)
    {
      //This method will always call for a prompt to print, then read what the user inputs into the console, check if it is an integer, and either return a correct response or a 0
      Console.Write(prompt);
      int userInput;
      var isThisGoodInput = Int32.TryParse(Console.ReadLine(), out userInput);
      
      if (isThisGoodInput)
      {
        return userInput;
      }
      else
      {
        Console.WriteLine("Sorry, that isn't a valid input, I'm using 0 as your answer.");
        return 0;
      }
    }
    static void Main(string[] args)
    {
      var database = new Database();
      database.LoadClients();
      database.LoadTransactions();

      Client currentClient;
      
       //Meant to print a greeting that welcomes the user, then check to see if they are either an existing User or create a New one based on the input
      var response = PromptForString("Welcome to the National Bank of Kristy! We are delighted to have you as a customer. Are you an (E)xisting user \nor a (N)ew one? ");
      Console.WriteLine();

      //Code that will verify if the user exists and inputs a pin for security
      if(response == "E")
      {
        var name = PromptForString("What is your name? ");
          
        // Make a new variable to store the found employee, or null if not found
        Client foundClient = database.FindClient(name);

        // If the foundClient is still null, nothing was found
        if (foundClient == null) 
        {
          Console.WriteLine("\nNo match found. Please start over to try again.");
          currentClient = null;
        } 
        else 
        {
          // Otherwise print details of the found employee
          //Code here to verify pin and return customer appropriately
          var correctPin = PromptForInteger($"\nHello {foundClient.UserName}, please enter your PIN number ");
          if(correctPin == foundClient.Pin)
          {
            // Return user if the PIN is correct
            Console.WriteLine($"\nWelcome {foundClient.UserName}! It is good to see you again.");
            currentClient = foundClient;
          }
          else
          {
            //Otherwise does not allow user access to bank because you should know your pin
            Console.WriteLine("\nIncorrect pin. Please try again.");
            currentClient = null;
          }
        }
      }
      //Code that will start to create a new customer 
      else if (response == "N")
      {
        var newClient = new Client();
        var savingsTransaction = new Transaction();
        var checkingsTransaction = new Transaction();
        var rightNow = DateTime.Now;

        //Will ask for name and house that name in a Client object, saving variable to be added to transaction list later
        var name = PromptForString("Lets set up your account now! \nWhat is your name? ");
        newClient.UserName = name;

        newClient.Pin = PromptForInteger("What will your PIN number be? ");

        //To start first deposit into savings account
        var savings = PromptForInteger("How much will you deposit into your savings account? (Only whole dollars are accepted at this bank. Sorry, no change.) ");
        if (savings > 0)
        {
          Console.WriteLine($"\nYou have chosen to add ${savings} as your starter funds to your savings account.\n");
          //Only adds to savings if the user deposits more than $0
          newClient.SavingsBalance = savings;

          //Adds a transaction type to add to list
          savingsTransaction.UserName = name;
          savingsTransaction.Date = rightNow.ToString();
          savingsTransaction.AccountType = "Savings";
          savingsTransaction.TransactionType = "Deposit";
          savingsTransaction.AmountRequested = savings;
          savingsTransaction.Balance = savings;
          database.AddTransaction(savingsTransaction);
          database.SaveTransactions();
        }
        else 
        {
          //Will not add a transaction history. Does not make sense to make a transaction add when nothing is being added.
          Console.WriteLine($"\nYou have chosen to add nothing to your savings account.\n");
          newClient.SavingsBalance = savings;
        }

        //To start first deposit into checkings account
        var checkings = PromptForInteger("How much will you deposit into your checkings account? (Only whole dollars are accepted at this bank. Sorry, no change.) ");
        if (checkings > 0)
        {
          Console.WriteLine($"\nYou have chosen to add ${checkings} as your starter funds to your checkings account.\n");
          //Only adds to checkings if the user deposits more than $0
          newClient.CheckingsBalance = checkings;

          //Adds a transaction type to add to list
          checkingsTransaction.UserName = name;
          checkingsTransaction.Date = rightNow.ToString();
          checkingsTransaction.AccountType = "Checkings";
          checkingsTransaction.TransactionType = "Deposit";
          checkingsTransaction.AmountRequested = checkings;
          checkingsTransaction.Balance = checkings;
          database.AddTransaction(checkingsTransaction);
          database.SaveTransactions();
        }
        else 
        {
          //Will not add a transaction history. Does not make sense to make a transaction add when nothing is being added.
          Console.WriteLine($"\nYou have chosen to add nothing to your checkings account.\n");
          newClient.CheckingsBalance = checkings;
        }

        Console.WriteLine($"Congratulations and welcome to the National Bank of Kristy, {name}! You are officially starting with ${newClient.SavingsBalance} in your savings account and ${newClient.CheckingsBalance} in your checkings account. We do appreciate your business.");
        //Adds new user to client list in Database
        database.AddClient(newClient);
        database.SaveClients();
        currentClient = newClient;
      }
      else
      {
        Console.WriteLine("\nI am not sure what you are saying, please try again.");
        currentClient = null;
      }

      var keepGoing = true;

      //Start of menu for user to work with their own account once it has been verified or loaded in
      while(keepGoing == true)
      {
        //Establishes new Transaction object to be used for either deposits or withdrawals in Checkings/ Savings
        var newTransaction = new Transaction();
        var rightNow = DateTime.Now;

        //If the greeting returns a valid customer, the real banking application begins
        if(currentClient != null)
        {
          response = PromptForString("\nWhich account would you like to work with? \n\n(S)avings \n(C)heckings\n(V)iew Transaction History\n(Q)uit\n");
        
        switch(response)
        {
          //Chosen by the user to work with their savings account
          case "S":
            response = PromptForString("\nYou have chosen your Savings account, what would you like to do with this account? \n(W)ithdraw \n(D)eposit \n(V)iew Account\n");

            switch(response)
            {
              //Withdraw from savings
              case "W":
                var intResponse = PromptForInteger("\nHow much would you like to withdraw? (Remember this bank only accepts whole dollar amounts) ");
                
                //Checks if client has requested more than $0 funds to withdraw, if they have the funds it will disperse
                if(intResponse > 0 && intResponse <= currentClient.SavingsBalance)
                {
                  //Double checks that the user wants to withdraw that amount
                  response = PromptForString($"You have requested to withdraw ${intResponse} are you sure you want to withdraw that amount? (Y/N) ");
                  
                  if(response == "Y")
                  {
                    database.Withdraw(intResponse, currentClient, "S");
                    
                    //Make a new transaction profile
                    newTransaction.UserName = currentClient.UserName;
                    newTransaction.Date = rightNow.ToString();
                    newTransaction.AccountType = "Savings";
                    newTransaction.TransactionType = "Withdrawal";
                    newTransaction.AmountRequested = intResponse;
                    newTransaction.Balance = currentClient.SavingsBalance;
                    database.AddTransaction(newTransaction);
                    database.SaveTransactions();
                    Console.WriteLine($"You have withdrew ${intResponse} from your savings account for a total balance of ${currentClient.SavingsBalance}.");
                  }
                  else if (response == "N")
                  {
                    Console.WriteLine("Okay, we will leave your Savings account alone.");
                  }
                  else
                  {
                    Console.WriteLine("I am not sure what you meant, we will leave your Savings Account alone.");
                  }
                }
                //If they requested more than $0 AND more than what is in their account, it will not disperse the funds
                else if(intResponse > 0 && intResponse > currentClient.SavingsBalance)
                {
                  Console.WriteLine("You do not have enough cash. Sorry 😔");
                }
                //If they requested $0, then nothing happens as well
                else
                {
                  Console.WriteLine("You asked to take nothing out.");
                }
                break;
              
              //Deposit from savings
              case "D":
                intResponse = PromptForInteger("\nHow much would you like to deposit? (Remember this bank only accepts whole dollar amounts) ");
                //Will only deposit if user actually deposits money
                if (intResponse > 0)
                {
                  database.Deposit(intResponse, currentClient, "S");
                  
                  //Make a new transaction profile
                  newTransaction.UserName = currentClient.UserName;
                  newTransaction.Date = rightNow.ToString();
                  newTransaction.AccountType = "Savings";
                  newTransaction.TransactionType = "Deposit";
                  newTransaction.AmountRequested = intResponse;
                  newTransaction.Balance = currentClient.SavingsBalance;
                  database.AddTransaction(newTransaction);
                  database.SaveTransactions();
                  Console.WriteLine($"You have deposited ${intResponse} into your savings account for a total balance of ${currentClient.SavingsBalance}.");

                  //Tests to see if code should keep going based on user's response
                  response = PromptForString("\nWould you like to do anything else? (Y/N) ");
                  if(response == "N")
                  {
                    Console.WriteLine("Okay, please come back again soon!");
                    keepGoing = false;
                  }
                  else if(response == "Y")
                  {
                    Console.WriteLine("Okay!");
                  }
                  else
                  {
                    Console.WriteLine("I did not quit get that. I will assume you mean keep going.");
                  }
                }
                else
                {
                  Console.WriteLine("It looks like you did not add anything!");
                }
                break;
              
              //View savings balance or transaction history
              case "V":
                response = PromptForString("\nWould you like to view your (B)alance or (T)ransaction History for your Savings account? ");

                switch(response)
                {
                  //View balance for savings
                  case "B":
                    //Views savings balance
                    Console.WriteLine($"You currently have ${currentClient.SavingsBalance} in your account. ");
                    break;

                  //View transaction history
                  case "T":
                    //View total user transaction history for their savings account
                    var savingsTransactions = database.ViewTransactions(currentClient.UserName, "S");

                    Console.WriteLine("\nHere is your Savings transaction history: ");
                    //Goes through list and prints out all the information in the transaction history with the same user name and puts it in a hopefully helpful table.
                    for(var count = 0; count < savingsTransactions.Count(); count++)
                    {
                      var transaction = savingsTransactions[count];
                      Console.WriteLine($"\n{count + 1} | {transaction.UserName} | {transaction.Date} | {transaction.AccountType}: {transaction.TransactionType} | Amount requested: ${transaction.AmountRequested} | Balance: ${transaction.Balance} |");
                    }
                    break;
                }
                break;
              
              //Prints if the user inputs something other than a valid response
              default:
                Console.WriteLine("I am not sure what you are saying. Please try again.");
                break;
            }
            break;

          //Chosen by the user to work with their deposit account          
          case "C":
            response = PromptForString("\nYou have chosen your Checkings account, what would you like to do with this account? \n(W)ithdraw \n(D)eposit \n(V)iew Account\n");

            switch(response)
            {
              //Withdraw checkings
              case "W":
                var intResponse = PromptForInteger("\nHow much would you like to withdraw? (Remember this bank only accepts whole dollar amounts) ");
                
                //Checks if client has requested more than $0 funds to withdraw, if they have the funds it will disperse
                if(intResponse > 0 && intResponse <= currentClient.CheckingsBalance)
                {
                  //Double checks that the user wants to withdraw that amount
                  response = PromptForString($"You have requested to withdraw ${intResponse} are you sure you want to withdraw that amount? (Y/N) ");
                  
                  if(response == "Y")
                  {
                    database.Withdraw(intResponse, currentClient, "C");
                    
                    //Make a new transaction profile
                    newTransaction.UserName = currentClient.UserName;
                    newTransaction.Date = rightNow.ToString();
                    newTransaction.AccountType = "Checkings";
                    newTransaction.TransactionType = "Withdrawal";
                    newTransaction.AmountRequested = intResponse;
                    newTransaction.Balance = currentClient.CheckingsBalance;
                    database.AddTransaction(newTransaction);
                    database.SaveTransactions();
                    Console.WriteLine($"You have withdrew ${intResponse} from your checkings account for a total balance of ${currentClient.CheckingsBalance}.");
                  }
                  else if (response == "N")
                  {
                    Console.WriteLine("Okay, we will leave your Checkings account alone.");
                  }
                  else
                  {
                    Console.WriteLine("I am not sure what you meant, we will leave your Checkings Account alone.");
                  }
                }
                //If they requested more than $0 AND more than what is in their account, it will not disperse the funds
                else if(intResponse > 0 && intResponse > currentClient.CheckingsBalance)
                {
                  Console.WriteLine("You do not have enough cash. Sorry 😔");
                }
                //If they requested $0, then nothing happens as well
                else
                {
                  Console.WriteLine("You asked to take nothing out.");
                }
                break;
              
              //Deposit checkings
              case "D":
                intResponse = PromptForInteger("\nHow much would you like to deposit? (Remember this bank only accepts whole dollar amounts) ");
                //Will only deposit if user actually deposits money
                if (intResponse > 0)
                {
                  database.Deposit(intResponse, currentClient, "C");
                  
                  //Make a new transaction profile
                  newTransaction.UserName = currentClient.UserName;
                  newTransaction.Date = rightNow.ToString();
                  newTransaction.AccountType = "Checkings";
                  newTransaction.TransactionType = "Deposit";
                  newTransaction.AmountRequested = intResponse;
                  newTransaction.Balance = currentClient.CheckingsBalance;
                  database.AddTransaction(newTransaction);
                  database.SaveTransactions();
                  Console.WriteLine($"You have deposited ${intResponse} into your checkings account for a total balance of ${currentClient.CheckingsBalance}.");

                  //Tests to see if code should keep going based on user's response
                  response = PromptForString("\nWould you like to do anything else? (Y/N) ");
                  if(response == "N")
                  {
                    Console.WriteLine("Okay, please come back again soon!");
                    keepGoing = false;
                  }
                  else if(response == "Y")
                  {
                    Console.WriteLine("Okay!");
                  }
                  else
                  {
                    Console.WriteLine("I did not quit get that. I will assume you mean keep going.");
                  }
                }
                else
                {
                  Console.WriteLine("It looks like you did not add anything!");
                }
                break;
              
              //View balance or transaction history
              case "V":
                response = PromptForString("\nWould you like to view your (B)alance or (T)ransaction History for your Checkings account? ");

                switch(response)
                {
                  //View balance
                  case "B":
                    //Views checking balance
                    Console.WriteLine($"You currently have ${currentClient.CheckingsBalance} in your account. ");
                    break;

                  //View transaction history
                  case "T":
                    //View total user transaction history
                    var checkingTransactions = database.ViewTransactions(currentClient.UserName, "C");

                    Console.WriteLine("\nHere is your Checking transaction history: ");
                    //Goes through list and prints out all the information in the transaction history with the same user name and puts it in a hopefully helpful table.
                    for(var count = 0; count < checkingTransactions.Count(); count++)
                    {
                      var transaction = checkingTransactions[count];
                      Console.WriteLine($"\n{count + 1} | {transaction.UserName} | {transaction.Date} | {transaction.AccountType}: {transaction.TransactionType} | Amount requested: ${transaction.AmountRequested} | Balance: ${transaction.Balance} |");
                    }
                    break;
                  
                  //Prints if the user inputs something other than a valid response
                  default:
                    Console.WriteLine("I am not sure what you are saying. Please try again.");
                    break;
                }
                break;

              //Prints if the user inputs something other than a valid response
              default:
                Console.WriteLine("I am not sure what you are saying. Please try again.");
                break;
            }
            break;
          
          case "V":
            //View total user transaction history
            var clientName = currentClient.UserName;
            var transactions = database.ViewTransactions(clientName, null);

            Console.WriteLine("\nHere is your transaction history: ");
            //Goes through list and prints out all the information in the transaction history with the same user name and puts it in a hopefully helpful table.
            for(var count = 0; count < transactions.Count(); count++)
            {
              var transaction = transactions[count];
              Console.WriteLine($"\n{count + 1} | {transaction.UserName} | {transaction.Date} | {transaction.AccountType}: {transaction.TransactionType} | Amount requested: ${transaction.AmountRequested} | Balance: ${transaction.Balance} |");
            }
            break;

          //Chosen if the user wants to quit
          case "Q":
            Console.WriteLine("\nPlease come back again!");
            keepGoing = false;
            break;
          
          //Prints if the user inputs something other than a valid response
          default:
            Console.WriteLine("\nI am not sure what you are saying. Please try again.");
            break;
          }
        }
        //Otherwise, the application stops if no valid customer is in the system
        else
        {
          keepGoing = false;
        }
      }
    }
  }
}
