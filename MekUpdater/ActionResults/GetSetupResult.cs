namespace MekUpdater.ActionResults;

/// <summary>
/// Update result returned after attempt to find setup.exe from file system
/// </summary>
public class GetSetupResult : UpdateResult
{
    internal GetSetupResult(bool success) : base(success)
    {
    }
}
