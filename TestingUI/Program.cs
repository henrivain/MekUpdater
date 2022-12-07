using MekPathLibrary;
using MekUpdater.GithubClient;
using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.FileManagers;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger<GithubApiClient>();
FolderPath path = new(@$"C:\Users\henri\Downloads\api_downloads\");
ZipPath zipPath = new(@$"C:\Users\henri\Downloads\api_downloads\releasev3.1.9.zip");

// Matikkaeditorinkaantaja
// Wordlists
//using GithubDownloadClient client = new("henrivain", "Matikkaeditorinkaantaja", logger);

//var result = await client.DownloadLatestReleaseZip(path);

var n = Path.Combine("C:\\Users\\henri\\Downloads\\api_downloads\\", "releasev3.1.9");
Console.WriteLine(n);

var extracter = new FileExtracter(zipPath, logger);

var result = await extracter.ExtractAsync();

logger.LogInformation("status: {status}", result.ResponseMessage);
logger.LogInformation("folder: {folder}", result.ResultFolder?.FullPath);
