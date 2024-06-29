﻿namespace StatsDownload.Logging.Tests
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using StatsDownload.Core.Interfaces;
    using StatsDownload.Core.Interfaces.DataTransfer;
    using StatsDownload.Core.Interfaces.Enums;

    [TestFixture]
    public class TestErrorMessageProvider
    {
        [SetUp]
        public void SetUp()
        {
            systemUnderTest = new ErrorMessageProvider();
        }

        private IErrorMessageService systemUnderTest;

        [Test]
        public void GetErrorMessage_WhenBitcoinAddressExceedsMaxSize_ReturnsBitcoinAddressExceedsMaxSizeMessage()
        {
            var failedUserData = new FailedUserData(0, RejectionReason.BitcoinAddressExceedsMaxSize,
                new UserData(0, "name", 0, 0, 0) { BitcoinAddress = "address" });
            string actual = systemUnderTest.GetErrorMessage(failedUserData);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem with the user's data. The user 'name' has a bitcoin address length of '7' and exceeded the max bitcoin address length. The user should shorten their bitcoin address. You should contact your technical advisor to review the logs and rejected users."));
        }

        [Test]
        public void
            GetErrorMessage_WhenDatabaseUnavailableDuringFileDownload_ReturnsFileDownloadDatabaseUnavailableMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.DatabaseUnavailable, new FilePayload(),
                StatsDownloadService.FileDownload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem downloading the file payload. There was a problem connecting to the database. The database is unavailable, ensure the database is available and configured correctly and try again."));
        }

        [Test]
        public void
            GetErrorMessage_WhenDatabaseUnavailableDuringStatsUpload_ReturnsStatsUploadDatabaseUnavailableMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.DatabaseUnavailable, new FilePayload(),
                StatsDownloadService.StatsUpload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem uploading the file payload. There was a problem connecting to the database. The database is unavailable, ensure the database is available and configured correctly and try again."));
        }

        [Test]
        public void GetErrorMessage_WhenDataStoreUnavailableDuringFileDownload_ReturnsDataStoreUnavailableMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.DataStoreUnavailable, new FilePayload(),
                StatsDownloadService.FileDownload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem downloading the file payload. There was a problem connecting to the data store. The data store is unavailable, ensure the data store is available and configured correctly and try again."));
        }

        [Test]
        public void GetErrorMessage_WhenFahNameExceedsMaxSize_ReturnsFahNameExceedsMaxSizeMessage()
        {
            var failedUserData = new FailedUserData(0, RejectionReason.FahNameExceedsMaxSize,
                new UserData(0, "user", 0, 0, 0));
            string actual = systemUnderTest.GetErrorMessage(failedUserData);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem with the user's data. The user 'user' has a FAH name length of '4' and exceeded the max FAH name length. The user should shorten their FAH name. You should contact your technical advisor to review the logs and rejected users."));
        }

        [Test]
        public void GetErrorMessage_WhenFahNameExceedsMaxSize_TruncatesNameInMessaging()
        {
            var failedUserData = new FailedUserData(0, RejectionReason.FahNameExceedsMaxSize,
                new UserData(0, new string(' ', 200), 0, 0, 0));
            string actual = systemUnderTest.GetErrorMessage(failedUserData);

            Assert.That(actual,
                Is.EqualTo(
                    $"There was a problem with the user's data. The user '{new string(' ', 175)}' has a FAH name length of '200' and exceeded the max FAH name length. The user should shorten their FAH name. You should contact your technical advisor to review the logs and rejected users."));
        }

        [Test]
        public void GetErrorMessage_WhenFailedAddToDatabase_ReturnsFailedAddToDatabaseMessage()
        {
            var failedUserData =
                new FailedUserData(0, RejectionReason.FailedAddToDatabase, new UserData(0, "user", 0, 0, 0));

            string actual = systemUnderTest.GetErrorMessage(failedUserData);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem adding the user 'user' to the database. Contact your technical advisor to review the logs and rejected users."));
        }

        [Test]
        public void GetErrorMessage_WhenFailedParsing_ReturnsFailedParsingMessage()
        {
            var failedUserData = new FailedUserData(0, "userdata", RejectionReason.FailedParsing);
            string actual = systemUnderTest.GetErrorMessage(failedUserData);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem parsing a user from the stats file. The user 'userdata' failed data parsing. You should contact your technical advisor to review the logs and rejected users."));
        }

        [Test]
        public void GetErrorMessage_WhenFailedUsersData_ReturnsFailedUserDataMessage()
        {
            var failedUsersData = new List<FailedUserData>
                                  {
                                      new FailedUserData(0, "userdata1", RejectionReason.FailedParsing),
                                      new FailedUserData(0, "userdata2", RejectionReason.FailedParsing)
                                  };

            string actual = systemUnderTest.GetErrorMessage(failedUsersData);

            Assert.That(actual,
                Is.EqualTo(
                    $"There was a problem uploading the file payload. The file passed validation but {failedUsersData.Count} lines failed; processing continued after encountering these lines. If this problem occurs again, then you should contact your technical advisor to review the logs and failed users."
                    + $"{Environment.NewLine}{Environment.NewLine}" + $"Top 10 Failed Users:{Environment.NewLine}"
                    + $"There was a problem parsing a user from the stats file. The user 'userdata1' failed data parsing. You should contact your technical advisor to review the logs and rejected users.{Environment.NewLine}"
                    + "There was a problem parsing a user from the stats file. The user 'userdata2' failed data parsing. You should contact your technical advisor to review the logs and rejected users."));
        }

        [Test]
        public void GetErrorMessage_WhenFailedUsersData_ReturnsTopTenFailedUserDataMessages()
        {
            var failedUsersData = new List<FailedUserData>
                                  {
                                      new FailedUserData(0, "userdata1", RejectionReason.FailedParsing),
                                      new FailedUserData(0, "userdata2", RejectionReason.FailedAddToDatabase,
                                          new UserData(0, "userdata2", 0, 0, 0)),
                                      new FailedUserData(0, "userdata3", RejectionReason.FailedParsing),
                                      new FailedUserData(0, "userdata4", RejectionReason.FailedParsing),
                                      new FailedUserData(0, "userdata5", RejectionReason.FailedParsing),
                                      new FailedUserData(0, "userdata6", RejectionReason.FailedParsing),
                                      new FailedUserData(0, "userdata7", RejectionReason.FailedParsing),
                                      new FailedUserData(0, "userdata8", RejectionReason.FailedParsing),
                                      new FailedUserData(0, "userdata9", RejectionReason.FailedParsing),
                                      new FailedUserData(0, "userdata10", RejectionReason.FailedParsing),
                                      new FailedUserData(0, "userdata11", RejectionReason.FailedParsing)
                                  };

            string actual = systemUnderTest.GetErrorMessage(failedUsersData);

            Assert.That(actual,
                Is.EqualTo(
                    $"There was a problem uploading the file payload. The file passed validation but {failedUsersData.Count} lines failed; processing continued after encountering these lines. If this problem occurs again, then you should contact your technical advisor to review the logs and failed users."
                    + $"{Environment.NewLine}{Environment.NewLine}" + $"Top 10 Failed Users:{Environment.NewLine}"
                    + $"There was a problem parsing a user from the stats file. The user 'userdata1' failed data parsing. You should contact your technical advisor to review the logs and rejected users.{Environment.NewLine}"
                    + $"There was a problem adding the user 'userdata2' to the database. Contact your technical advisor to review the logs and rejected users.{Environment.NewLine}"
                    + $"There was a problem parsing a user from the stats file. The user 'userdata3' failed data parsing. You should contact your technical advisor to review the logs and rejected users.{Environment.NewLine}"
                    + $"There was a problem parsing a user from the stats file. The user 'userdata4' failed data parsing. You should contact your technical advisor to review the logs and rejected users.{Environment.NewLine}"
                    + $"There was a problem parsing a user from the stats file. The user 'userdata5' failed data parsing. You should contact your technical advisor to review the logs and rejected users.{Environment.NewLine}"
                    + $"There was a problem parsing a user from the stats file. The user 'userdata6' failed data parsing. You should contact your technical advisor to review the logs and rejected users.{Environment.NewLine}"
                    + $"There was a problem parsing a user from the stats file. The user 'userdata7' failed data parsing. You should contact your technical advisor to review the logs and rejected users.{Environment.NewLine}"
                    + $"There was a problem parsing a user from the stats file. The user 'userdata8' failed data parsing. You should contact your technical advisor to review the logs and rejected users.{Environment.NewLine}"
                    + $"There was a problem parsing a user from the stats file. The user 'userdata9' failed data parsing. You should contact your technical advisor to review the logs and rejected users.{Environment.NewLine}"
                    + "There was a problem parsing a user from the stats file. The user 'userdata10' failed data parsing. You should contact your technical advisor to review the logs and rejected users."));
        }

        [Test]
        public void GetErrorMessage_WhenFileDownloadFailedDecompression_ReturnsFileDownloadFailedDecompressionMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.FileDownloadFailedDecompression,
                new FilePayload(), StatsDownloadService.FileDownload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem decompressing the file payload. The file has been moved to a failed directory for review. If this problem occurs again, then you should contact your technical advisor to review the logs and failed files."));
        }

        [Test]
        public void GetErrorMessage_WhenFileDownloadNotFound_ReturnsFileDownloadNotFoundMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.FileDownloadNotFound, new FilePayload(),
                StatsDownloadService.FileDownload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem downloading the file payload. The file payload could not be found. Check the download URI configuration and try again. If this problem occurs again, then you should contact your technical advisor to review the logs."));
        }

        [Test]
        public void GetErrorMessage_WhenFileDownloadTimeout_ReturnsFileDownloadTimeoutMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.FileDownloadTimeout, new FilePayload(),
                StatsDownloadService.FileDownload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem downloading the file payload. There was a timeout when downloading the file payload. If a timeout occurs again, then you can try increasing the configurable download timeout."));
        }

        [Test]
        public void GetErrorMessage_WhenFriendlyNameExceedsMaxSize_ReturnsFriendlyNameExceedsMaxSizeMessage()
        {
            var failedUserData = new FailedUserData(0, RejectionReason.FriendlyNameExceedsMaxSize,
                new UserData(0, "name", 0, 0, 0) { FriendlyName = "friendly" });
            string actual = systemUnderTest.GetErrorMessage(failedUserData);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem with the user's data. The user 'name' has a friendly name length of '8' and exceeded the max friendly name length. The user should shorten their friendly name. You should contact your technical advisor to review the logs and rejected users."));
        }

        [Test]
        public void GetErrorMessage_WhenInvalidStatsFile_ReturnsInvalidStatsFileMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.InvalidStatsFileUpload,
                StatsDownloadService.StatsUpload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem uploading the file payload. The file failed validation; check the logs for more information. If this problem occurs again, then you should contact your technical advisor to review the logs and failed uploads."));
        }

        [Test]
        public void GetErrorMessage_WhenMinimumWaitTimeNotMet_ReturnsMinimumWaitTimeNotMetMessage()
        {
            var configuredWaitTime = new TimeSpan(1, 0, 0, 0);

            string actual = systemUnderTest.GetErrorMessage(FailedReason.MinimumWaitTimeNotMet,
                new FilePayload { MinimumWaitTimeSpan = configuredWaitTime }, StatsDownloadService.FileDownload);

            Assert.That(actual,
                Is.EqualTo(
                    $"There was a problem downloading the file payload. The file download service was run before the minimum wait time {MinimumWait.TimeSpan} or the configured wait time {configuredWaitTime}. Configure to run the service less often or decrease your configured wait time and try again."));
        }

        [Test]
        public void
            GetErrorMessage_WhenMissingRequiredObjectsDuringFileDownload_ReturnsFileDownloadMissingRequiredObjectsMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.DatabaseMissingRequiredObjects,
                StatsDownloadService.FileDownload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem downloading the file payload. The database is missing required objects. Add the missing database objects and try again. You should contact your technical advisor to review the logs."));
        }

        [Test]
        public void
            GetErrorMessage_WhenMissingRequiredObjectsDuringStatsUpload_ReturnsStatsUploadMissingRequiredObjectsMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.DatabaseMissingRequiredObjects,
                StatsDownloadService.StatsUpload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem uploading the file payload. The database is missing required objects. Add the missing database objects and try again. You should contact your technical advisor to review the logs."));
        }

        [Test]
        public void GetErrorMessage_WhenNoFailedReason_ReturnsEmptyMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.None, new FilePayload(),
                StatsDownloadService.FileDownload);

            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void GetErrorMessage_WhenRequiredSettingsInvalid_ReturnsRequiredSettingsInvalidMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.RequiredSettingsInvalid, new FilePayload(),
                StatsDownloadService.FileDownload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem downloading the file payload. The required settings are invalid; check the logs for more information. Ensure the settings are complete and accurate, then try again."));
        }

        [Test]
        public void GetErrorMessage_WhenStatsUploadTimeout_ReturnsStatsUploadTimeoutMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.UnexpectedDatabaseException,
                StatsDownloadService.StatsUpload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem uploading the file payload. There was an unexpected database exception and the file has been marked rejected. If this problem occurs again, then you should contact your technical advisor to review the rejections and logs."));
        }

        [Test]
        public void
            GetErrorMessage_WhenUnexpectedExceptionDuringFileDownload_ReturnsFileDownloadUnexpectedExceptionMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.UnexpectedException, new FilePayload(),
                StatsDownloadService.FileDownload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem downloading the file payload. There was an unexpected exception. Check the log for more information."));
        }

        [Test]
        public void
            GetErrorMessage_WhenUnexpectedExceptionDuringStatsUpload_ReturnsStatsUploadUnexpectedExceptionMessage()
        {
            string actual =
                systemUnderTest.GetErrorMessage(FailedReason.UnexpectedException, StatsDownloadService.StatsUpload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem uploading the file payload. There was an unexpected exception. Check the log for more information."));
        }

        [Test]
        public void GetErrorMessage_WhenUnexpectedFormat_ReturnsUnexpectedFormatMessage()
        {
            var failedUserData = new FailedUserData(0, "userdata", RejectionReason.UnexpectedFormat);
            string actual = systemUnderTest.GetErrorMessage(failedUserData);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem parsing a user from the stats file. The user 'userdata' was in an unexpected format. You should contact your technical advisor to review the logs and rejected users."));
        }

        [Test]
        public void GetErrorMessage_WhenUnexpectedStatsDownloadServiceEnumValueProvided_UsesDefaultMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.UnexpectedException,
                (StatsDownloadService)Enum.Parse(typeof (StatsDownloadService), "-1"));

            Assert.That(actual, Is.EqualTo("There was an unexpected exception. Check the log for more information."));
        }

        [Test]
        public void
            GetErrorMessage_WhenUnexpectedValidationExceptionDuringFileDownload_ReturnsUnexpectedValidationExceptionMessage()
        {
            string actual = systemUnderTest.GetErrorMessage(FailedReason.UnexpectedValidationException,
                new FilePayload(), StatsDownloadService.FileDownload);

            Assert.That(actual,
                Is.EqualTo(
                    "There was a problem validating the file. There was an unexpected exception while validating. Check the log for more information."));
        }

        [Test]
        public void GetErrorMessage_WhenUnknownRejectionReason_ReturnsEmptyString()
        {
            string actual = systemUnderTest.GetErrorMessage(new FailedUserData(0, null,
                (RejectionReason)Enum.Parse(typeof (RejectionReason), "-1")));

            Assert.That(actual, Is.Empty);
        }
    }
}