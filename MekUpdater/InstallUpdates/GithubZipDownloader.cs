/// Copyright 2021 Henri Vainio 
using MekPathLibraryTests.Exceptions;
using MekPathLibraryTests.Helpers;
using static MekPathLibraryTests.UpdateDownloadInfo;
using MekPathLibrary;

namespace MekPathLibraryTests.InstallUpdates
{

    /// <summary>
    /// Class for downloading zip file from github api and bringing it to computer's file system
    /// </summary>
    internal class GithubZipDownloader
    {
        /// <summary>
        /// Initialize new GithubZipDownloader with url to download from and file path to copy zip to
        /// <para/>File path must include file name with .zip extension or update.zip will be added
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <param name="filePath"></param>
        /// <exception cref="ArgumentException"></exception>
        internal GithubZipDownloader(string downloadUrl, ZipPath zipPath, FolderPath extractPath)
        {
            Info.RepoInfo.DownloadUrl = Validator.IsDownloadUrlValid(downloadUrl);
            Info.ZipFilePath = zipPath;
            Info.ExtractPath = extractPath;
        }

        /// <summary>
        /// Initialize new GithubZipDownloader with ResetDownloadInfo already formed. 
        /// Ref<> will make update Info updateable during process
        /// </summary>
        /// <param name="info"></param>
        internal GithubZipDownloader(UpdateDownloadInfo info)
        {
            Info = info;
        }

        /// <summary>
        /// Information about update progression
        /// </summary>
        internal UpdateDownloadInfo Info = new();

        /// <summary>
        /// Download zip file from given url to github api and bring it to given path 
        /// </summary>
        /// <returns>awaitable Task</returns>
        internal async Task<DownloadUpdateFilesResult> DownloadAsync()
        {
            try
            {
                await RunDownloadAsync();
                Info.DownloadCompleted();
                return new(true);
            }
            catch (Exception ex)
            {
                ErrorMsg msg = GetExceptionReason(ex);
                Info.Error = (FailState.Download, msg);
                Info.DownloadFailed();
                return new(false)
                {
                    ErrorMsg = msg,
                    Message = $"Downloading zip file from github failed, because of {msg}: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Run zip download process from url and copy to given folder 
        /// </summary>
        /// <returns>awaitable Task</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="TaskCanceledException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        private async Task RunDownloadAsync()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "request");
            Info.Downloading();

            using Stream stream = await client.GetStreamAsync(Info.RepoInfo.DownloadUrl);

            CreateFolderInNeed();
            using FileStream fileStream = new(Info.ZipFilePath.FullPath, FileMode.OpenOrCreate);
            Info.Copying();
            await stream.CopyToAsync(fileStream);
        }

        /// <summary>
        /// Copy given stream to folder using path from Info.ZipFilePath
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>awaitable task</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="TaskCanceledException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        private async Task CopyStreamToFolder(Stream stream)
        {
            Info.Copying();
            CreateFolderInNeed();
            await stream.CopyToAsync(GetZipFileStream());
        }

        /// <summary>
        /// Download zip from githup as Stream
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Stream zip</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="TaskCanceledException"></exception>
        private async Task<Stream> DownloadZip(HttpClient client)
        {
            Info.Downloading();
            return await client.GetStreamAsync(Info.RepoInfo.DownloadUrl);
        }

        /// <summary>
        /// Open or create FileStream with ".zip" extension for zip file
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        private FileStream GetZipFileStream()
        {
            return new(Info.ZipFilePath.ToString(), FileMode.OpenOrCreate);
        }

        /// <summary>
        /// Check that directory exist and create if not
        /// </summary>
        private void CreateFolderInNeed()
        {
            string folder = new FolderPath(Info.ZipFilePath.ToString()).FullPath;
            if (folder == string.Empty) return;
            Directory.CreateDirectory(folder);
            Console.WriteLine($"[UpdateStatus] Downloading zip to folder: {folder}");
        }

        /// <summary>
        /// Gets reason for exception thrown in RunDownloadAsync
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>fitting ErrorMsg for ex or ErrorMsg.Other if ex is not defined in switch</returns>
        private static ErrorMsg GetExceptionReason(Exception ex)
        {
            AppError.Text(ex.Message, 3);
            return ex switch
            {
                InvalidOperationException => ErrorMsg.BadUrl,
                HttpRequestException => ErrorMsg.NetworkError,
                TaskCanceledException => ErrorMsg.ServerTimeout,
                NotSupportedException => ErrorMsg.OutsideMachine,
                System.Security.SecurityException => ErrorMsg.NoPermission,
                DirectoryNotFoundException => ErrorMsg.NoPermission,
                PathTooLongException => ErrorMsg.PathTooLong,
                FileNotFoundException => ErrorMsg.FileNotFound,
                UnauthorizedAccessException => ErrorMsg.FileReadOnly,
                IOException => ErrorMsg.PathNotOpen,
                ArgumentException => ErrorMsg.PathNullOrEmpty,
                _ => ErrorMsg.Other
            };
        }
    }
}
