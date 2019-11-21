using System;
using System.Collections.Generic;
using System.Text;


namespace Capstone.Classes
{
    public class Drink : VendingMachineItem
    {
        public Drink(string name, decimal price) : base(name, price) { }

        public override string ItemNoise()
        {
            return "Glug Glug, Yum!";
        }
    }
}
