﻿namespace StatsDownload.Core
{
    using System;
    using Interfaces.DataTransfer;

    public class FileDownloadMinimumWaitTimeProvider : IFileDownloadMinimumWaitTimeService
    {
        private readonly IFileDownloadDatabaseService fileDownloadDatabaseService;

        public FileDownloadMinimumWaitTimeProvider(IFileDownloadDatabaseService fileDownloadDatabaseService)
        {
            if (fileDownloadDatabaseService == null)
            {
                throw new ArgumentNullException(nameof(fileDownloadDatabaseService));
            }

            this.fileDownloadDatabaseService = fileDownloadDatabaseService;
        }

        public bool IsMinimumWaitTimeMet(FilePayload filePayload)
        {
            TimeSpan configuredMinimumWaitTimeSpan = filePayload.MinimumWaitTimeSpan;
            DateTime lastSuccessfulRun = fileDownloadDatabaseService.GetLastFileDownloadDateTime();
            DateTime dateTimeNow = DateTime.Now;

            if (dateTimeNow - lastSuccessfulRun < MinimumWait.TimeSpan)
            {
                return false;
            }

            if (dateTimeNow - lastSuccessfulRun < configuredMinimumWaitTimeSpan)
            {
                return false;
            }

            return true;
        }
    }
}