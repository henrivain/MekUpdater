using MekUpdater.UpdateRunner;
// Copyright 2021 Henri Vainio 
namespace MekUpdater.InstallUpdates;


/// <summary>
/// Class for downloading zip file from github api and bringing it to computer's file system
/// </summary>
internal class GithubZipDownloader
{
    /// <summary>
    /// Initialize new GithubZipDownloader with url to download from and file path to copy zip to
    /// </summary>
    /// <param name="downloadUrl"></param>
    /// <param name="zipPath"></param>
    /// <exception cref="ArgumentException">thrown if url is not valid or zip path does not have value</exception>
    internal GithubZipDownloader(string downloadUrl, ZipPath zipPath)
    {
        DownloadUrl = UrlValidator.IsDownloadUrlValid(downloadUrl);
        if (zipPath.HasValue is false) throw new ArgumentException($"{nameof(zipPath)} does not have value");
        ZipPath = zipPath;
    }

    string DownloadUrl { get; }
    ZipPath ZipPath { get; }

    /// <summary>
    /// Download zip file from given github api using given url and bring it to given path 
    /// </summary>
    /// <returns>awaitable Task</returns>
    internal async Task<DownloadUpdateFilesResult> DownloadAsync()
    {
        try
        {
            await RunDownloadAsync();
            return new(true);
        }
        catch (Exception ex)
        {
            var msg = GetExceptionReason(ex);
            return new(false)
            {
                UpdateMsg = msg,
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

        using Stream stream = await client.GetStreamAsync(DownloadUrl);

        Directory.CreateDirectory(ZipPath.FolderPath.FullPath);

        using FileStream fileStream = new(ZipPath.FullPath, FileMode.OpenOrCreate);
        await stream.CopyToAsync(fileStream);
    }

    /// <summary>
    /// Gets reason for exception thrown in RunDownloadAsync
    /// </summary>
    /// <param name="ex"></param>
    /// <returns>fitting UpdateMsg or UpdateMsg.Unknown if ex is not defined in switch</returns>
    private static UpdateMsg GetExceptionReason(Exception ex)
    {
        return ex switch
        {
            InvalidOperationException => UpdateMsg.BadUrl,
            HttpRequestException => UpdateMsg.NetworkError,
            TaskCanceledException => UpdateMsg.ServerTimeout,
            NotSupportedException => UpdateMsg.OutsideMachine,
            System.Security.SecurityException => UpdateMsg.NoPermission,
            DirectoryNotFoundException => UpdateMsg.NoPermission,
            PathTooLongException => UpdateMsg.PathTooLong,
            FileNotFoundException => UpdateMsg.FileNotFound,
            UnauthorizedAccessException => UpdateMsg.FileReadOnly,
            IOException => UpdateMsg.PathNotOpen,
            ArgumentException => UpdateMsg.PathNullOrEmpty,
            _ => UpdateMsg.Unknown
        };
    }
}
