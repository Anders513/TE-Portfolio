using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Classes;

namespace Capstone.Classes
{
    public class MenusAndSubmenus
    {
        private VendingMachine _vendingMachine;
        public MenusAndSubmenus(VendingMachine vendingMachine)
        {
            _vendingMachine = vendingMachine;
        }
        private string _userChoice { get; set; }
        private bool _isExit { get; set; } = false;
        private bool _purchaseExit { get; set; } = false;

        public void MainMenu()
        {
            while (_isExit == false)
            {
                Console.WriteLine("Vendo-Matic 600");
                Console.WriteLine();
                Console.WriteLine("1. Display Items");
                Console.WriteLine("2. Purchase Items");
                Console.WriteLine("3. Exit");
                Console.WriteLine();
                Console.Write("Make selection: ");
                _userChoice = Console.ReadLine();
                Console.Clear();

                if (_userChoice == "1")
                {
                    DisplayItems();
                    Console.Clear();
                }
                else if (_userChoice == "2")
                {
                    _purchaseExit = false;
                    while (_purchaseExit == false)
                    {
                        Console.Clear();
                        Console.WriteLine("Purchase Items Menu");
                        Console.WriteLine();
                        Console.WriteLine("1. Feed Money");
                        Console.WriteLine("2. Select Product");
                        Console.WriteLine("3. Finish Transaction");
                        Console.WriteLine();
                        Console.Write("Make selection: ");
                        _userChoice = Console.ReadLine();

                        if (_userChoice == "1")
                        {
                            FeedMoney();
                            Console.Clear();
                        }
                        else if (_userChoice == "2")
                        {
                            PurchaseItem();
                            Console.Clear();
                        }
                        else if (_userChoice == "3")
                        {
                            GiveChange();
                            Console.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Invalid entry, please press any key to continue");
                        }
                    }
                }
                else if (_userChoice == "3")
                {
                    EndProgram();
                }
                else if (_userChoice == "4")
                {
                    PrintReport();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Invalid entry, please press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                }

            }
        }
        private void DisplayItems()
        {
            Console.WriteLine("Display Items");
            Console.WriteLine();
            Console.WriteLine(_vendingMachine.DisplayInventory());
            Console.Write("Press any to return to main menu. ");
            Console.ReadKey();
        }   
        private void FeedMoney()
        {
            Console.Clear();
            Console.WriteLine($"Your total balance is: {_vendingMachine.Balance}");
            Console.WriteLine();
            Console.WriteLine("How much money would you like to add? (Enter whole dollar amounts only up to 20 dollars at a time)");
            decimal moneyAdded = 0;
            try
            {
                moneyAdded = decimal.Parse(Console.ReadLine());
            }
            catch (OverflowException)
            {               
            }
            catch (FormatException)
            {          
            }
            if (_vendingMachine.VerifyMoneyAdded(moneyAdded))
            {             
                Console.Clear();
                _vendingMachine.AddMoney(moneyAdded);
                Console.WriteLine($"Your balance is ${ _vendingMachine.Balance}");
                Console.WriteLine();
                Console.Write("Press any to return to Purchase Items menu. ");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Invalid amount, press any key to continue");
                Console.ReadKey();
            }
        }
        private void PurchaseItem()
        {
            if (_vendingMachine.Balance == 0.00M)
            {
                Console.WriteLine("Please deposit money first. Press any key to continue.");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine(_vendingMachine.DisplayInventory());
                Console.WriteLine();
                Console.Write("Please enter your choice: ");
                string itemChoice = Console.ReadLine();
                Console.WriteLine(_vendingMachine.PurchaseChoice(itemChoice.ToUpper()));
                Console.ReadKey();
            }
        }
        private void GiveChange()
        {
            Console.Clear();           
            Console.WriteLine(_vendingMachine.GiveChange());          
            Console.WriteLine();
            Console.WriteLine("Press any key to return to Vendo-Matic 600 main menu.");
            Console.ReadKey();
            _purchaseExit = true;
        }
        private void PrintReport()
        {
            Console.Clear();
            Console.WriteLine("This is a secret sales report option");
            Console.WriteLine(_vendingMachine.DisplaySalesReport());
            _vendingMachine.PrintSalesReport();
            Console.ReadKey();
        }
        private void EndProgram()
        {
            _vendingMachine.WriteLog();
            Console.Clear();
            Console.WriteLine("Thank you for using the Vendo-Matic 600!");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit program.");
            Console.ReadKey();
            _isExit = true;
        }

    }
}


