using System;
using System.Collections.Generic;
using System.Text;


namespace Capstone.Classes
{
    public class Chip : VendingMachineItem
    {
        public Chip(string name, decimal price) : base(name, price) { }

        public override string ItemNoise()
        {
            return "Crunch Crunch, Yum!";
        }
    }
}
