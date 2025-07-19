using System;
using System.Collections.Generic;
using System.Linq;

namespace Super_Money
{
    internal class BankAccount
    {
        public string BankName { get; private set; }
        public string MobileNumber { get; set; }
        public double Balance { get; private set; }
        public List<string> TransactionHistory { get; private set; }
        public double CashbackBalance { get; private set; }

        public BankAccount(string bankName, string mobileNumber, double initialBalance)
        {
            BankName = bankName;
            MobileNumber = mobileNumber;
            Balance = initialBalance;
            TransactionHistory = new List<string>();
            CashbackBalance = 0.00;
        }

        public void SetBalance(double newBalance)
        {
            Balance = newBalance;
        }

        public void AddTransaction(string transaction)
        {
            TransactionHistory.Add(transaction);
        }

        public void AddCashback(double amount)
        {
            CashbackBalance += amount;
        }

        public void ResetCashback()
        {
            CashbackBalance = 0.00;
        }
    }

    public class Super_Money_App
    {
        private static int storedPasskey = 0;

        private static List<BankAccount> userBankAccounts = new List<BankAccount>();

        private static BankAccount currentActiveBank = null;

        private const double CASHBACK_PERCENTAGE = 0.05;

        public static void Main(string[] args)
        {
            userBankAccounts.Add(new BankAccount("State Bank of India", "", 10000.00));
            userBankAccounts.Add(new BankAccount("HDFC Bank", "", 12000.00));
            userBankAccounts.Add(new BankAccount("ICICI Bank", "", 15000.00));
            userBankAccounts.Add(new BankAccount("Axis Bank", "", 11000.00));

            if (storedPasskey == 0)
            {
                Console.WriteLine("Welcome to Super_Money! Let's set up your passkey.");
                SetPasskey();
            }

            if (!AuthenticatePasskey())
            {
                Console.WriteLine("Authentication failed. Exiting application.");
                Console.ReadKey();
                return;
            }

            if (!LinkBankAccount())
            {
                Console.WriteLine("Bank account linking failed. Exiting application.");
                Console.ReadKey();
                return;
            }

            ShowDashboard();
        }

