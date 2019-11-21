using Capstone.IReport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes.Report_Classes
{    
    public class FeedMoneyLog : ILogReports
    {
        private string _VendingAction { get; set; } = "FEED MONEY:";
        private decimal _BalanceAdjustment { get; }
        private decimal _Balance { get; }

        public FeedMoneyLog(decimal balanceAdjustment, decimal balance)
        {
            _BalanceAdjustment = balanceAdjustment;
            _Balance = balance;
        }

        public string WriteLog()
        {
            string logEntry = $"{DateTime.UtcNow} {_VendingAction} " +
                $"${_BalanceAdjustment.ToString("0.00")} ${_Balance}";
            return logEntry;
        }
    }
}
