using System;
using Capstone.Classes;
using Capstone.Classes.Report_Classes;

namespace Capstone_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Add inventory and setting up menu with inventory
            string inventoryFile = @"..\..\..\..\VendingMachine.txt";
            VendingMachine vendingMachine = new VendingMachine(inventoryFile);
            MenusAndSubmenus mainmenu = new MenusAndSubmenus(vendingMachine);
            mainmenu.MainMenu();
        }
    }
}
