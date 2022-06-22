namespace MekUpdater.Fluent.Interfaces;

public interface ICanStartSetup
{
    ICanFinishUpdate StartSetup();

    UpdateDownloadInfo Info { get; }

}
