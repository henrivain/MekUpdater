using MekUpdater.GithubClient;

using GithubRepositoryClient client = new("henrivain", "Wordlists");

var result = await client.GetRepositoryInfo();

Console.WriteLine("Success");
