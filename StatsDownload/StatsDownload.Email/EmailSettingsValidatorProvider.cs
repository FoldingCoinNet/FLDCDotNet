﻿namespace StatsDownload.Email
{
    using System;
    using System.Collections.Generic;

    public class EmailSettingsValidatorProvider : IEmailSettingsValidatorService
    {
        public string ParseFromAddress(string unsafeFromAddress)
        {
            throw new NotImplementedException();
        }

        public string ParseFromDisplayName(string unsafeFromDisplayName)
        {
            throw new NotImplementedException();
        }

        public string ParsePassword(string unsafePassword)
        {
            throw new NotImplementedException();
        }

        public int ParsePort(string unsafePort)
        {
            int port;
            if (!int.TryParse(unsafePort, out port))
            {
                throw new EmailArgumentException(nameof(unsafePort), "An integer was not provided");
            }

            if (port < 1 || port > 65535)
            {
                throw new EmailArgumentException(nameof(unsafePort), "The port should be between 1 and 65535, inclusive");
            }

            return port;
        }

        public IEnumerable<string> ParseReceivers(string unsafeReceivers)
        {
            if (string.IsNullOrWhiteSpace(unsafeReceivers))
            {
                throw new EmailArgumentException(nameof(unsafeReceivers), "A receivers list was not provided");
            }

            return unsafeReceivers.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string ParseSmtpHost(string unsafeSmtpHost)
        {
            throw new NotImplementedException();
        }
    }
}