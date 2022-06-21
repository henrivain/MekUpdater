/// Copyright 2021 Henri Vainio 

using MekUpdater.Exceptions;
using MekUpdater.Helpers;
using MekUpdater.InstallUpdates;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MekUpdater
{
    /// <summary>
    /// Class to handle update process from github api
    /// <para/>To achive best possible outcome you should: new() => DownloadAsync() => ExtractZipAsync()
    /// </summary>
    public class UpdateRouter
    {
        /// <summary>
        /// Handles Update downloading and zip extraction with Download() and Extract
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <param name="repoOwner"></param>
        /// <param name="repoName"></param>
        /// <param name="zipPath"></param>
        /// <param name="extractionPath"></param>
        /// <exception cref="ArgumentException"></exception>
        public UpdateRouter(
            string downloadUrl,
            string repoOwner,
            string repoName,
            string? zipPath = null,
            string? extractionPath = null
            )
        {
            zipPath ??= FilePathHelper.GetAppDataTempZipPath(repoName);
            extractionPath ??= FilePathHelper.RemoveFileName(zipPath);
            downloadUrl = Validator.IsDownloadUrlValid(downloadUrl);
            zipPath = Validator.GetCorrectWindowsPath(zipPath);
            extractionPath = Validator.GetCorrectFolderPath(extractionPath);
          
            if (zipPath.Contains(".zip") is false) Path.Combine(zipPath, "update.zip");

            Info = new UpdateDownloadInfo()
            {
                ZipFilePath = downloadUrl,
                ExtractPath = extractionPath,
                RepoInfo = new()
                {
                    DownloadUrl = downloadUrl,
                    RepoOwner = repoOwner,
                    RepoName = repoName
                }
            };
        }

        /// <summary>
        /// Handles Update downloading and zip extraction with Download() and Extract
        /// </summary>
        public UpdateRouter(UpdateDownloadInfo info)
        {
            Info = info;
        }



        /// <summary>
        /// Contains all the information about current running update process
        /// </summary>
        public UpdateDownloadInfo Info { get; }

        /// <summary>
        /// Download .zip file from github and copy it to given file path async
        /// </summary>
        /// <returns>Task of UpdateDownloadInfo</returns>
        /// <exception cref="UpdateDownloadFailedException"></exception>
        public async Task<UpdateRouter> DownloadAsync()
        {
            GithubZipDownloader downloader = new(Info);
            await downloader.DownloadAsync();

            if (Info.Status is UpdateDownloadInfo.CompletionStatus.DownloadFailed)
            {
                throw new UpdateDownloadFailedException(
                    AppError.Text(
                        $"Update failed ; reason {nameof(Info.Error.msg)}"
                        ));
            }
            return this;
        }

        /// <summary>
        /// Extract downloaded zip file to folder defined in (UpdateDownloadInfo) Info.ExtractionPath
        /// </summary>
        /// <param name="forceExtract"></param>
        /// <returns>async Task and UpdateDownloadInfo with all current info about update</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ZipExtractionException"></exception>
        public async Task<UpdateRouter> ExtractZipAsync(bool forceExtract = false)
        {
            ZipExtracter unZipper = new(Info);
            
            // Validate process
            bool downloadCompleted = Info.IsDownloadCompletedSuccesfully();
            if (downloadCompleted is false && forceExtract is false)
            {
                throw new InvalidOperationException(AppError.Text(
                    "Can't extract zip because download has not been completed succefully " +
                    "and process is not forced"));
            }

            await unZipper.Extract();

            if (Info.Status is UpdateDownloadInfo.CompletionStatus.ExtractionFailed)
            {
                throw new ZipExtractionException(
                    AppError.Text(
                        $"Update failed ; reason {nameof(Info.Error.msg)}"
                        ));
            }
            return this;
        }

        /// <summary>
        /// Start application't setup installer from extractPath/setupFolder/setup.exe
        /// <para/>
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public UpdateRouter StartSetup(UpdateDownloadInfo? info = null)
        {
            SetupLauncher launcher = new(info ?? Info);
            launcher.StartSetup();
            return this;
        }
    }
}
