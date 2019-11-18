using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Capstone.Classes;


namespace CapstoneTests
{
    [TestClass]
    public class UnitTest1
    {
        //Vending Item Classes
        [TestMethod]
        public void CreateVendingItems()
        {
            Chip chip = new Chip("Potato Crisps", 3.05M);
            Assert.AreEqual(chip.Name, "Potato Crisps");
            Assert.AreEqual(chip.Price, 3.05M);

            Candy candy = new Candy("Moonpie", 1.80M);
            Assert.AreEqual(candy.Name, "Moonpie");
            Assert.AreEqual(candy.Price, 1.80M);

            Drink drink = new Drink("Cola", 1.25M);
            Assert.AreEqual(drink.Name, "Cola");
            Assert.AreEqual(drink.Price, 1.25M);

            Gum gum = new Gum("U-Chews", .85M);
            Assert.AreEqual(gum.Name, "U-Chews");
            Assert.AreEqual(gum.Price, .85M);

        }
        [TestMethod]
        public void CreateDictionary()
        {
            using (StreamWriter sw = new StreamWriter(@"..\..\..\..\DictionaryTest.txt"))
            {
                sw.WriteLine("A1|Potato Crisps|3.05|Chip");
                sw.WriteLine("B1|Moonpie|1.80|Candy");
                sw.WriteLine("C1|Cola|1.25|Drink");
                sw.WriteLine("D1|U-Chews|0.85|Gum");
            }            
            VendingMachine vendingMachine = new VendingMachine();
            vendingMachine.ReadInventory(@"..\..\..\..\DictionaryTest.txt");           
            Assert.IsInstanceOfType(vendingMachine._Inventory["A1"], typeof(Chip));
            Assert.IsInstanceOfType(vendingMachine._Inventory["B1"], typeof(Candy));
            Assert.IsInstanceOfType(vendingMachine._Inventory["C1"], typeof(Drink));
            Assert.IsInstanceOfType(vendingMachine._Inventory["D1"], typeof(Gum));

            vendingMachine.UpdateInventory("A1");
            Assert.AreEqual(4, vendingMachine._Inventory["A1"].Quantity);
            vendingMachine.UpdateInventory("B1");
            vendingMachine.UpdateInventory("B1");
            Assert.AreEqual(3, vendingMachine._Inventory["B1"].Quantity);
        }

        [TestMethod]
        public void BalanceUpdate()
        {
            VendingMachine vendingMoney = new VendingMachine();

            Assert.AreEqual(5.00M, vendingMoney.AddMoney(5.00M));
            Assert.AreEqual(15.00M, vendingMoney.AddMoney(10.00M));
            Assert.AreEqual(15.00M, vendingMoney.Balance);
        }

        [TestMethod]
        public void ChangeTest()
        {
            VendingMachine vendingMoney = new VendingMachine();
            vendingMoney.AddMoney(5);
            Assert.AreEqual(20, vendingMoney.CustomerChangeQuarters());
            Assert.AreEqual(0, vendingMoney.CustomerChangeQuarters());

            vendingMoney.AddMoney(5);
            Assert.AreEqual(50, vendingMoney.CustomerChangeDimes());
            Assert.AreEqual(0, vendingMoney.CustomerChangeDimes());

            vendingMoney.AddMoney(5);
            Assert.AreEqual(100, vendingMoney.CustomerChangeNickels());
            Assert.AreEqual(0, vendingMoney.CustomerChangeNickels());
        }

        [TestMethod]
        public void SalesReport()
        {
            VendingMachine vendingMachine = new VendingMachine();
            vendingMachine.WriteLog("FEED ME", 5.00M, ":", @"..\..\..\..\VendingMachineLogTest.txt");
            using (StreamReader sr = new StreamReader(@"..\..\..\..\VendingMachineLogTest.txt"))
            {
                string testLine=sr.ReadLine();
                string[] testObjects = testLine.Split(" ");
                Assert.AreEqual("FEED", testObjects[3]);
                Assert.AreEqual("ME", testObjects[4]);
                Assert.AreEqual("$5.00", testObjects[6]);
                Assert.AreEqual("0.00", testObjects[7]);
            }
        }
    }
}
