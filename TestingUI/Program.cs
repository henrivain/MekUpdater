using MekPathLibrary;
using MekUpdater.GithubClient;
using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.FileManagers;
using MekUpdater.Helpers;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger<GithubApiClient>();
FolderPath path = new(@$"C:\Users\henri\Downloads\api_downloads\");
ZipPath zipPath = new(@$"C:\Users\henri\Downloads\api_downloads\release_v3.1.9.zip");

// Matikkaeditorinkaantaja
// Wordlists
//using GithubDownloadClient client = new("henrivain", "Matikkaeditorinkaantaja", logger);

string owner = "henrivain";
string repo = "Matikkaeditorinkaantaja";
VersionTag minVersion = new("1.3.7");


logger.LogInformation("Get release info.");
var infoClient = new GithubInfoClient(owner, repo, logger);
var infoResult = await infoClient.GetLatestRelease();
if (infoResult.NotSuccess())
{
    logger.LogError("Failed to check for updates.");
    return 1; 
}
if (infoResult.Content?.TagName is null)
{
    logger.LogError("Latest version is not defined.");
    return 1;
}
if (new VersionTag(infoResult.Content.TagName) <= minVersion)
{
    logger.LogInformation("Update already installed.");
    return 0;
}

logger.LogInformation("Download update.");
var downloadClient = new GithubDownloadClient(owner, repo, logger);
var downloadResult = await downloadClient.DownloadReleaseZip(new(infoResult.Content.TagName), path);
if (downloadResult.NotSuccess())
{
    logger.LogError("Failed to download file.");
    return 1;
}
if (downloadResult.ResultPath?.PathExist is not true)
{
    logger.LogError("Download path is null.");
    return 1;
}

logger.LogInformation("Extract file.");
var fileExtracter = new FileExtracter(downloadResult.ResultPath, logger);
var extractionResult = await fileExtracter.ExtractAsync();
if (extractionResult.NotSuccess())
{
    logger.LogError("Failed to extract zip file.");
    return 1;
}
if (extractionResult?.ResultPath?.PathExist is not true)
{
    logger.LogError("Setup file is not defined.");
    return 1;
}

logger.LogInformation("Start process");
var setupLauncher = new SetupLauncher(extractionResult.ResultPath, logger);
var setupLaunchResult = setupLauncher.LaunchSetup();
if (setupLaunchResult.NotSuccess())
{
    logger.LogError("Setup launch not successful");
    return 1;
}

return 0;
