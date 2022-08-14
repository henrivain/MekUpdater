using MekUpdater.Helpers;
using MekUpdater.UpdateBuilder;
using MekUpdater.UpdateBuilder.Interfaces;

namespace MekPathLibraryTests.UpdateBuilder;

public class UpdateBuilder : ICanAddPath, ICanRunUpdate, ICanFinishUpdate, ICanBuild
{
    private Update Update { get; }

    private UpdateBuilder(string repoOwner, string repoName)
    {
        Update = new(repoOwner, repoName);
    }

    public static ICanAddPath Create(string repoOwner, string repoName)
    {
        return new UpdateBuilder(repoOwner, repoName);
    }

    public ICanAddPath Where(ZipPath zipPath)
    {
        Update.ZipPath = zipPath;
        return this;
    }
    
    public ICanAddPath Where(FolderPath setupDestinationFolder)
    {
        Update.SetupDestinationFolder = setupDestinationFolder;
        return this;
    }

    public ICanRunUpdate RunUpdate() => this;

    public ICanRunUpdate IfVersionBiggerThan(VersionTag version)
    {
        Update.CurrentVersion = version;
        return this;
    }

    public ICanRunUpdate IfNotPreview()
    {
        Update.CanUpdatePreviewVersion = false;
        return this;
    }

    public ICanFinishUpdate StartsSetup(bool startSetup = true)
    {
        Update.StartSetup = startSetup;
        return this;
    }

    public ICanBuild TidiesUp(bool runTidyUp = true)
    {
        Update.TidyUpWhenFinishing = runTidyUp;
        return this;
    }

    public Update Build() => Update;
}
