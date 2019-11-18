using System;
using System.Collections.Generic;
using System.Text;


namespace Capstone.Classes
{
    public class Gum : VendingMachineItem
    {
        public Gum(string name, decimal price) : base(name, price) { }

        public override string ItemNoise()
        {
            return "Chew Chew, Yum!";
        }
    }
}
