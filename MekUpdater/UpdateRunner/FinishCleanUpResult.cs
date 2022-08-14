using MekUpdater.UpdateBuilder;

namespace MekUpdater.UpdateRunner;

public class FinishCleanUpResult : UpdateResult
{
    public FinishCleanUpResult(bool success) : base(success) { }
}
