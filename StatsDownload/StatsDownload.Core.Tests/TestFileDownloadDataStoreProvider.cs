﻿namespace StatsDownload.Core.Tests
{
    using System;

    using NSubstitute;

    using NUnit.Framework;

    [TestFixture]
    public class TestFileDownloadDataStoreProvider
    {
        private IDatabaseConnectionServiceFactory databaseConnectionServiceFactoryMock;

        private IDatabaseConnectionService databaseConnectionServiceMock;

        private IDatabaseConnectionSettingsService databaseConnectionSettingsServiceMock;

        private IFileDownloadLoggingService fileDownloadLoggingServiceMock;

        private IFileDownloadDataStoreService systemUnderTest;

        [Test]
        public void Constructor_WhenNullDependencyProvided_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                NewFileDownloadDataStoreProvider(
                    null,
                    databaseConnectionServiceFactoryMock,
                    fileDownloadLoggingServiceMock));
            Assert.Throws<ArgumentNullException>(
                () =>
                NewFileDownloadDataStoreProvider(
                    databaseConnectionSettingsServiceMock,
                    null,
                    fileDownloadLoggingServiceMock));
            Assert.Throws<ArgumentNullException>(
                () =>
                NewFileDownloadDataStoreProvider(
                    databaseConnectionSettingsServiceMock,
                    databaseConnectionServiceFactoryMock,
                    null));
        }

        [Test]
        public void IsAvailable_WhenDatabaseConnectionFails_ConnectionDisposed()
        {
            databaseConnectionServiceMock.When(mock => mock.Open()).Throw<Exception>();

            InvokeIsAvailable();

            Received.InOrder(
                (() =>
                    {
                        databaseConnectionServiceMock.Open();
                        databaseConnectionServiceMock.Dispose();
                    }));
        }

        [Test]
        public void IsAvailable_WhenDatabaseConnectionFails_LoggingIsCalled()
        {
            var expected = new Exception();
            databaseConnectionServiceMock.When(mock => mock.Open()).Throw(expected);

            InvokeIsAvailable();

            Received.InOrder(
                (() =>
                    {
                        fileDownloadLoggingServiceMock.LogVerbose("IsAvailable Invoked");
                        fileDownloadLoggingServiceMock.LogException(expected);
                    }));
        }

        [Test]
        public void IsAvailable_WhenDatabaseConnectionFails_ReturnsFalse()
        {
            databaseConnectionServiceMock.When(mock => mock.Open()).Throw<Exception>();

            bool actual = InvokeIsAvailable();

            Assert.That(actual, Is.False);
        }

        [Test]
        public void IsAvailable_WhenDatabaseConnectionSuccessful_ConnectionClosed()
        {
            InvokeIsAvailable();

            Received.InOrder(
                (() =>
                    {
                        databaseConnectionServiceMock.Open();
                        databaseConnectionServiceMock.Dispose();
                    }));
        }

        [Test]
        public void IsAvailable_WhenDatabaseConnectionSuccessful_LoggingIsCalled()
        {
            InvokeIsAvailable();

            Received.InOrder(
                (() =>
                    {
                        fileDownloadLoggingServiceMock.LogVerbose("IsAvailable Invoked");
                        fileDownloadLoggingServiceMock.LogVerbose("Database connection was successful");
                    }));
        }

        [Test]
        public void IsAvailable_WhenDatabaseConnectionSuccessful_ReturnsTrue()
        {
            bool actual = InvokeIsAvailable();

            Assert.That(actual, Is.True);
        }

        [SetUp]
        public void SetUp()
        {
            databaseConnectionSettingsServiceMock = Substitute.For<IDatabaseConnectionSettingsService>();
            databaseConnectionSettingsServiceMock.GetConnectionString().Returns("connectionString");

            databaseConnectionServiceMock = Substitute.For<IDatabaseConnectionService>();
            databaseConnectionServiceFactoryMock = Substitute.For<IDatabaseConnectionServiceFactory>();
            databaseConnectionServiceFactoryMock.Create("connectionString").Returns(databaseConnectionServiceMock);

            fileDownloadLoggingServiceMock = Substitute.For<IFileDownloadLoggingService>();

            systemUnderTest = NewFileDownloadDataStoreProvider(
                databaseConnectionSettingsServiceMock,
                databaseConnectionServiceFactoryMock,
                fileDownloadLoggingServiceMock);
        }

        [Test]
        public void UpdateToLatest_WhenDatabaseConnectionSuccessful_LoggingIsCalled()
        {
            InvokeUpdateToLatest();

            Received.InOrder(
                (() =>
                    {
                        fileDownloadLoggingServiceMock.LogVerbose("UpdateToLatest Invoked");
                        fileDownloadLoggingServiceMock.LogVerbose("Database connection was successful");
                    }));
        }

        [Test]
        public void UpdateToLatestIsAvailable_WhenDatabaseConnectionSuccessful_ConnectionClosed()
        {
            InvokeUpdateToLatest();

            Received.InOrder(
                (() =>
                    {
                        databaseConnectionServiceMock.Open();
                        databaseConnectionServiceMock.Dispose();
                    }));
        }

        private bool InvokeIsAvailable()
        {
            return systemUnderTest.IsAvailable();
        }

        private void InvokeUpdateToLatest()
        {
            systemUnderTest.UpdateToLatest();
        }

        private IFileDownloadDataStoreService NewFileDownloadDataStoreProvider(
            IDatabaseConnectionSettingsService databaseConnectionSettingsService,
            IDatabaseConnectionServiceFactory databaseConnectionServiceFactory,
            IFileDownloadLoggingService fileDownloadLoggingService)
        {
            return new FileDownloadDataStoreProvider(
                databaseConnectionSettingsService,
                databaseConnectionServiceFactory,
                fileDownloadLoggingService);
        }
    }
}