        private static void SetPasskey()
        {
            while (true)
            {
                Console.Write("Please enter a 4-digit passkey for Super_Money: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int newPasskey))
                {
                    if (input.Length == 4)
                    {
                        storedPasskey = newPasskey;
                        Console.WriteLine("Super_Money Passkey set successfully!");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid passkey. Please enter exactly 4 digits.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numerical passkey.");
                }
            }
        }

        private static bool AuthenticatePasskey()
        {
            const int MAX_ATTEMPTS = 3;
            int attempt = 0;
            while (attempt < MAX_ATTEMPTS)
            {
                Console.Write("Enter your Super_Money Passkey: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int enteredPasskey))
                {
                    if (enteredPasskey == storedPasskey)
                    {
                        Console.WriteLine("Super_Money Passkey authenticated successfully!");
                        return true;
                    }
                    else
                    {
                        attempt++;
                        int remainingAttempts = MAX_ATTEMPTS - attempt;
                        Console.WriteLine("You have entered the wrong Passkey.");
                        if (remainingAttempts > 0)
                        {
                            Console.WriteLine("Remaining attempts: " + remainingAttempts);
                            Console.WriteLine("-----Please Try Again-----");
                        }
                        else
                        {
                            Console.WriteLine("No attempts remaining. Access denied.");
                            return false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numerical Passkey.");
                    attempt++;
                    int remainingAttempts = MAX_ATTEMPTS - attempt;
                    if (remainingAttempts > 0)
                    {
                        Console.WriteLine("Remaining attempts: " + remainingAttempts);
                        Console.WriteLine("-----Please Try Again-----");
                    }
                    else
                    {
                        Console.WriteLine("No attempts remaining. Access denied.");
                        return false;
                    }
                }
            }
            return false;
        }

        private static bool LinkBankAccount()
        {
            Console.WriteLine("\n--- Link Your Bank Account ---");
            string mobileNum;
            while (true)
            {
                Console.Write("Please enter your 10-digit mobile number: ");
                mobileNum = Console.ReadLine();

                if (mobileNum != null && mobileNum.Length == 10 && mobileNum.All(char.IsDigit))
                {
                    break;
                }
                else
                {
                    Console.WriteLine(
                        "Error: The mobile number must be exactly 10 digits and contain only numbers. Please try again.");
                }
            }

            List<BankAccount> foundBanks = new List<BankAccount>();
            foreach (BankAccount bank in userBankAccounts)
            {
                foundBanks.Add(bank);
            }

            Console.WriteLine("\nBanks found for " + mobileNum + ":");
            if (!foundBanks.Any())
            {
                Console.WriteLine("No bank accounts found for this mobile number. Please try again.");
                return false;
            }
            for (int i = 0; i < foundBanks.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + foundBanks[i].BankName);
            }

            int selectedBankIndex = -1;
            while (selectedBankIndex < 0 || selectedBankIndex >= foundBanks.Count)
            {
                Console.Write("Select a bank by number: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int selectedNum))
                {
                    selectedBankIndex = selectedNum - 1;
                    if (selectedBankIndex < 0 || selectedBankIndex >= foundBanks.Count)
                    {
                        Console.WriteLine("Invalid selection. Please enter a number from the list.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }

            currentActiveBank = foundBanks[selectedBankIndex];
            currentActiveBank.MobileNumber = mobileNum;
            Console.WriteLine("You have selected: " + currentActiveBank.BankName);

            Random rand = new Random();
            int generatedOtp = rand.Next(1000, 10000);
            Console.WriteLine($"A 4-digit OTP has been sent to {mobileNum} (Simulated OTP: {generatedOtp})");

            const int OTP_MAX_ATTEMPTS = 3;
            int otpAttempt = 0;
            while (otpAttempt < OTP_MAX_ATTEMPTS)
            {
                Console.Write("Enter the OTP: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int enteredOtp))
                {
                    if (enteredOtp == generatedOtp)
                    {
                        Console.WriteLine($"OTP verified successfully! {currentActiveBank.BankName} is now linked.");
                        return true;
                    }
                    else
                    {
                        otpAttempt++;
                        Console.WriteLine("Incorrect OTP. Remaining attempts: " + (OTP_MAX_ATTEMPTS - otpAttempt));
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a numerical OTP.");
                    otpAttempt++;
                }
            }
            Console.WriteLine("OTP verification failed. Too many attempts.");
            return false;
        }

        private static void ShowDashboard()
        {
            Console.WriteLine("******************************************");
            Console.WriteLine("        Welcome to the Super_Money Dashboard!       ");
            Console.WriteLine("******************************************");
            Console.WriteLine(
                "Currently active bank: " + (currentActiveBank != null ? currentActiveBank.BankName : "None"));
            Console.WriteLine();

            int choice;
            do
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Balance");
                Console.WriteLine("2. Send Money");
                Console.WriteLine("3. History");
                Console.WriteLine("4. Reward (Cashback)");
                Console.WriteLine("5. Change Bank Account");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();
                if (!int.TryParse(input, out choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 6.");
                    choice = 0;
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        DisplayBalance();
                        break;
                    case 2:
                        SendMoney();
                        break;
                    case 3:
                        DisplayHistory();
                        break;
                    case 4:
                        DisplayReward();
                        break;
                    case 5:
                        Console.WriteLine("\nAttempting to change bank account...");
                        if (!LinkBankAccount())
                        {
                            Console.WriteLine("Failed to change bank account. Returning to main menu.");
                        }
                        else
                        {
                            Console.WriteLine("Bank account changed successfully to: " + currentActiveBank.BankName);
                        }
                        break;
                    case 6:
                        Console.WriteLine("Thank you for using Super_Money! Goodbye.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option (1-6).");
                        break;
                }
            } while (choice != 6);
        }

        private static void DisplayBalance()
        {
            if (currentActiveBank != null)
            {
                Console.WriteLine($"Your current balance in {currentActiveBank.BankName}: ${currentActiveBank.Balance:F2}");
            }
            else
            {
                Console.WriteLine("No bank account is currently linked. Please link one to view balance.");
            }
        }

        private static void SendMoney()
        {
            if (currentActiveBank == null)
            {
                Console.WriteLine("Please link a bank account before sending money.");
                return;
            }

            Console.Write("Enter recipient's name/account (e.g., John Doe): ");
            string recipient = Console.ReadLine();

            Console.Write("Enter amount to send: $");
            string amountInput = Console.ReadLine();
            if (!double.TryParse(amountInput, out double amount))
            {
                Console.WriteLine("Invalid amount. Please enter a number.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Amount must be positive.");
                return;
            }

            if (currentActiveBank.Balance >= amount)
            {
                currentActiveBank.SetBalance(currentActiveBank.Balance - amount);

                double cashbackEarned = amount * CASHBACK_PERCENTAGE;
                currentActiveBank.AddCashback(cashbackEarned);

                string transaction = $"Sent ${amount:F2} to {recipient} from {currentActiveBank.BankName}. Earned ${cashbackEarned:F2} cashback.";
                currentActiveBank.AddTransaction(transaction);

                Console.WriteLine($"Successfully sent ${amount:F2} to {recipient} from {currentActiveBank.BankName}.");
                Console.WriteLine($"You earned ${cashbackEarned:F2} cashback! Your new cashback balance is ${currentActiveBank.CashbackBalance:F2}.");
                DisplayBalance();
            }
            else
            {
                Console.WriteLine($"Insufficient balance in {currentActiveBank.BankName}. Current balance: ${currentActiveBank.Balance:F2}");
            }
        }

        private static void DisplayHistory()
        {
            if (currentActiveBank == null)
            {
                Console.WriteLine("No bank account is currently linked to view history.");
                return;
            }
            Console.WriteLine($"\n--- Transaction History for {currentActiveBank.BankName} ---");
            List<string> history = currentActiveBank.TransactionHistory;
            if (history.Count == 0)
            {
                Console.WriteLine("No transactions yet for this bank account.");
            }
            else
            {
                for (int i = 0; i < history.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {history[i]}");
                }
            }
        }

        private static void DisplayReward()
        {
            if (currentActiveBank == null)
            {
                Console.WriteLine("No bank account is currently linked to view rewards.");
                return;
            }
            Console.WriteLine($"Your current cashback reward in {currentActiveBank.BankName}: ${currentActiveBank.CashbackBalance:F2}");

            if (currentActiveBank.CashbackBalance > 0)
            {
                Console.Write($"Do you want to redeem your cashback to your {currentActiveBank.BankName} main balance? (yes/no): ");
                string redeemChoice = Console.ReadLine()?.Trim().ToLower();
                if (redeemChoice == "yes")
                {
                    currentActiveBank.SetBalance(currentActiveBank.Balance + currentActiveBank.CashbackBalance);
                    Console.WriteLine($"Successfully redeemed ${currentActiveBank.CashbackBalance:F2} cashback to your {currentActiveBank.BankName} main balance.");
                    currentActiveBank.ResetCashback();
                    DisplayBalance();
                }
                else
                {
                    Console.WriteLine("Cashback not redeemed.");
                }
            }
        }
    }
}