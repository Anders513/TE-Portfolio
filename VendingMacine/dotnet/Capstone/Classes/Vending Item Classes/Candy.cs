using System;
using System.Collections.Generic;
using System.Text;


namespace Capstone.Classes
{
    public class Candy: VendingMachineItem
    {
        public Candy(string name, decimal price) : base(name, price) { }

        public override string ItemNoise()
        {
            return "Munch Munch, Yum!";
        }
    }
}
