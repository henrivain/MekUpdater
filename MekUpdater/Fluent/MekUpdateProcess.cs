/// Copyright 2021 Henri Vainio 
using MekPathLibrary;
using MekPathLibraryTests.Exceptions;
using MekPathLibraryTests.Fluent.Interfaces;
using MekPathLibraryTests.Helpers;

namespace MekPathLibraryTests.Fluent
{
    public partial class MekUpdateProcess : ICanCheckUpdates, ICanDownloadUpdate, ICanStartSetup, ICanFinishUpdate
    {
        private MekUpdateProcess(string repoOwner, string repoName)
        {
            ZipPath zipPath = new(
                Path.Combine(Helper.DownloadsFolder.ToString(), $"temp\\{repoName}\\update.zip"));
            Console.WriteLine(zipPath);
            
            Info = new()
            {
                IsUpdating = true,
                ZipFilePath = zipPath,
                ExtractPath = zipPath.FolderPath,
                RepoInfo = new()
                {
                    RepoOwner = repoOwner,
                    RepoName = repoName
                }
            };
            Info.Waiting();
        }

        public UpdateDownloadInfo Info { get; private set; }

        public VersionTag OpenVersion = new("v0.0.0");

        private UpdateRouter? Router;

        bool _useLogging = false;
        bool _uninstallOld = false;

        
        
        // STEP 1: Initialice new process


        /// <summary>
        /// Initialize new update process with (repository needs to be public)
        /// <para/>repoOwner = github username of the repository's owner
        /// <para/>repoName = name of the github, where update will be checked
        /// </summary>
        /// <param name="repoOwner"></param>
        /// <param name="repoName"></param>
        /// <returns>new MekUpdateProcess</returns>
        public static ICanCheckUpdates New(string repoOwner, string repoName)
        {
            return new MekUpdateProcess(repoOwner, repoName);
        }

        
        
        // STEP 2 Logging or not (needs to be improved with ILogger later)
        // TODO: Or Choose if old version should be removed before installation process


        public ICanCheckUpdates UseLogging(int milliSecondsDelay = 1000)
        {
            if (_useLogging) return this;   // lock that method will activate only once
            _useLogging = true;

            LogStatus(milliSecondsDelay);
            return this;
        }

        /// <summary>
        /// TODO: remove old installation or maybe do it after update ??
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ICanCheckUpdates UnInstallOldVersion()
        {
            if (_uninstallOld) return this;     // lock that method will activate only once
            _uninstallOld = true;

            throw new NotImplementedException();
        }



        // STEP 3: choose which way to set download url and version open for download


        /// <summary>
        /// Check for updates and set data 
        /// </summary>
        /// <returns>Task of current object</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="DataParseException"></exception>
        public async Task<ICanDownloadUpdate> CheckUpdates()
        {
            UpdateChecker checker = new(Info.RepoInfo.RepoOwner, Info.RepoInfo.RepoName);
            await checker.CheckForUpdates();
            SetCheckerData(checker);
            return this;
        }

        /// <summary>
        /// Set UpdateChecker values using already completed check. 
        /// Checker must be in IsSuccess (= true) state and have DownloadUrl 
        /// and VersionString initialized
        /// </summary>
        /// <param name="checker"></param>
        /// <returns>current object</returns>
        public ICanDownloadUpdate UseChecker(UpdateChecker checker)
        {
            SetCheckerData(checker);
            return this;
        }

        /// <summary>
        /// Make Update using given url zipball download link. 
        /// Open version is for later checking if download is necessary
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <param name="openVersion"></param>
        /// <returns>current object</returns>
        /// <exception cref="ArgumentException">Throws if downloadUrl is bad</exception>
        public ICanDownloadUpdate UseUrl(string downloadUrl, VersionTag openVersion)
        {
            Info.RepoInfo.DownloadUrl = downloadUrl;
            OpenVersion = openVersion;
            return this;
        }


        
        
        // STEP 4: choose if versio needs to be checked before update
        // (If you choose DownloadAnyway(), update will be downloaded also in case, you already have update.)


        /// <summary>
        /// Force update download also in case version is same or smaller 
        /// than current installed version
        /// </summary>
        /// <returns>task of current object</returns>
        /// <exception cref="UpdateDownloadFailedException">if download fails for some reason</exception>
        /// <exception cref="InvalidOperationException">If download was failed can't extract zip</exception>
        /// <exception cref="ZipExtractionException">If zip extraction fails</exception>
        public async Task<ICanStartSetup> DownloadAnyway()
        {
            return await DownloadAndExtract();
        }

        /// <summary>
        /// Download zip file and extract it if version with bigger version number is available
        /// </summary>
        /// <returns>task of current object</returns>
        /// <exception cref="InvalidOperationException">If cant get assembly version or if download fails so zip can't be extracted</exception>
        /// <exception cref="UpdateAlreadyInstalledException">If available update is same or older than current installed version</exception>
        /// <exception cref="UpdateDownloadFailedException">if download fails for some reason</exception>
        /// <exception cref="ZipExtractionException">If zip extraction fails</exception>
        public async Task<ICanStartSetup> DownloadIfNewVersion(VersionTag currentVersion)
        {
            if (currentVersion >= OpenVersion)
            {
                throw new UpdateAlreadyInstalledException("Current version same or newer");
            }
            return await DownloadAndExtract();
        }



        // STEP 5: Launch installer (setup.exe)


        public ICanFinishUpdate StartSetup()
        {
            if (Router is null) throw new InvalidOperationException("Router is null (it shouldn't");
            Router.StartSetup();
            Info.IsUpdating = false;

            if (Info.Status is UpdateDownloadInfo.CompletionStatus.LaunchingCompleted) Info.Completed();
            if (Info.Status is UpdateDownloadInfo.CompletionStatus.LaunchingFailed) Info.Failed();

            return this;
        }


        /// <summary>
        /// Clean directories by removing downloaded files
        /// </summary>
        public void TidyUp()
        {
            throw new NotImplementedException();
        }



    }
}
