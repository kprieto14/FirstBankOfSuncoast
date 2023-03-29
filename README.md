P(roblem)
Create a console app that allows a user to manage savings and checking banking transactions.

A user will make a series of transactions.

You will compute balances by examining all the transactions in the history. For instance, if a user deposits 10 to their savings, then withdraws 8 from their savings, then deposits 25 to their checking, they have three transactions to consider. Compute the checking and saving balance, using the transaction list, when needed. In this case, their savings balance is 2 and their checking balance is 25.

The transactions will be saved in a file, using a CSV format to record the data.


E(xamples)
Transaction class will allow user to deposit/ withdraw from their checkings/ savings and save that information in a list for the user
User cannot withdraw more than what is in their account





D(ata)
Lists
    Transactions list that keeps track of the transactions
    List of customers/ users
    
Classes
    Transaction class
        User:
        Date
        DepositOrSavings?
        TransactionType = Deposit or Withdraw
        Amount(taken or withdrawn)
        Balance

        List of transactions (send back to user class to be added to their transaction list)?
    
    User Class
        User name
        Pin
        SavingsAmount
        CheckingsAmounts 
        TransactionHistory? (will be a list of Transactions)

        List of users

Switch statements that allows user to make options
    savings or deposit
    deposit or withdrawal

Method and variable to grab date times

While Loop

PromptForString method
PromptForInteger method
    Only return 0 if invalid prompt
Inside the User class
    ValidUser method (this will also check later on if the customer inputs a pin correctly)
    New User method
Inside the transaction class
    Deposit Method
    Withdraw Method
    View Method

A(lgo)
Print greeting(Hello! Welcome to the Bank of Kristy) along with the following steps below
Are you an existing or new customer?
    If E: Call ValidUser method
    
    If N: Call NewCustomer Method
        What is your name?
        What PIN number would you like?
        How much would you like to deposit into your savings?
        How much do you want to deposit into your checkings?
        Print out message with new customer information
    
While loop starts here
Cont from E: Which account do you want to work with? Savings or Checkings? Or enter Q to quit.
        If S: Do you want to withdraw, deposit, or view?
            W: Withdraw method
            D: Deposit method
            V: View method
        If C: Do you want to withdraw, deposit, or view?
            W: Withdraw method
            D: Deposit method
            V: View method
        If Q: Stop program
        Else if: Please enter valid prompt
        Else: Something went wrong. Please contact our Customer Service Representative, Kristy, to fix the code.

Methods!
    ValidUser method
        While Loop begins!
        Enter your bank account name or Q to quit: 
        Check to see if what the user inputs matches whatever is in the list of Users
            If correct: 
            Confirm this is the user you want
            If Y: return User
                end loop
            If N: Restart loop
            If incorrect: No user found, please try again
            If Q: return "Q" to quit program and end loop
    
    newUser method
        var newUser = User();
        promptForString(Please enter your name) = add to newUser name
        add to pin user = promptForInteger
        add to savings = promptforInteger
            Also add to transactionList if > 0
        add to checkings = promptforInteger
            Also add to transactionList if > 0
        Print out message with Customer Information

    Withdraw Method
        Look into user's s/c account
        Print(you currently have {amount} in your bank account, how much would you like to withdraw?)
        amountRequested = PromptforInteger
        If UserAmount > amountRequested
            newBalance = amountRequested - UserAmount
            Wish granted! You have withdrawn {amountRequested}, you have {newBalance} in your {savings/Checkings} Account
            add to transaction class which will then be passed to add to list
        Would you like to do anything else?
            If Y return null?
            If No, return "Q" to end loop

    Deposit Method
        Look into user's s/c account
        amountRequested = PromptForInteger(How much would you like to deposit?)
        newBalance =  amountRequested + UserAmount
        If(amountRequested > 0)
            Print(Your {savings/Checkings} is growing! You have added {amountRequested} to your account for a total of {USerAmount});
            Add to transaction list
            Would you like to do anything else?
            If Y return null?
            If No, return "Q" to end loop
        Else
            Print(Looks like you didn't add anything)
            Would you like to do anything else?
            If Y return null?
            If No, return "Q" to end loop
    
    View Method
        Look into user's s/c account
        promptForString(Would you like to look at your balance or transaction history?)
            B:
                Print userAmount depending on savings or checkings
                Would you like to do anything else?
                    If Y return null?
                    If No, return "Q" to end loop
            T:
                Print out transaction history
                Would you like to do anything else?
                    If Y return null?
                    If No, return "Q" to end loop
        


Code
