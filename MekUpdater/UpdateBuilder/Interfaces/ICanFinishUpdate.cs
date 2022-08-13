namespace MekPathLibraryTests.UpdateBuilder.Interfaces;

public interface ICanFinishUpdate
{
    ICanBuild TidiesUp(bool runTidyUp = true);
}
