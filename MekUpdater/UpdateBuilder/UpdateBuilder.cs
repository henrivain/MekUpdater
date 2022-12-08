using System.Security.AccessControl;
using MekUpdater.UpdateBuilder.Interfaces;
using Microsoft.Extensions.Logging;

namespace MekUpdater.UpdateBuilder;

/// <summary>
/// Build updates using fluent api
/// </summary>
public class UpdateBuilder : ICanAddPath, ICanRunUpdate, ICanFinishUpdate, ICanBuild, IStartSetupMode
{
 
    private Update Update { get; }

    private UpdateBuilder(string repoOwner, string repoName)
    {
        Update = new(repoOwner, repoName);
    }


    /// <summary>
    /// Create new update builder
    /// <para/>Repository owner github user name and repository name must be defined
    /// </summary>
    /// <param name="repoOwner"></param>
    /// <param name="repoName"></param>
    /// <returns>new instance of UpdateBuilder</returns>
    /// <exception cref="ArgumentException">throws if any argument is null, empty or whitspace</exception>
    public static ICanAddPath Create(string repoOwner, string repoName)
    {
        if (string.IsNullOrWhiteSpace(repoOwner))
        {
            throw new ArgumentException($"'{nameof(repoOwner)}' cannot be null or whitespace.", nameof(repoOwner));
        }
        if (string.IsNullOrWhiteSpace(repoName))
        {
            throw new ArgumentException($"'{nameof(repoName)}' cannot be null or whitespace.", nameof(repoName));
        }

        return new UpdateBuilder(repoOwner, repoName);
    }

    /// <summary>
    /// ResultPath where zip file downloaded from github api will be stored
    /// </summary>
    /// <param name="zipPath"></param>
    /// <returns>this update builder instance</returns>
    public ICanAddPath DownloadZipTo(ZipPath zipPath)
    {
        Update.ZipPath = zipPath;
        return this;
    }

    /// <summary>
    /// ResultPath to folder where files from downloaded zip file will be stored after extraction
    /// </summary>
    /// <param name="setupDestinationFolder"></param>
    /// <returns>this update builder instance</returns>
    public ICanAddPath UsingExtractionFolder(FolderPath setupDestinationFolder)
    {
        Update.ExtractionFolder = setupDestinationFolder;
        return this;
    }

    /// <summary>
    /// Logger used to log update progression (default is null)
    /// </summary>
    /// <param name="logger"></param>
    /// <returns>this update builder instance</returns>
    public ICanAddPath AddLogger(ILogger logger)
    {
        Update.Logger = new(logger);
        return this;
    }

    /// <summary>
    /// Update will be run if conditions defined after this call are met
    /// </summary>
    /// <returns>this update builder instance</returns>
    public ICanRunUpdate RunUpdate() => this;

    /// <summary>
    /// Add version required from available version.
    /// Available version must be bigger than this version. 
    /// Non preview versions are seen as newer as preview version.
    /// </summary>
    /// <param name="version"></param>
    /// <returns>this update builder instance</returns>
    public ICanRunUpdate IfVersionBiggerThan(VersionTag version)
    {
        Update.CurrentVersion = version;
        return this;
    }

    /// <summary>
    /// Requires available version to NOT be preview
    /// </summary>
    /// <returns>this update builder instance</returns>
    public ICanRunUpdate IfNotPreview()
    {
        Update.CanUpdatePreviewVersion = false;
        return this;
    }

    /// <summary>
    /// Setup.exe will be run if next call in chain is IsTrue(), 
    /// othervise setup will not be started.
    /// </summary>
    /// <returns>this update builder instance</returns>
    public IStartSetupMode StartsSetup()
    {
        return this;
    }

    /// <summary>
    /// Setup.exe will be started after succesful download and extraction process
    /// </summary>
    /// <returns>this update builder instance</returns>
    public ICanFinishUpdate IsTrue()
    {
        Update.StartSetup = true;
        return this;
    }

    /// <summary>
    /// Setup.exe will NOT be started after succesful download and extraction process
    /// </summary>
    /// <returns>this update builder instance</returns>
    public ICanFinishUpdate IsFalse()
    {
        Update.StartSetup = false;
        return this;
    }
    
    /// <inheritdoc/>
    public ICanBuild TidiesUpIsTrue()
    {
        Update.TidyUpWhenFinishing = true;
        return this;
    }

    /// <inheritdoc/>
    public ICanBuild TidiesUpIsFalse()
    {
        Update.TidyUpWhenFinishing = false;
        return this;
    }

    /// <summary>
    /// Build update instance ready for running
    /// </summary>
    /// <returns>this update builder instance</returns>
    public Update Build() => Update;


}
