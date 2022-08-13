using MekPathLibraryTests.UpdateRunner;

namespace MekUpdater.InstallUpdates
{
    public class ZipExtractionResult : GetSetupResult
    {
        internal ZipExtractionResult(bool success) : base(success) { }

        public string? StackTrace { get; init; }
    }
}
