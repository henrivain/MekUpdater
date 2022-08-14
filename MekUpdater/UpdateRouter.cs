/// Copyright 2021 Henri Vainio 

using MekUpdater.Check;
using MekUpdater.Exceptions;
using MekUpdater.Helpers;
using MekUpdater.InstallUpdates;

namespace MekUpdater
{
    /// <summary>
    /// Class to handle update process from github api
    /// <para/>To achive best possible outcome you should: new() => DownloadAsync() => ExtractZipAsync()
    /// </summary>
    public class UpdateRouter
    {
        /// <summary>
        /// Handles Update downloading and zip extraction with Download() and ExtractAsync
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <param name="repoOwner"></param>
        /// <param name="repoName"></param>
        /// <param name="zipPath"></param>
        /// <param name="extractionPath"></param>
        /// <exception cref="ArgumentException"></exception>
        public UpdateRouter(RepositoryInfo repoInfo, ZipPath zipPath, FolderPath extractionPath)
        {
            if (zipPath.HasValue is false)
            {
                zipPath = new(
                    Path.Combine(Helper.UserTempFolder.ToString(), "MekUpdater\\update.zip")
                    );
            }

            if (extractionPath.HasValue is false)
            {
                extractionPath = zipPath.FolderPath;
            }

            Info = new UpdateDownloadInfo()
            {
                ZipFilePath = zipPath,
                ExtractPath = extractionPath,
                RepoInfo = repoInfo
            };
        }

        /// <summary>
        /// Handles Update downloading and zip extraction with Download() and ExtractAsync
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
        /// <returns>Task of ResetDownloadInfo</returns>
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
        /// ExtractAsync downloaded zip file to folder defined in (ResetDownloadInfo) Info.ExtractionPath
        /// </summary>
        /// <param name="forceExtract"></param>
        /// <returns>async Task and ResetDownloadInfo with all current info about update</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ZipExtractionException"></exception>
        public async Task<UpdateRouter> ExtractZipAsync(bool forceExtract = false)
        {
            ZipExtracter unZipper = new(Info);

            // Validate process
            var downloadCompleted = Info.IsDownloadCompletedSuccesfully();
            if (downloadCompleted is false && forceExtract is false)
            {
                throw new InvalidOperationException(AppError.Text(
                    "Can't extract zip because download has not been completed succefully " +
                    "and process is not forced"));
            }

            await unZipper.ExtractAsync();

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
