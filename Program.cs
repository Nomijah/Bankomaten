// Petter Boström SUT22
using System;

namespace Bankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /* Array for holding user names and pin numbers with creation 
             * of test users. */
            string[,] userList = new string[,]
            { { "Lukas", "4378" },
            { "Frida", "6901" },
            { "Elsa", "0209" },
            { "Abdul", "7288"},
            { "Kent", "5156" } };

            // Array with account types
            string[] accName = new string[] {"Lönekonto", "Sparkonto",
                "Resekonto", "Barnsparkonto", "Gemensamt konto" };

            /* Array with accounts for each user, every other number is 
             * account type and every other is value. */
            decimal[] LukasKonton = new decimal[] { 0, 13278.32m, 2, 36000 };
            decimal[] FridasKonton = new decimal[] { 0, 2435.67m, 1, 124762.35m, 
                4, 34238.98m };
            decimal[] ElsasKonton = new decimal[] { 0, 36452.55m, };
            decimal[] AbdulsKonton = new decimal[] { 0, 55435.11m, 3, 8540 };
            decimal[] KentsKonton = new decimal[] 
                { 0, 243.45m, 1, 36000, 2, 100 };

            Console.ReadKey();
        }
        // Method for printing out user accounts
        public static void PrintAccount(string[] AccName, decimal[] Account)
        {
            // Counter for list number
            int counter = 1;
            Console.WriteLine("Dina konton:");
            // Loop through accounts
            for (int i = 0; i < Account.Length; i++)
            {
                // Print account name for each even index in account array
                if (i%2 == 0)
                {
                    Console.Write("{0}. {1}: ", counter, AccName[i]);
                    counter++;
                }
                // Print amount for every other index
                else
                {
                    Console.Write(Account[i].ToString() + " SEK\n");
                }
            }
        }
        // Method for transferring money between accounts
        public static decimal[] TransferMoney(string[] AccName, decimal[] Account)
        {
            // Check if user has more than one account
            if (Account.Length < 4)
            {
                Console.WriteLine("Du har bara ett konto.");
            }
            else
            {
                // Print users accounts
                PrintAccount(AccName, Account);
                // Ask which account to transfer from
                Console.Write("Vilket konto vill du föra över pengar från?" +
                    "(ange siffran): ");
                int fromAccount = CheckAccount(Account);
                // Ask which account to transfer to
                Console.Write("Vilket konto vill du föra över pengar till?" +
                    "(ange siffran): ");
                int toAccount = CheckAccount(Account);
                // Ask how much to transfer
                Console.WriteLine("Saldo: {0}", Account[fromAccount]);
                bool amountError = true;
                decimal transferSum = 0;
                while (amountError)
                {
                    try
                    {
                        Console.WriteLine("Hur mycket pengar vill du föra" +
                            " över?");
                        transferSum = decimal.Parse(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Felaktig inmatning, använd enbart" +
                            " siffror och använd kommatecken före ören.");
                    }
                    // Check if given sum is available in account
                    if (transferSum <= 0 || transferSum > Account[fromAccount])
                    {
                        Console.WriteLine("Den angivna summan finns inte " +
                            "på kontot");
                    }
                    else
                    {
                        // Transfer the money and present new account balance
                        Account[fromAccount] = 
                            Account[fromAccount] - transferSum;
                        Account[toAccount] = Account[toAccount] + transferSum;
                        Console.WriteLine("Överföringen lyckades. Dina nya" +
                            " saldon är:");
                        PrintAccount(AccName, Account);
                        amountError = false;
                    }
                }
                
            }
            return Account;
        }

        private static int CheckAccount(decimal[] Account)
        {
            bool error = true;
            int userChoice = 0;
            while (error)
            {
                string userInput = Console.ReadLine();
                if (!Int32.TryParse(userInput, out userChoice))
                {
                    Console.WriteLine("Använd enbart siffror.");
                }
                else if (Int32.Parse(userInput) > Account.Length / 2
                    || Int32.Parse(userInput) < 1)
                {
                    Console.WriteLine("Siffran du angivit finns inte, " +
                        "försök igen.");
                }
                else
                {
                    userChoice = (Int32.Parse(userInput) * 2) - 1;
                    error = false;
                }
            }
            return userChoice;
        }
    }
}
