using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FirstBankOfSuncoast
{
    class Database
    {
        //Will privately house our users and their information
        private List<Client> users = new List<Client>();
        //Will house bank transaction histories
        private List<Transaction> transactionHistory = new List<Transaction>();
        
        public void ValidCustomer()
        {
            //This method will check if the user exists at this bank and return the correct person
        }

        public void AddClient(Client newClient)
        {
            //Will add new client to the list of users that is sent from the main program
            users.Add(newClient);
        }

        public void AddTransaction(Transaction newTransaction)
        {
            //Will add to transaction history list every time it is called
            transactionHistory.Add(newTransaction);
        }

        public void Deposit()
        {
            //This method will deposit into the customers account
        }

        public void Withdraw()
        {
            //This method will withdraw from the customers account
        }

        public void View()
        {
            //This method will either view the balance or transaction history based on what was passed through the computer
        }
    }
}