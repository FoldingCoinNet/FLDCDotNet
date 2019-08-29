﻿namespace StatsDownload.Core.Implementations
{
    using StatsDownload.Core.Interfaces;
    using StatsDownload.Core.Interfaces.DataTransfer;
    using StatsDownload.Core.Interfaces.Enums;

    public class UncDataStoreProvider : IDataStoreService
    {
        private readonly IDirectoryService directoryService;

        private readonly IFileService fileService;

        private readonly IUncDataStoreSettings settings;

        public UncDataStoreProvider(IUncDataStoreSettings settings, IDirectoryService directoryService,
                                    IFileService fileService)
        {
            this.settings = settings;
            this.directoryService = directoryService;
            this.fileService = fileService;
        }

        public (bool, FailedReason) IsAvailable()
        {
            string uploadDirectory = settings.UploadDirectory.LocalPath;
            bool directoryExists = directoryService.Exists(uploadDirectory);
            return (directoryExists, FailedReason.DataStoreUnavailable);
        }

        public void UploadFile(FilePayload filePayload)
        {
            fileService.CopyFile(filePayload.DownloadFilePath, filePayload.UploadPath);
        }
    }
}