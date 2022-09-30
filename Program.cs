// Petter Boström SUT22
using System;
// To use List
using System.Collections.Generic;

namespace Bankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create array for holding user names
            string[] userList = new string[5];
            // Create five test users and put ID in array
            User user1 = new User("Lars", 4378);
            userList[0] = user1.Id;
            User user2 = new User("Frida", 6901);
            userList[1] = user2.Id;
            User user3 = new User("Anna", 0209);
            userList[2] = user3.Id;
            User user4 = new User("Abdul", 7288);
            userList[3] = user4.Id;
            User user5 = new User("Kent", 5156);
            userList[4] = user5.Id;
            // Open two accounts for each user
            user1.OpenAccount("Lönekonto", 23578.32, "SEK");
            user1.OpenAccount("Sparkonto", 7308.5, "EUR");
            user2.OpenAccount("Lönekonto", 248.54, "SEK");
            user2.OpenAccount("Sparkonto", 2310.69, "SEK");
            user3.OpenAccount("Lönekonto", 50344.87, "SEK");
            user3.OpenAccount("Sparkonto", 354275.12, "USD");
            user4.OpenAccount("Lönekonto", 18432.32, "SEK");
            user4.OpenAccount("Sparkonto", 305233.31, "SEK");
            user5.OpenAccount("Lönekonto", 3488.02, "SEK");
            user5.OpenAccount("Sparkonto", 485321.79, "EUR");

            Console.ReadKey();
        }
    }
    // Class for user management
    public class User
    {
        // User Id
        public string Id { get; set; }
        // User security pin
        public int Pin { get; set; }
        // Create list for user accounts
        public List<Account> Accounts = new List<Account>();
        public User(string name, int pin)
        {
            Id = name;
            Pin = pin;
        }
        // Method for opening a new account
        public void OpenAccount(string name, double value, string currency)
        {
            Account account = new Account(name, value, currency);
            Accounts.Add(account);
        }
        // Method for printing out users accounts
        public void PrintAccounts()
        {
            Console.WriteLine(Id);
            foreach (var item in Accounts)
            {
                item.PrintAccount();
            }
        }
        // Method for transferring money
    }
    // Class for account management
    public class Account
    {
        public string AccountName { get; set; }
        public double Value { get; set; }
        public string Currency { get; set; }

        public Account(string name, double value, string currency)
        {
            AccountName = name;
            Value = value;
            Currency = currency;
        }
        // Create method to present variables as string
        public override string ToString()
        {
            return base.ToString();
        }
        // Create method to print account details
        public void PrintAccount()
        {
            Console.WriteLine("Kontonamn: {0} \nSaldo: {1} {2}", AccountName,
                Value, Currency);
        }
    }
}
