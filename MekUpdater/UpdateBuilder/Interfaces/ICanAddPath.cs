using Microsoft.Extensions.Logging;

namespace MekUpdater.UpdateBuilder.Interfaces;

public interface ICanAddPath
{
    ICanAddPath DownloadZipTo(ZipPath zipPath);
    ICanAddPath UsingExtractionFolder(FolderPath setupDestinationFolder);
    ICanAddPath AddLogger(ILogger logger);
    ICanRunUpdate RunUpdate();
}
