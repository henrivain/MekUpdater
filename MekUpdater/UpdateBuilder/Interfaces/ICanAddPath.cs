namespace MekPathLibraryTests.UpdateBuilder.Interfaces;

public interface ICanAddPath
{
    ICanAddPath Where(ZipPath zipPath);
    ICanAddPath Where(FolderPath setupDestinationFolder);
    ICanRunUpdate RunUpdate();
}
