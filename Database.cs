using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace FirstBankOfSuncoast
{
    class Database
    {
        //Will privately house our users and their information
        private List<Client> Clients { get; set; } = new List<Client>();
        //Will house bank transaction histories
        private List<Transaction> TransactionHistory { get; set; } = new List<Transaction>();

        //Load our clients from the CSV File or create a CSV File if necessary
        public void LoadClients()
        {
            //Reader process
            if (File.Exists("clients.csv"))
            {
            //Creates a stream reader to get information from our file
            TextReader reader;

            //If the file exists
            if (File.Exists("clients.csv"))
            {
            // Assign a StreamReader to read from the file
            reader = new StreamReader("clients.csv");
            }
            else
            {
            // Assign a StringReader to read from an empty string
            reader = new StringReader("");
            }
        
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
            // Tell the reader to interpret the first row as a "header".
            HasHeaderRecord = true,
            };
        
            // Create a CSV reader to parse the stream into CSV format
            var csvReader = new CsvReader(reader, config);

            //Houses our clients from reading file
            Clients = csvReader.GetRecords<Client>().ToList();

            // Close the reader
            reader.Close();
            }
        }
        //Save our clients to CSV
        public void SaveClients()
        {
            // Create a stream for writing information into a file
            var fileWriter = new StreamWriter("clients.csv");
            // Create an object that can write CSV to the fileWriter
            var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
            // Ask our csvWriter to write out our list of clients
            csvWriter.WriteRecords(Clients);
            // Tell the file we are done
            fileWriter.Close();
        }
        //Load all transactions at the bank from the csv file or create one if necessary
        public void LoadTransactions()
        {
            //Reader process
            if (File.Exists("transactions.csv"))
            {
                //Creates a stream reader to get information from our file
                TextReader reader;

                //If the file exists
                if (File.Exists("transactions.csv"))
                {
                    // Assign a StreamReader to read from the file
                    reader = new StreamReader("transactions.csv");
                }
                else
                {
                    // Assign a StringReader to read from an empty string
                    reader = new StringReader("");
                }
    
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    // Tell the reader to interpret the first row as a "header".
                    HasHeaderRecord = true,
                };
        
                // Create a CSV reader to parse the stream into CSV format
                var csvReader = new CsvReader(reader, config);

                //Houses our transactions from reading file
                TransactionHistory = csvReader.GetRecords<Transaction>().ToList();

                // Close the reader
                reader.Close();
            }
        }
        //Save all transactions after each transaction
        public void SaveTransactions()
        {
            // Create a stream for writing information into a file
            var fileWriter = new StreamWriter("transactions.csv");
            // Create an object that can write CSV to the fileWriter
            var csvWriter = new CsvWriter(fileWriter, CultureInfo.InvariantCulture);
            // Ask our csvWriter to write out our list of transactions
            csvWriter.WriteRecords(TransactionHistory);
            // Tell the file we are done
            fileWriter.Close();
        }
        //Locates a client to send back to the main program
        public Client FindClient(string name)
        {
            //This method will check if the user exists at this bank and return the correct person
            Client foundClient = Clients.FirstOrDefault(client => client.UserName == name.ToUpper());

            return foundClient;
        }
        //Adds a new client when prompted
        public void AddClient(Client newClient)
        {
            //Will add new client to the list of users that is sent from the main program
            Clients.Add(newClient);
        }
        //Adds transaction to list when called
        public void AddTransaction(Transaction newTransaction)
        {
            //Will add to transaction history list every time it is called
            TransactionHistory.Add(newTransaction);
        }
        //Will eventually calculate deposits and such?
        public void Deposit()
        {
            //This method will deposit into the customers account
        }
        //Will eventually calculate withdrawals (ew)
        public void Withdraw()
        {
            //This method will withdraw from the customers account
        }
        //Finds all transactions that have the same user and returns back all of the transactions that match
        public List<Transaction> ViewTransactions(string name)
        {
            //This method will return the transaction history by finding every transaction that matches the name of the user
            var requestedTransactions = TransactionHistory.Where(client => client.UserName == name).ToList();

            return requestedTransactions;
        }
    }
}