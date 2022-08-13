using MekPathLibraryTests.UpdateBuilder;

namespace MekPathLibraryTests.UpdateRunner;

public class GetSetupResult : UpdateResult
{
    internal GetSetupResult(bool success) : base(success)
    {
    }
}
