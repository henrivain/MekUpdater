namespace MekUpdater.ActionResults;

/// <summary>
/// Update result returned after useless cache file are removed
/// </summary>
public class FinishCleanUpResult : UpdateResult
{
    internal FinishCleanUpResult(bool success) : base(success) { }
}
