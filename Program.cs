// Petter Boström SUT22
using System;

namespace Bankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool runProgram = true;
            while (runProgram)
            {
                // Variable to save users index place in login-array
                int userIndex;
                // Run UserLogin and save the users accounts to userAcc
                decimal[] userAcc = UserLogin(out userIndex);
                // If user is successfully logged in, run program
                if (userAcc != null)
                {
                    runProgram = MainMenu(userAcc, userIndex);
                }
                else
                {
                    Console.WriteLine("\nDu har angett fel kod tre gånger," +
                        " programmet avslutas.");
                    runProgram = false;
                }
            }
            Console.WriteLine("\nTryck på valfri tangent för att avsluta.");
            Console.ReadKey();
        }

        // Method for user login
        public static decimal[] UserLogin(out int userIndex)
        {
            // Variable to save user index for further use in program
            userIndex = 0;
            Console.WriteLine("Skriv ditt användarnamn:");
            string userInput = Console.ReadLine();
            // Check if user name is valid
            bool notFound = true;
            while (notFound)
            {
                for (int i = 0; i < GlobalInfo.userList.GetLength(0); i++)
                {
                    // Check if entered user name is in userList
                    if (userInput == GlobalInfo.userList[i, 0])
                    {
                        userIndex = i;
                        i = GlobalInfo.userList.GetLength(0);
                        notFound = false;
                    }
                    // If username is not found, ask user to try again
                    else if (i == GlobalInfo.userList.GetLength(0) - 1)
                    {
                        Console.WriteLine("Användarnamnet saknas," +
                            " försök igen.");
                        userInput = Console.ReadLine();
                    }
                }
            }
            bool pinCheck = CheckPin(userIndex);
            if (pinCheck)
            {
                // Return accounts for logged in user
                switch (userIndex)
                {
                case 0:
                    return GlobalInfo.LukasAcc;
                case 1:
                    return GlobalInfo.FridaAcc;
                case 2:
                    return GlobalInfo.ElsaAcc;
                case 3:
                    return GlobalInfo.AbdulAcc;
                case 4:
                    return GlobalInfo.KentAcc;
                default:
                        return null;
                }
            }
            else
            {
                // If login failed, return null
                return null;
            }
        }

        // Method for checking pin code
        public static bool CheckPin(int UserIndex)
        {
            Console.Write("Skriv din fyrsiffriga pinkod: ");
            string userInput = Console.ReadLine();
            int i = 2;
            bool correctPin = false;
            do
            {
                // Check if entered pin is correct
                if (userInput == GlobalInfo.userList[UserIndex, 1])
                {
                    correctPin = true;
                    // change value of i to break loop
                    i = 0;
                }
                else
                {
                    Console.Write($"Fel kod, du har {i} försök kvar. " +
                        "\nSkriv din fyrsiffriga pinkod: ");
                    userInput = Console.ReadLine();
                    i--;
                }
            } while (i > 0);
            // If pin was not entered correctly in three tries, return false
            if (!correctPin)
                return false;
            else
                return true;
        }

        // Method for printing all user accounts
        public static void PrintAccount(decimal[] Account)
        {
            // Counter for list number
            int counter = 1;
            Console.WriteLine("\nDina konton:");
            // Loop through accounts
            for (int i = 0; i < Account.Length; i++)
            {
                // Print account name for each even index in account array
                if (i % 2 == 0)
                {
                    Console.Write("{0}. {1}: ", counter, 
                        GlobalInfo.accName[Convert.ToInt32(Account[i])]);
                    counter++;
                }
                // Print amount for every other index
                else
                {
                    Console.Write(Account[i].ToString() + " SEK\n");
                }
            }
        }

        // Method for printing single user account
        public static void PrintSingleAccount(int type, decimal balance)
        {
            Console.WriteLine($"{GlobalInfo.accName[type]} {balance} SEK");
        }

        //Method for transferring money between accounts
        public static decimal[] TransferMoney
            (decimal[] Account)
        {
            // Check if user has more than one account
            if (Account.Length < 4)
            {
                Console.WriteLine("\nDu har bara ett konto.");
            }
            else
            {
                // Print users accounts
                PrintAccount(Account);
                // Ask which account to transfer from
                Console.Write("\nVilket konto vill du föra över pengar från?" +
                    "(ange siffran): ");
                int fromAccount = CheckAccount(Account);
                // Check if the chosen account is empty
                while (EmptyAccount(Account, fromAccount))
                {
                    Console.Write("Kontot du har valt är tomt, vänligen" +
                        " välj ett annat konto: ");
                    fromAccount = CheckAccount(Account);
                }
                // Ask which account to transfer to
                Console.Write("Vilket konto vill du föra över pengar till?" +
                    "(ange siffran): ");
                int toAccount = CheckAccount(Account);
                // Check if the same account is chosen again
                while (toAccount == fromAccount)
                {
                    Console.WriteLine("\nDu måste välja ett annat konto" +
                        " än det du redan valt.");
                    toAccount = CheckAccount(Account);
                }
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
                        Console.WriteLine("\nÖverföringen lyckades. Dina nya" +
                            " saldon är:");
                        PrintSingleAccount
                            (Convert.ToInt32(Account[fromAccount - 1]),
                            Account[fromAccount]);
                        PrintSingleAccount
                            (Convert.ToInt32(Account[toAccount - 1]), 
                            Account[toAccount]);
                        amountError = false;
                    }
                }

            }
            return Account;
        }

        // Method for withdrawing money
        public static decimal[] WithdrawMoney(decimal[] Account, int UserIndex)
        {
            // Print users accounts
            PrintAccount(Account);
            // Ask which account to withdraw from
            Console.Write("Vilket konto vill du ta ut pengar ifrån?" +
                "(ange siffran): ");
            int fromAccount = CheckAccount(Account);
            // Check if chosen account is empty
            while (EmptyAccount(Account, fromAccount))
            {
                Console.Write("\nKontot du har valt är tomt, vänligen " +
                    "välj ett annat konto: ");
                fromAccount = CheckAccount(Account);
            }
            // Ask how much to withdraw
            Console.WriteLine("Saldo: {0}", Account[fromAccount]);
            bool amountError = true;
            decimal userSum = 0;
            while (amountError)
            {
                try
                {
                    Console.WriteLine("Hur mycket pengar vill du ta ut?");
                    userSum = decimal.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Felaktig inmatning, använd enbart" +
                        " siffror och använd kommatecken före ören.");
                }
                // Check if given sum is available in account
                if (userSum <= 0 || userSum > Account[fromAccount])
                {
                    Console.WriteLine("Den angivna summan finns inte " +
                        "på kontot");
                }
                else
                {
                    // Ask user for pin code verification
                    if (CheckPin(UserIndex))
                    {
                        // Withdraw given amount from account
                        Account[fromAccount] =
                            Account[fromAccount] - userSum;
                        Console.WriteLine("Uttaget lyckades. Ditt nya" +
                            " saldo är:");
                        PrintSingleAccount
                            (Convert.ToInt32(Account[fromAccount - 1]),
                            Account[fromAccount]);
                    }
                    else
                    {
                        Console.WriteLine("Uttaget godkändes inte.");
                    }
                    amountError = false;
                }
            }
            return Account;
        }

        // Method for checking if user has chosen an existing account
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
                    // Convert the given number to correct index number in array
                    userChoice = (Int32.Parse(userInput) * 2) - 1;
                    error = false;
                }
            }
            return userChoice;
        }

        // Method for checking if account is empty
        private static bool EmptyAccount(decimal[] Account, int accNum)
        {
            return Account[accNum] == 0 ? true : false;
        }

        // Method for checking if all accounts are empty
        private static bool AllEmpty(decimal[] Account)
        {
            bool empty = true;
            for (int i = 1; i < Account.GetLength(0); i += 2)
            {
                if (Account[i] > 0)
                    empty = false;
            }
            return empty;
        }

        // Method for opening a new account
        private static decimal[] OpenAccount(decimal[] Account)
        {
            Console.WriteLine("Vilken typ av konto vill du öppna?");
            int counter = 1;
            foreach (var item in GlobalInfo.accName)
            {
                Console.WriteLine($"{counter}. {item}");
                counter++;
            }
            Console.Write("\nAnge siffran för kontotypen du vill öppna: ");
            string userInput = Console.ReadLine();
            // Check if user input is valid
            while (userInput != "1" && userInput != "2" && userInput != "3"
                && userInput != "4" && userInput != "5")
            {
                Console.Write("Felaktig inmatning, välj en siffra mellan" +
                    " 1-5: ");
                userInput = Console.ReadLine();
            }
            // variable to save chosen account type index
            int nameIndex = Int32.Parse(userInput) - 1;
            // Create new larger array for accounts
            decimal[] newAccounts = new decimal[Account.Length + 2];
            // Transfer info from old array to new
            for (int i = 0; i < Account.Length; i++)
            {
                newAccounts[i] = Account[i];
            }
            // Add new account to the array
            newAccounts[Account.Length] = nameIndex;
            newAccounts[Account.Length + 1] = 0.00M;
            Console.WriteLine($"Ett {GlobalInfo.accName[nameIndex]} är nu " +
                $"öppnat.");
            return newAccounts;
        }

        // Method for menu choice
        private static bool MainMenu(decimal[] Account, int userIndex)
        {
            // boolean for return value
            bool keepGoing = true;
            bool userLoggedIn = true;
            while (userLoggedIn)
            {
                Console.WriteLine("\nVad vill du göra?\n" +
                "1. Se dina konton och saldo\n" +
                "2. Överföra pengar mellan konton\n" +
                "3. Ta ut pengar\n" +
                "4. Öppna ett nytt konto\n" +
                "5. Logga ut\n" +
                "6. Stäng programmet");
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1":
                        Console.Clear();
                        PrintAccount(Account);
                        break;
                    case "2":
                        Console.Clear();
                        // Check if user has any money to transer
                        if (AllEmpty(Account))
                            Console.WriteLine("Du har tyvärr inga pengar.");
                        else
                            Account = TransferMoney(Account);
                        break;
                    case "3":
                        Console.Clear();
                        // Check if user has any money to withdraw
                        if (AllEmpty(Account))
                            Console.WriteLine("Du har tyvärr inga pengar.");
                        else
                            Account = WithdrawMoney(Account, userIndex);
                        break;
                    case "4":
                        Console.Clear();
                        Account = OpenAccount(Account);
                        break;
                    case "5":
                        Console.Clear();
                        keepGoing = true;
                        userLoggedIn = false;
                        break;
                    case "6":
                        Console.Clear();
                        keepGoing = false;
                        userLoggedIn = false;
                        break;
                    default:
                        Console.WriteLine("Felaktigt val, ange endast siffra " +
                            "+ enter.");
                        break;
                }
            }
            // Update accounts at GlobalInfo before exiting
            switch (userIndex)
            {
                case 0:
                    GlobalInfo.LukasAcc = Account;
                    break;
                case 1:
                    GlobalInfo.FridaAcc = Account;
                    break;
                case 2:
                    GlobalInfo.ElsaAcc = Account;
                    break;
                case 3:
                    GlobalInfo.AbdulAcc = Account;
                    break;
                case 4:
                    GlobalInfo.KentAcc = Account;
                    break;
            }
            return keepGoing;
        }
    }

    // Class for holding user info, account names and user accounts
    public static class GlobalInfo
    {
        /* Array for holding user names and pin numbers with creation 
             * of test users. */
        public static string[,] userList = new string[,]
            { { "Lukas", "4378" },
            { "Frida", "6901" },
            { "Elsa", "0209" },
            { "Abdul", "7288"},
            { "Kent", "5156" } };

        // Array with account types
        public static string[] accName = new string[] {"Lönekonto", "Sparkonto",
                "Resekonto", "Barnsparkonto", "Gemensamt konto" };

        /* Array with accounts for each user, every other number is 
              * account type and every other is value. */
        public static decimal[] LukasAcc = new decimal[]
            { 0, 13278.32m, 2, 36000 };
        public static decimal[] FridaAcc = new decimal[] 
            { 0, 2435.67m, 1, 124762.35m, 4, 34238.98m };
        public static decimal[] ElsaAcc = new decimal[] 
            { 0, 36452.55m, };
        public static decimal[] AbdulAcc = new decimal[] 
            { 0, 55435.11m, 3, 8540 };
        public static decimal[] KentAcc = new decimal[]
           { 0, 243.45m, 1, 36000, 2, 100 };
    }
}
