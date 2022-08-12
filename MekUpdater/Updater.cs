/// Copyright 2022 Henri Vainio 

namespace MekUpdater;

/// <summary>
/// Check and run updates on your application
/// </summary>
public class AppUpdater
{
    /// <summary>
    /// Initialize new AppUpdater with information about github repository author name and repository name
    /// <para/>Pass parameters in format AppUpdater("MyLoginName", "MyCoolRepository") 
    /// <para/>Local paths will be handled automatically to appData/appName/temp
    /// THESE NAMES MUST BE RIGHT FOR UPDATER TO WORK
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if any of the arguments is null or empty</exception>
    public AppUpdater(string repoOwner, string repoName)
    {
        ZipPath zipPath = new(
                Path.Combine(Helper.UserTempFolder.ToString(), $"{repoName}\\update.zip")
                );

        Info = new()
        {
            ZipFilePath = zipPath,
            ExtractPath = zipPath.FolderPath,
            RepoInfo = new()
            {
                RepoOwner = repoOwner,
                RepoName = repoName
            }
        };
    }

    /// <summary>
    /// Initialize new AppUpdater with information about github repository author name and repository name and
    /// where to place downloaded zip file. Unknown properties will be added automatically
    /// <para/>Pass parameters in format AppUpdater("MyLoginName", "MyCoolRepository", "C:path/to/file.zip")
    /// <para/>Extraction path will be routed to folder containing downloaded zip file
    /// THESE NAMES MUST BE RIGHT FOR UPDATER TO WORK
    /// </summary>
    /// <exception cref="ArgumentException">Will be thrown if zipPath 
    /// does not have value or any of other arg does not have value</exception>
    public AppUpdater(string repoOwner, string repoName, ZipPath zipPath)
    {
        if (zipPath.HasValue is false) throw new ArgumentException($"{nameof(zipPath)}.FullPath can't be empty");

        Info = new()
        {
            ZipFilePath = zipPath,
            ExtractPath = zipPath.FolderPath,
            RepoInfo = new()
            {
                RepoOwner = repoOwner,
                RepoName = repoName
            }
        };
    }

    /// <summary>
    /// Initialize new AppUpdater with information about github repository 
    /// author name and repository name and where to place downloaded zip file 
    /// and extracted files.
    /// <para/>Pass parameters in format AppUpdater("MyLoginName", "MyCoolRepository", "C:path/to/file.zip", "C:path/to") 
    /// REPOSITORY NAMES MUST BE RIGHT FOR UPDATER TO WORK
    /// </summary>
    /// <exception cref="ArgumentException">Will be thrown if zipPath or extractionPath does
    /// not have value or any of other arg does not have value</exception>
    public AppUpdater(string repoOwner, string repoName, ZipPath zipPath, FolderPath extractionPath)
    {
        if (zipPath.HasValue is false) throw new ArgumentException($"{nameof(zipPath)}.FullPath can't be empty");
        if (extractionPath.HasValue is false) throw new ArgumentException($"{nameof(extractionPath)}.FullPath can't be empty");

        Info = new()
        {
            ZipFilePath = zipPath,
            ExtractPath = extractionPath,
            RepoInfo = new()
            {
                RepoOwner = repoOwner,
                RepoName = repoName
            }
        };
    }


    private UpdateChecker? UpdateChecker { get; set; } = null;
    private UpdateDownloadInfo Info { get; set; }
    private void LogStatus(int milliSecondsDelay = 1000)
    {
        string loggerName = "[UpdateStatus]";
        Console.WriteLine($"{loggerName} Start Update");
        Task.Run(async () =>
        {
            while (true)
            {
                if (Info?.IsUpdating is false) return;
                await Task.Delay(milliSecondsDelay);
                if (Info?.Status is null)
                {
                    Console.WriteLine($"{loggerName} UpdateStatusInfo not initialized");
                    continue;
                }
                Console.WriteLine($"{loggerName} {Info?.Status}");
            }
        });
        Console.WriteLine($"{loggerName} End Update");
    }



    /// <summary>
    /// Get update checker and information about repository with it
    /// </summary>
    /// <returns>async Task of UpdateChecker</returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataParseException"></exception>
    public async Task<UpdateChecker> CheckForUpdates()
    {
        UpdateChecker ??= new(Info.RepoInfo.RepoOwner, Info.RepoInfo.RepoName);
        return await UpdateChecker.CheckForUpdates();
    }

    /// <summary>
    /// Download update files from Github, extract and launch setup.exe
    /// <para/>call only if CheckForUpdates() is run before
    /// </summary>
    /// <returns>awaitable Task</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task DownloadUpdateAsync()
    {
        try
        {
            Info.RepoInfo.DownloadUrl = Validator.IsDownloadUrlValid(UpdateChecker?.DownloadLink);
        }
        catch (ArgumentException)
        {
            throw new InvalidOperationException(AppError.Text(
                $"{nameof(CheckForUpdates)}() must be ran before calling this method"));
        }
        await RunUpdate();
    }

    /// <summary>
    /// Download update files from Github, extract and launch setup.exe
    /// <para/>pass url in format "https://api.github.com/repos/{author}/{repo}/zipball/{version}"
    /// </summary>
    /// <param name="downloadUrl"></param>
    /// <returns>awaitable Task</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task DownloadUpdateAsync(string downloadUrl)
    {
        Info.RepoInfo.DownloadUrl = Validator.IsDownloadUrlValid(downloadUrl);
        await RunUpdate();
    }
    
    private async Task RunUpdate()
    {
        Info.IsUpdating = true;
        LogStatus();
        UpdateRouter downloader = new(Info);
        await downloader.DownloadAsync();
        await downloader.ExtractZipAsync();
        downloader.StartSetup();
        Info.IsUpdating = false;

        if (Info.Status is UpdateDownloadInfo.CompletionStatus.LaunchingCompleted) Info.Completed();
        if (Info.Status is UpdateDownloadInfo.CompletionStatus.LaunchingFailed) Info.Failed();
    }
}
