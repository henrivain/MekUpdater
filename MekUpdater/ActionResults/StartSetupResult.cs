namespace MekUpdater.ActionResults;

/// <summary>
/// Update result returned after attempt to launch setup.exe 
/// </summary>
public class StartSetupResult : UpdateResult
{
    internal StartSetupResult(bool success) : base(success) { }

    /// <summary>
    /// Path to setup.exe
    /// </summary>
    public SetupExePath? SetupExePath { get; set; }
}
