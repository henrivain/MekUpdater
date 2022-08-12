using MekUpdater.UpdateBuilder;

namespace MekUpdater.UpdateRunner;

internal class FluentUpdater
{
    internal UpdateResult Result { get; private set; }
    private Update Update { get; }

    internal FluentUpdater(Update update)
    {
        Result = new();
        Update = update;
    }

    internal async Task<UpdateCheckResult> CheckForUpdatesAsync()
    {
        UpdateCheckResult result = new();


        return result;
    }

    internal async Task<GetSetupFileResult> UpdateAndExtractAsync()
    {
        GetSetupFileResult result = new();



        return result;
    }

    internal static void StartSetup()
    {

    }

    internal async Task<FinishCleanUpResult> TidyUpAsync()
    {
        FinishCleanUpResult result = new();

        return result;
    }
}
