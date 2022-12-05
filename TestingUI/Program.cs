using MekUpdater.GithubClient;
using Microsoft.Extensions.Logging;
using MekPathLibrary;




FilePath path = new(@"C:\test\file.txt");
Console.WriteLine(path);

ZipPath zipPath = path.FolderPath.ToFilePath<ZipPath>(path.FileName);
Console.WriteLine(zipPath);




using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger<GithubApiClient>();


using GithubInfoClient client = new("henrivain", "Wordlists", logger);

var result = await client.GetReleases();

Console.WriteLine(result.ResponseMessage);
