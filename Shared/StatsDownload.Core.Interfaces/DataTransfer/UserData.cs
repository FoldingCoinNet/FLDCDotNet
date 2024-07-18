﻿namespace StatsDownload.Core.Interfaces.DataTransfer
{
    public class UserData
    {
        public UserData()
            : this(0, null, 0, 0, 0)
        {
        }

        public UserData(int lineNumber, string name, long totalPoints, long totalWorkUnits, long teamNumber)
        {
            LineNumber = lineNumber;
            Name = name;
            TotalPoints = totalPoints;
            TotalWorkUnits = totalWorkUnits;
            TeamNumber = teamNumber;
        }

        public string BitcoinAddress { get; set; }

        public string BitcoinCashAddress { get; set; }

        public string CashTokensAddress { get; set; }

        public string FriendlyName { get; set; }

        public int LineNumber { get; }

        public string Name { get; }

        public string SlpAddress { get; set; }

        public long TeamNumber { get; }

        public long TotalPoints { get; }

        public long TotalWorkUnits { get; }
    }
}