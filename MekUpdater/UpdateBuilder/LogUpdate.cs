using Microsoft.Extensions.Logging;

namespace MekUpdater.UpdateBuilder;

internal partial class UpdateLogger
{
    public UpdateLogger(ILogger? logger)
    {
        _logger = logger;
    }

    private readonly ILogger? _logger;

    private readonly record struct SuccessMessage(string ActionName, string? UsedPath);
    private readonly record struct ErrorMessage(string ActionName, string ActionReference, string Message, string? UsedPath);

    internal virtual void LogResult(UpdateResult result, string actionName)
    {
        if (result is null) return;
        if (result.Success)
        {
            TryLog(new SuccessMessage(actionName, null));
            return;
        }
        TryLog(new ErrorMessage(actionName, result.UpdateMsg.ToString(), result.Message, null));
    }
    internal virtual void LogResult(UpdateResult result, string actionName, string usedPath)
    {
        if (result is null) return;
        if (result.Success)
        {
            TryLog(new SuccessMessage(actionName, usedPath));
            return;
        }
        TryLog(new ErrorMessage(actionName, result.UpdateMsg.ToString(), result.Message, usedPath));
    }
    internal virtual void LogMessage(string message, LogType type)
    {
        TryLog($"[MekUpdater] {message}", type);
    }
    private void TryLog(SuccessMessage message)
    {
        string parsed = $"[MekUpdater] SUCCESS: Action {message.ActionName} completed successfully. {(message.UsedPath is null ? "" : $"\n\tUsed path: {message.UsedPath}")} ";
        TryLog(parsed, LogType.Info);
    }
    private void TryLog(ErrorMessage message)
    {
        string path = string.Empty;
        if (message.UsedPath is null)
        {
            path = $"\n\tusing path: '{message.UsedPath}'";
        }
        string parsed =
            $"[MekUpdater] ERROR: Action {message.ActionName}{path}\n\tfailed because of {message.ActionReference}: {message.Message}";
        TryLog(parsed, LogType.Error);
    }
    private void TryLog(string message, LogType type)
    {
#pragma warning disable CA2254 // Template should be a static expression
        if (_logger is null) return;

        switch (type)
        {
            case LogType.Error:
                _logger.LogError(message);
                break;

            case LogType.Warning:
                _logger.LogWarning(message);
                break;

            case LogType.Info:
                _logger.LogInformation(message);
                break;

            default:
                _logger.LogInformation(message);
                break;
        }



#pragma warning restore CA2254 // Template should be a static expression
    }
}
