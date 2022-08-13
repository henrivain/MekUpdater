/// Copyright 2021 Henri Vainio 
using MekPathLibraryTests.Fluent.Interfaces;
using MekPathLibraryTests.Exceptions;
using MekPathLibraryTests.Helpers;

namespace MekPathLibraryTests.Fluent
{
    public partial class MekUpdateProcess
    {
        /// <summary>
        /// Sets all data needed for update from update checker
        /// <para/>DownloadUrl, OpenVersion and IsPreRelease
        /// </summary>
        /// <param name="checker"></param>
        /// <exception cref="InvalidOperationException"></exception>
        private void SetCheckerData(UpdateChecker checker)
        {
            if (checker.IsSuccess is false)
                throw new InvalidOperationException($"{checker} failed to get data");

            try
            {
                Info.RepoInfo.DownloadUrl = checker.DownloadLink ??
                    throw new InvalidOperationException($"{nameof(checker.DownloadLink)} was not defined");
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Info wrong format ; was given {checker.DownloadLink}");
            }

            try
            {
                OpenVersion = new VersionTag(checker.VersionString ?? string.Empty);
            }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Given version string is not right format ; was given {checker.VersionString}");
            }

            if (checker.IsPreRelease is true)
                OpenVersion.SetVersionId(VersionTag.SpecialId.Beta);
        }

        /// <summary>
        /// Log Info.Status to console until update ends
        /// </summary>
        /// <param name="milliSecondsDelay"></param>
        private void LogStatus(int milliSecondsDelay = 1000)
        {
            string loggerName = "[UpdateStatus]";
            Console.WriteLine($"{loggerName} Use logging");
            Console.WriteLine($"{loggerName} Start Update");
            Task.Run(async () =>
            {
                while (true)
                {
                    if (Info?.IsUpdating is false) break;
                    await Task.Delay(milliSecondsDelay);
                    if (Info?.Status is null)
                    {
                        Console.WriteLine($"{loggerName} UpdateStatusInfo not initialized");
                        continue;
                    }
                    Console.WriteLine($"{loggerName} {Info?.Status}");
                }
                Console.WriteLine($"{loggerName} End Update");
            });
        }

        /// <summary>
        /// Downloads zip from server and extracts it to folder
        /// </summary>
        /// <returns>Task of current object</returns>
        /// <exception cref="UpdateDownloadFailedException">If download process fails</exception>
        /// <exception cref="InvalidOperationException">If download was not succesful</exception>
        /// <exception cref="ZipExtractionException">If zip extraction process is failed</exception>
        private async Task<ICanStartSetup> DownloadAndExtract()
        {
            Info.Downloading();
            Router = new(Info);
            await Router.DownloadAsync();
            await Router.ExtractZipAsync();
            return this;
        }
    }
}
