namespace MekUpdater.UpdateBuilder.Interfaces;

public interface ICanRunUpdate
{
    ICanRunUpdate IfVersionBiggerThan(VersionTag version);
    ICanRunUpdate IfNotPreview();
    ICanFinishUpdate StartsSetup(bool startSetup = false);
}
