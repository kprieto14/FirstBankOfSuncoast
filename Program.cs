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
        //If the greeting returns a valid customer, the real banking application begins
        if(currentClient != null)
        {
          response = PromptForString("\nWhich account would you like to work with? \n\n(S)avings \n(C)heckings\n(V)iew Transaction History\n(Q)uit\n");
        
        switch(response)
        {
          //Chosen by the user to work with their savings account
          case "S":
            response = PromptForString("\nYou have chosen your Savings account, what would you like to do with this account? \n(W)ithdraw \n(D)eposit \n(V)iew Balance\n");

            switch(response)
            {
              //Withdraw from savings
              case "W":
                Console.WriteLine("This will eventually allow you to withdraw money.");
                //database.SaveTransactions();
                break;
              
              //Deposit from savings
              case "D":
                Console.WriteLine("This will eventually allow you to deposit money.");
                //database.SaveTransactions();
                break;
              
              //View savings balance or transaction history
              case "V":
                //Shows savings balance
                Console.WriteLine($"You currently have ${currentClient.SavingsBalance} in your account. ");
                break;
              
              //Prints if the user inputs something other than a valid response
              default:
                Console.WriteLine("I am not sure what you are saying. Please try again.");
                break;
            }
            break;

          //Chosen by the user to work with their deposit account          
          case "C":
            response = PromptForString("You have chosen your Checkings account, what would you like to do with this account? \n(W)ithdraw \n(D)eposit \n(V)iew\n");

            switch(response)
            {
              //Withdraw
              case "W":
                Console.WriteLine("This will eventually allow you to withdraw money.");
                //database.SaveTransactions();
                break;
              
              //Deposit
              case "D":
                Console.WriteLine("This will eventually allow you to deposit money.");
                //database.SaveTransactions();
                break;
              
              //View balance or transaction history
              case "V":
                response = PromptForString("\nWould you like to view your (B)alance or (T)ransaction History? ");

                switch(response)
                {
                  //View balance
                  case "B":
                    //Views checking balance
                    Console.WriteLine($"You currently have ${currentClient.CheckingsBalance} in your account. ");
                    break;

                  //View transaction history
                  case "T":
                    Console.WriteLine("This will eventually allow you to view your transaction history.");
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
            //View transaction history
            var clientName = currentClient.UserName;
            var transactions = database.ViewTransactions(clientName);

            Console.WriteLine("\nHere is your transaction history: ");
            //Goes through list and prints out all the information in the transaction history with the same user name and puts it in a hopefully helpful table.
            for(var count = 0; count < transactions.Count(); count++)
            {
              var transaction = transactions[count];
              Console.WriteLine($"\n{count} | {transaction.UserName} | {transaction.Date} | {transaction.AccountType}: {transaction.TransactionType} | Amount requested: ${transaction.AmountRequested} | Balance: ${transaction.Balance} |");
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

        //Otherwise, the application stops
        else
        {
          keepGoing = false;
        }
      }
    }
  }
}
