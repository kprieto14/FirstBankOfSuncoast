using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
//using CsvHelper;
//using CsvHelper.Configuration;

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
    static void Greeting()
    {
      var database = new Database();
      //Meant to print a greeting that welcomes the user, then check to see if they are either an existing User or create a New one based on the input
      var response = PromptForString("Welcome to the National Bank of Kristy! We are delighted to have you as a customer. Are you an (E)xisting user \nor a (N)ew one? ");
      Console.WriteLine();

      switch(response)
      {
        case "E":
          Console.WriteLine("This will eventually verify your account.");
          //Once verified, return newly made client object
          break;
        
        //Code that will start to create a new customer 
        case "N":
          var client = new Client();
          var savingsTransaction = new Transaction();
          var checkingsTransaction = new Transaction();
          var rightNow = DateTime.Now;

          //Will ask for name and house that name in a Client object, saving variable to be added to transaction list later
          var name = PromptForString("Lets set up your account now! \nWhat is your name? ");
          client.UserName = name;

          client.Pin = PromptForInteger("What will your PIN number be? ");

          //To start first deposit into savings account
          var savings = PromptForInteger("How much will you deposit into your savings account? (Only whole dollars are accepted at this bank. Sorry, no change.) ");
          if (savings > 0)
          {
            Console.WriteLine($"\nYou have chosen to add ${savings} as your starter funds to your savings account.\n");
            //Only adds to savings if the user deposits more than $0
            client.SavingsBalance = savings;

            //Adds a transaction type to add to list
            savingsTransaction.UserName = name;
            savingsTransaction.Date = rightNow.ToString();
            savingsTransaction.AccountType = "Savings";
            savingsTransaction.AmountRequested = savings;
            savingsTransaction.Balance = savings;
            database.AddTransaction(savingsTransaction);
          }
          else 
          {
            //Will not add a transaction history. Does not make sense to make a transaction add when nothing is being added.
            Console.WriteLine($"\nYou have chosen to add nothing to your savings account.\n");
            client.SavingsBalance = savings;
          }

          //To start first deposit into checkings account
          var checkings = PromptForInteger("How much will you deposit into your checkings account? (Only whole dollars are accepted at this bank. Sorry, no change.) ");
          if (checkings > 0)
          {
            Console.WriteLine($"\nYou have chosen to add ${checkings} as your starter funds to your checkings account.\n");
            //Only adds to checkings if the user deposits more than $0
            client.CheckingsBalance = checkings;

            //Adds a transaction type to add to list
            checkingsTransaction.UserName = name;
            checkingsTransaction.Date = rightNow.ToString();
            checkingsTransaction.AccountType = "Checkings";
            checkingsTransaction.AmountRequested = checkings;
            checkingsTransaction.Balance = checkings;
            database.AddTransaction(checkingsTransaction);
          }
          else 
          {
            //Will not add a transaction history. Does not make sense to make a transaction add when nothing is being added.
            Console.WriteLine($"\nYou have chosen to add nothing to your checkings account.\n");
            client.CheckingsBalance = checkings;
          }

          Console.WriteLine($"Congratulations and welcome to the National Bank of Kristy, {name}! You are officially starting with ${client.SavingsBalance} in your savings account and ${client.CheckingsBalance} in your checkings account. We do appreciate your business.");
          //Return newly made client object
          break;

        default:
          Console.WriteLine("\nI am not sure what you are saying, please try again.");
          break;
      }
    }
    static void Main(string[] args)
    {
      var database = new Database();
      
      Greeting();

      //var keepGoing = true;

      /*Start of menu for user to work with their own account once it has been verified or loaded in
      while(keepGoing == true)
      {
        var response = PromptForString("Which account would you like to work with? \n\n(S)avings \n(C)heckings\n(Q)uit\n");
        
        switch(response)
        {
          //Chosen by the user to work with their savings account
          case "S":
            response = PromptForString("\nYou have chosen your Savings account, what would you like to do with this account? \n(W)ithdraw \n(D)eposit \n(V)iew\n");

            switch(response)
            {
              //Withdraw from savings
              case "W":
                Console.WriteLine("This will eventually allow you to withdraw money.");
                break;
              
              //Deposit from savings
              case "D":
                Console.WriteLine("This will eventually allow you to deposit money.");
                break;
              
              //View savings balance or transaction history
              case "V":
                response = PromptForString("Would you like to view your (B)alance or (T)ransaction History?");

                switch(response)
                {
                  //View balance
                  case "B":
                    Console.WriteLine("This will eventually allow you to view your balance.");
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

          //Chosen by the user to work with their deposit account          
          case "C":
            response = PromptForString("You have chosen your Checkings account, what would you like to do with this account? \n(W)ithdraw \n(D)eposit \n(V)iew\n");

            switch(response)
            {
              //Withdraw
              case "W":
                Console.WriteLine("This will eventually allow you to withdraw money.");
                break;
              
              //Deposit
              case "D":
                Console.WriteLine("This will eventually allow you to deposit money.");
                break;
              
              //View balance or transaction history
              case "V":
                response = PromptForString("Would you like to view your (B)alance or (T)ransaction History?");

                switch(response)
                {
                  //View balance
                  case "B":
                    Console.WriteLine("This will eventually allow you to view your balance.");
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
          
          //Chosen if the user wants to quit
          case "Q":
            Console.WriteLine("\nPlease come back again!");
            keepGoing = false;
            break;
          
          //Prints if the user inputs something other than a valid response
          default:
            Console.WriteLine("I am not sure what you are saying. Please try again.");
            break;
        }
      }*/
    }
  }
}
