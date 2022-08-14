namespace MekUpdater.ActionResults
{
    public class ZipExtractionResult : GetSetupResult
    {
        internal ZipExtractionResult(bool success) : base(success) { }

        public string? StackTrace { get; init; }
    }
}
