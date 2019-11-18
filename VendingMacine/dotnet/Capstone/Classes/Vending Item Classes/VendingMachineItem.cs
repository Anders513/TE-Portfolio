using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Capstone.Classes
{
    public abstract class VendingMachineItem 
    {   
        public string Name { get; set; }
        public decimal Price { get; set; }      
        public int Quantity { get; set; } = 5;

        protected VendingMachineItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public virtual string ItemNoise()
        {
            return "";
        }
    }
}
