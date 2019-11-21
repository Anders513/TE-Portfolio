using Capstone.IReport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes.Report_Classes
{
    public class ChangeLog : ILogReports
    {
        private string _VendingAction { get; set; } = "GIVE CHANGE:";
        private decimal _BalanceAdjustment { get; }
        private decimal _Balance { get; } = 0.00M;

        public ChangeLog(decimal balanceAdjustment)
        {
            _BalanceAdjustment = balanceAdjustment;
        }

        public string WriteLog()
        {
            string logEntry = $"{DateTime.UtcNow} {_VendingAction} ${_BalanceAdjustment} ${_Balance}";
            return logEntry;
        }        
    }
}
