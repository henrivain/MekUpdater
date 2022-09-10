namespace MekUpdater.ActionResults
{
    /// <summary>
    /// Update result returned after update zip file is extracted into local filesystem
    /// </summary>
    public class ZipExtractionResult : GetSetupResult
    {
        internal ZipExtractionResult(bool success) : base(success) { }

        /// <summary>
        /// Stack trace to thrown exception
        /// </summary>
        public string? StackTrace { get; init; }
    }
}
