using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Capstone.Classes.Report_Classes;
using Capstone.IReport;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        public VendingMachine() { }

        private Dictionary<string, VendingMachineItem> _inventory = new Dictionary<string, VendingMachineItem>();
        private List<ILogReports> _logReport = new List<ILogReports>();
        
        //Reads contents of Inventory and Updates Inventory
        public VendingMachine(string filePath)
        {
            ReadInventory(filePath);
        }
        private void ReadInventory(string filePath)
        {
            var inventoryLine = File.ReadAllLines(filePath);
            {
                foreach (var vendingItem in inventoryLine)
                {
                    CreateVendingItem(vendingItem);
                }
            }
        }
        public string DisplayInventory()
        {
            string display = "";
            foreach (var item in _inventory)
            {
                string itemInfo = string.Format("{0, -30}", (item.Key + " | " + item.Value.Name));               
                if (item.Value.Quantity == 0)
                {
                    itemInfo += "SOLD OUT";
                }
                else
                {
                    itemInfo += "$"+item.Value.Price.ToString();
                }
                display += itemInfo + "\n";
            }
            return display;
        }
        private void UpdateInventory(string choiceKey)
        {
            var updateInventory = _inventory[choiceKey];
            updateInventory.Quantity--;
        }
        private VendingMachineItem GetItem(string itemChoice)
        {
            return _inventory[itemChoice];
        }

        //Verifies user selection
        public string PurchaseChoice(string itemChoice)
        {
            string purchaseMessage = "";
            if (_inventory.ContainsKey(itemChoice))
            {
                //Verifies if choice is available to user
                if (Balance < _inventory[itemChoice].Price)
                {
                    purchaseMessage = "You do not have enough money to purchase it, please add more money";
                }
                else if (_inventory[itemChoice].Quantity == 0)
                {
                    purchaseMessage = "Item is unavailable";
                }
                else
                {
                    //Updating inventory and balance
                    SubtractMoney(itemChoice);
                    UpdateInventory(itemChoice);

                    //Purchase message
                    purchaseMessage = $"Thank you, you have ${Balance} left in your account";
                    purchaseMessage += $"\nDispensing {_inventory[itemChoice].Name}";
                    purchaseMessage += $"\n{_inventory[itemChoice].ItemNoise()}";
                }
            }
            else
            {
                purchaseMessage = "Invalid choice, press any key to continue";
            }
            return purchaseMessage;
        }
        public bool VerifyMoneyAdded(decimal moneyAdded)
        {
            bool loadedMoney = false;
            if (moneyAdded > 0.00M && moneyAdded <= 20.00M)
            {
                while (moneyAdded > 0)
                {
                    moneyAdded -= 1.00M;
                    if (moneyAdded == 0.00M)
                    {
                        loadedMoney = true;
                    }
                    else if (moneyAdded < 0.00M)
                    {
                        loadedMoney = false;
                    }
                }
            }
            return loadedMoney;
        }

        //Creates Vending Machine Objects
        public void CreateVendingItem(string vendingData)
        {
            string[] vendingInfo = vendingData.Split("|");
            string itemKey = vendingInfo[0];
            string itemName = vendingInfo[1];
            decimal itemPrice = decimal.Parse(vendingInfo[2]);
            string itemType = vendingInfo[3];
            VendingMachineItem vendingMachineItem = null;

            if (itemType == "Chip")
            {
                vendingMachineItem = new Chip(itemName, itemPrice);
            }
            else if (itemType == "Gum")
            {
                vendingMachineItem = new Gum(itemName, itemPrice);
            }
            else if (itemType == "Drink")
            {
                vendingMachineItem = new Drink(itemName, itemPrice);
            }
            else if (itemType == "Candy")
            {
                vendingMachineItem = new Candy(itemName, itemPrice);
            }
            _inventory.Add(itemKey, vendingMachineItem);
        }

        //Handles money of Vending Machine
        public decimal Balance { get; private set; } = 0.00M;
        public decimal AddMoney(decimal money)
        {            
            //Update balance
            Balance += (Math.Round(money, 2));

            //Update Log
            FeedMoneyLog feedmoney = new FeedMoneyLog(Math.Round(money, 2), Balance);
            _logReport.Add(feedmoney);

            return Balance;
        }
        public decimal SubtractMoney(string key)
        {
            //Update Balance
            decimal cost = _inventory[key].Price;
            Balance -= cost;

            //Update Log
            string itemSelection = $"{_inventory[key].Name} {key}";
            ItemPurchaseLog itemPurchase = new ItemPurchaseLog(cost, Balance, itemSelection);
            _logReport.Add(itemPurchase);

            return Balance;
        }

        //Gives change to customer looping through private change functions
        public string GiveChange()
        {
            decimal change = Balance;
            string changeGiven = "Your change is: ";
            if (Balance == 0)
            {
                changeGiven = "Have a nice day";
            }
            else
            {
                if (Balance > 1.00M)
                {
                    changeGiven += $"{CustomerChangeDollars()} dollars";
                }
                if (Balance > .25M)
                {
                    changeGiven += $", {CustomerChangeQuarters()} quarters";
                }
                if (Balance > .10M)
                {
                    changeGiven += $", {CustomerChangeDimes()} dimes";
                }
                if (Balance == .05M)
                {
                    changeGiven += $", {CustomerChangeNickels()} nickels";
                }
                changeGiven += $"\nYour balance is now ${Balance}";
            }
            ChangeLog giveChange = new ChangeLog(change);
            _logReport.Add(giveChange);
            return changeGiven;
        }
        private int CustomerChangeDollars()
        {
            int dollarsReturned = 0;
            while (Balance >= 1.00M)
            {
                dollarsReturned += 1;
                Balance -= 1.00M;
            }
            return dollarsReturned;
        }
        private int CustomerChangeQuarters()
        {
            int quartersReturned = 0;
            while (Balance >= .25M)
            {
                quartersReturned += 1;
                Balance -= .25M;
            }
            return quartersReturned;
        }
        private int CustomerChangeDimes()
        {
            int dimesReturned = 0;
            while (Balance >= .10M)
            {
                dimesReturned += 1;
                Balance -= .10M;
            }
            return dimesReturned;
        }
        private int CustomerChangeNickels()
        {
            int nickelsReturned = 0;
            while (Balance == .05M)
            {
                nickelsReturned += 1;
                Balance -= .05M;
            }
            return nickelsReturned;
        }

        //Reporting functions
        public void WriteLog()
        {
            using (StreamWriter sw = new StreamWriter(@"..\..\..\..\VendingMachineLog.txt", true))
            {
                foreach(var log in _logReport)
                {
                    sw.WriteLine(log.WriteLog());
                }
                sw.WriteLine("END OF LOG FOR CUSTOMER");
                sw.WriteLine();
            }
        }
        public string DisplaySalesReport()
        {
            decimal totalSales = 0.00M;
            string vendingReport = "";
            foreach (var item in _inventory)
            {
                vendingReport += string.Format("{0, -20}",item.Value.Name);
                int numberSold = (5 - item.Value.Quantity);
                totalSales += (numberSold * item.Value.Price);
                vendingReport += $" | {numberSold} \n";
            }
            vendingReport += $"\n **TOTAL SALES** ${totalSales}";
            return vendingReport;
        }
        public void PrintSalesReport()
        {           
            {
                using (StreamWriter sw = new StreamWriter(@"..\..\..\..\VendingMachineSalesLog.txt", false))
                {
                    sw.Write(DisplaySalesReport());
                }
            }
        }
    }
}