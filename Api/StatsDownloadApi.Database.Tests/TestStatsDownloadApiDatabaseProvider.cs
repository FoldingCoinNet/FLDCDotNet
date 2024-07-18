﻿namespace StatsDownloadApi.Database.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    using NSubstitute;

    using NUnit.Framework;

    using StatsDownload.Core.Interfaces;
    using StatsDownload.Core.Interfaces.DataTransfer;
    using StatsDownload.Core.Interfaces.Enums;
    using StatsDownload.Database.Tests;

    using StatsDownloadApi.Interfaces;

    using Constants = StatsDownloadApi.Database.Constants;

    [TestFixture]
    public class TestStatsDownloadApiDatabaseProvider
    {
        [SetUp]
        public void SetUp()
        {
            databaseConnectionServiceMock = Substitute.For<IDatabaseConnectionService>();

            statsDownloadDatabaseServiceMock = Substitute.For<IStatsDownloadDatabaseService>();
            statsDownloadDatabaseServiceMock.When(service =>
                service.CreateDatabaseConnectionAndExecuteAction(Arg.Any<Action<IDatabaseConnectionService>>())).Do(
                callInfo =>
                {
                    var service = callInfo.Arg<Action<IDatabaseConnectionService>>();

                    service.Invoke(databaseConnectionServiceMock);
                });

            DatabaseProviderTestingHelper.SetUpDatabaseConnectionServiceReturns(databaseConnectionServiceMock);

            loggerMock = Substitute.For<ILogger<StatsDownloadApiDatabaseProvider>>();

            systemUnderTest = NewStatsDownloadApiDatabaseProvider(loggerMock, statsDownloadDatabaseServiceMock);
        }

        private IDatabaseConnectionService databaseConnectionServiceMock;

        private ILogger<StatsDownloadApiDatabaseProvider> loggerMock;

        private IStatsDownloadDatabaseService statsDownloadDatabaseServiceMock;

        private IStatsDownloadApiDatabaseService systemUnderTest;

        [Test]
        public void Constructor_WhenNullDependencyProvided_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                NewStatsDownloadApiDatabaseProvider(null, statsDownloadDatabaseServiceMock));
            Assert.Throws<ArgumentNullException>(() => NewStatsDownloadApiDatabaseProvider(loggerMock, null));
        }

        [Test]
        public void GetValidatedFiles_WhenInvoked_GetsValidatedFiles()
        {
            databaseConnectionServiceMock.When(service =>
                service.ExecuteStoredProcedure("[FoldingCash].[GetValidatedFiles]", Arg.Any<IEnumerable<DbParameter>>(),
                    Arg.Any<DataTable>())).Do(callInfo =>
            {
                var dataTable = callInfo.Arg<DataTable>();
                dataTable.Columns.Add(new DataColumn("DownloadId", typeof (int)));
                dataTable.Columns.Add(new DataColumn("DownloadDateTime", typeof (DateTime)));
                dataTable.Columns.Add(new DataColumn("FilePath", typeof (string)));
                DataRow user1 = dataTable.NewRow();
                dataTable.Rows.Add(user1);
                user1["DownloadId"] = 1;
                user1["DownloadDateTime"] = DateTime.Today.AddMinutes(1);
                user1["FilePath"] = "FilePath1";
                DataRow user2 = dataTable.NewRow();
                dataTable.Rows.Add(user2);
                user2["DownloadId"] = 2;
                user2["DownloadDateTime"] = DateTime.Today.AddMinutes(2);
                user2["FilePath"] = "FilePath2";
                dataTable.AcceptChanges();
            });

            IList<ValidatedFile> actual = systemUnderTest.GetValidatedFiles(DateTime.MinValue, DateTime.MaxValue);

            Assert.That(actual.Count, Is.EqualTo(2));
            Assert.That(actual[0].DownloadId, Is.EqualTo(1));
            Assert.That(actual[0].DownloadDateTime, Is.EqualTo(DateTime.Today.AddMinutes(1)));
            Assert.That(actual[0].FilePath, Is.EqualTo("FilePath1"));
            Assert.That(actual[1].DownloadId, Is.EqualTo(2));
            Assert.That(actual[1].DownloadDateTime, Is.EqualTo(DateTime.Today.AddMinutes(2)));
            Assert.That(actual[1].FilePath, Is.EqualTo("FilePath2"));
        }

        [Test]
        public void GetValidatedFiles_WhenInvoked_GetValidatedFilesParametersAreProvided()
        {
            IEnumerable<DbParameter> actualParameters = null;

            databaseConnectionServiceMock.When(service =>
                service.ExecuteStoredProcedure("[FoldingCash].[GetValidatedFiles]", Arg.Any<IEnumerable<DbParameter>>(),
                    Arg.Any<DataTable>())).Do(callInfo =>
            {
                actualParameters = callInfo.Arg<IEnumerable<DbParameter>>();
            });

            systemUnderTest.GetValidatedFiles(DateTime.MinValue, DateTime.MaxValue);

            Assert.That(actualParameters.Count, Is.EqualTo(2));
            Assert.That(actualParameters.ElementAt(0).ParameterName, Is.EqualTo("@StartDate"));
            Assert.That(actualParameters.ElementAt(0).DbType, Is.EqualTo(DbType.Date));
            Assert.That(actualParameters.ElementAt(0).Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(actualParameters.ElementAt(0).Value, Is.EqualTo(DateTime.MinValue));
            Assert.That(actualParameters.ElementAt(1).ParameterName, Is.EqualTo("@EndDate"));
            Assert.That(actualParameters.ElementAt(1).DbType, Is.EqualTo(DbType.Date));
            Assert.That(actualParameters.ElementAt(1).Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(actualParameters.ElementAt(1).Value, Is.EqualTo(DateTime.MaxValue));
        }

        [TestCase(true, DatabaseFailedReason.None)]
        [TestCase(false, DatabaseFailedReason.DatabaseMissingRequiredObjects)]
        public void IsAvailable_WhenInvoked_ReturnsIsAvailable(bool expectedIsAvailable,
                                                               DatabaseFailedReason expectedReason)
        {
            statsDownloadDatabaseServiceMock.IsAvailable(Constants.StatsDownloadApiDatabase.ApiObjects)
                                            .Returns((expectedIsAvailable, expectedReason));

            (bool isAvailable, DatabaseFailedReason reason) actual = systemUnderTest.IsAvailable();

            Assert.That(actual.isAvailable, Is.EqualTo(expectedIsAvailable));
            Assert.That(actual.reason, Is.EqualTo(expectedReason));
        }

        private IStatsDownloadApiDatabaseService NewStatsDownloadApiDatabaseProvider(
            ILogger<StatsDownloadApiDatabaseProvider> logger,
            IStatsDownloadDatabaseService statsDownloadDatabaseService)
        {
            return new StatsDownloadApiDatabaseProvider(logger, statsDownloadDatabaseService);
        }
    }
}