using System.Runtime.InteropServices;
using MekPathLibrary;
using MekUpdater.GithubClient;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger<GithubApiClient>();
FolderPath path = new(@$"C:\Users\henri\Downloads\api_downloads\");

// Matikkaeditorinkaantaja
// Wordlists
using GithubDownloadClient client = new("henrivain", "Matikkaeditorinkaantaja", logger);

var result = await client.DownloadLatestReleaseZip(path);

logger.LogInformation("status: {status}", result.ResponseMessage);
