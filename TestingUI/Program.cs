using MekPathLibrary;
using MekUpdater.GithubClient;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger<GithubApiClient>();
FolderPath path = new(@$"C:\Users\henri\Downloads\api_downloads\");

using GithubDownloadClient client = new("henrivain", "Wordlists", logger);

var result = await client.DownloadAsset(new("v1.2.0"), path, "Wordlists_Android-Signed", false);

Console.WriteLine(result.ResponseMessage);
