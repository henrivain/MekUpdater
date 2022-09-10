namespace MekUpdater.UpdateBuilder.Interfaces;

public interface ICanFinishUpdate : ICanBuild
{
    ICanBuild TidiesUp(bool runTidyUp = true);
}
