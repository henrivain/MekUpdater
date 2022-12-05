using MekUpdater.GithubClient;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
var logger = loggerFactory.CreateLogger<GithubRepositoryClient>();


using GithubRepositoryClient client = new("henrivain", "Wordlists", logger);

var result = await client.GetReleases();

Console.WriteLine(result.ResponseMessage);
