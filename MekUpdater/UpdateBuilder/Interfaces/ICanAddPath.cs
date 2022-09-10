using Microsoft.Extensions.Logging;

namespace MekUpdater.UpdateBuilder.Interfaces;

/// <summary>
/// Configure paths and logger used during the update process, part of update builder fluent api
/// </summary>
public interface ICanAddPath
{
    /// <summary>
    /// Path where zip file downloaded from github api will be stored
    /// </summary>
    /// <param name="zipPath"></param>
    /// <returns>calling instance</returns>
    ICanAddPath DownloadZipTo(ZipPath zipPath);

    /// <summary>
    /// Path to folder where files from downloaded zip file will be stored after extraction
    /// </summary>
    /// <param name="setupDestinationFolder"></param>
    /// <returns>calling instance</returns>
    ICanAddPath UsingExtractionFolder(FolderPath setupDestinationFolder);

    /// <summary>
    /// Logger used to log update progression (default is null)
    /// </summary>
    /// <param name="logger"></param>
    /// <returns>calling instance</returns>
    ICanAddPath AddLogger(ILogger logger);

    /// <summary>
    /// Update will be run if conditions defined after this call are met
    /// </summary>
    /// <returns>calling instance</returns>
    ICanRunUpdate RunUpdate();
}
