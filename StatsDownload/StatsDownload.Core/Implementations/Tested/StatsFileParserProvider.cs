﻿namespace StatsDownload.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class StatsFileParserProvider : IStatsFileParserService
    {
        private const string ExpectedHeader = @"name	newcredit	sum(total)	team";

        private readonly IAdditionalUserDataParserService additionalUserDataParserService;

        public StatsFileParserProvider(IAdditionalUserDataParserService additionalUserDataParserService)
        {
            if (additionalUserDataParserService == null)
            {
                throw new ArgumentNullException(nameof(additionalUserDataParserService));
            }

            this.additionalUserDataParserService = additionalUserDataParserService;
        }

        public ParseResults Parse(string fileData)
        {
            var usersData = new List<UserData>();
            var failedUsersData = new List<FailedUserData>();

            string[] fileLines = GetFileLines(fileData);

            if (IsInvalidStatsFile(fileLines))
            {
                throw new InvalidStatsFileException();
            }

            Parse(fileLines, usersData, failedUsersData);

            return new ParseResults(usersData, failedUsersData);
        }

        private string[] GetFileLines(string fileData)
        {
            return fileData?.Replace("\r\n", "\n").Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private bool IsInvalidStatsFile(string[] fileLines)
        {
            return fileLines == null || fileLines.Length < 2 || !ValidDateTime(fileLines[0])
                   || fileLines[1] != ExpectedHeader;
        }

        private bool IsInvalidUserData(string[] unparsedUserData)
        {
            return unparsedUserData.Length != 3 && unparsedUserData.Length != 4;
        }

        private void Parse(string[] fileLines, List<UserData> usersData, List<FailedUserData> failedUsersData)
        {
            for (var lineIndex = 2; lineIndex < fileLines.Length; lineIndex++)
            {
                string currentLine = fileLines[lineIndex];
                string[] unparsedUserData = currentLine.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (IsInvalidUserData(unparsedUserData))
                {
                    failedUsersData.Add(new FailedUserData(currentLine));
                    continue;
                }

                UserData userData;

                if (TryParseUserData(unparsedUserData, out userData))
                {
                    additionalUserDataParserService.Parse(userData);
                    usersData.Add(userData);
                    continue;
                }

                failedUsersData.Add(new FailedUserData(currentLine, userData));
            }
        }

        private bool TryParseUserData(string[] unparsedUserData, out UserData userData)
        {
            var index = 0;
            string name = unparsedUserData[index];
            index++;

            if (unparsedUserData.Length == 3)
            {
                name = string.Empty;
                index--;
            }

            ulong totalPoints;
            ulong totalWorkUnits;
            ulong teamNumber;

            bool totalPointsParsed = ulong.TryParse(unparsedUserData[index], out totalPoints);
            index++;
            bool totalWorkUnitsParsed = ulong.TryParse(unparsedUserData[index], out totalWorkUnits);
            index++;
            bool teamNumberParsed = ulong.TryParse(unparsedUserData[index], out teamNumber);
            index++;

            userData = new UserData(name, totalPoints, totalWorkUnits, teamNumber);

            return totalPointsParsed && totalWorkUnitsParsed && teamNumberParsed;
        }

        private bool ValidDateTime(string dateTime)
        {
            DateTime parsedDateTime;
            var format = "ddd MMM dd HH:mm:ss PST yyyy";
            return DateTime.TryParseExact(dateTime, format, CultureInfo.CurrentCulture,
                DateTimeStyles.NoCurrentDateDefault, out parsedDateTime);
        }
    }
}