namespace MekPathLibraryTests.Fluent.Interfaces;

public interface ICanFinishUpdate
{
    void TidyUp();
    UpdateDownloadInfo Info { get; }
}
