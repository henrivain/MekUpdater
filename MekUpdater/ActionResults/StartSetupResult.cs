namespace MekUpdater.ActionResults
{
    public class StartSetupResult : UpdateResult
    {
        internal StartSetupResult(bool success) : base(success) { }
        
        public SetupExePath? SetupExePath { get; set; }
    }
}
