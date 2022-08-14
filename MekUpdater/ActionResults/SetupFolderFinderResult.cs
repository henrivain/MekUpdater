namespace MekUpdater.ActionResults;

public class SetupFolderFinderResult : SetupPathFinderResult
{
    internal SetupFolderFinderResult(bool success) : base(success) { }
    public override SetupExePath? SetupPath => null;
    public virtual string? SetupFolderName { get; init; }
}
