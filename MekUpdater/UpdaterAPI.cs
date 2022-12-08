using MekUpdater.GithubClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MekUpdater;

/// <summary>
/// Main API of MekUdpater
/// </summary>
public class UpdaterAPI : IDisposable
{

    /// <summary>
    /// New UpdaterAPI with no logging
    /// </summary>
    /// <param name="githubUsername">Github username of the repository owner.</param>
    /// <param name="repositoryName">Name of the github repository.</param>
    public UpdaterAPI(string githubUsername, string repositoryName) 
        : this(githubUsername, repositoryName, NullLogger.Instance) { }

    /// <summary>
    /// New UpdaterAPI with logging
    /// </summary>
    /// <param name="githubUsername">Github username of the repository owner.</param>
    /// <param name="repositoryName">Name of the github repository.</param>
    /// <param name="logger">Logger to be used.</param>
    public UpdaterAPI(string githubUsername, string repositoryName, ILogger logger)
    {
        DownloadClient = new GithubDownloadClient(githubUsername, repositoryName);
        Logger = logger;
    }


    private IGithubDownloadClient DownloadClient { get; }
    private IGithubInfoClient InfoClient => DownloadClient.InfoClient;
    private ILogger Logger { get; }



    












    /// <inheritdoc/>
    public void Dispose()
    {
        DownloadClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}
