using Microsoft.Extensions.Logging;

namespace MekUpdater.UpdateBuilder.Interfaces;

public interface ICanAddPath
{
    ICanAddPath Where(ZipPath zipPath);
    ICanAddPath Where(FolderPath setupDestinationFolder);
    ICanAddPath AddLogger(ILogger logger);
    ICanRunUpdate RunUpdate();
}
