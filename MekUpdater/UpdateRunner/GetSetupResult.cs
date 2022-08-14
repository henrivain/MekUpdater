using MekUpdater.UpdateBuilder;

namespace MekUpdater.UpdateRunner;

public class GetSetupResult : UpdateResult
{
    internal GetSetupResult(bool success) : base(success)
    {
    }
}
