using Capstone.IReport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes.Report_Classes
{
    public class ItemPurchaseLog: ILogReports
    {
        private string _VendingChoice { get; } 
        private decimal _BalanceAdjustment { get; }
        private decimal _Balance { get; }

        public ItemPurchaseLog(decimal balanceAdjustment, decimal balance, string itemchoice)
        {
            _BalanceAdjustment = balanceAdjustment;
            _Balance = balance;
            _VendingChoice = itemchoice;
        }

        public string WriteLog()
        {
            string logEntry = $"{DateTime.UtcNow} {_VendingChoice} ${_BalanceAdjustment} ${_Balance}";
            return logEntry;
        }
    }
}
