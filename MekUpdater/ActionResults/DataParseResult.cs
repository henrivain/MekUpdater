using MekUpdater.CheckUpdates;

namespace MekUpdater.ActionResults;

/// <summary>
/// Result returned after data from github api is parsed
/// </summary>
public class DataParseResult : UpdateResult
{
    internal DataParseResult(bool success) : base(success) { }

    /// <summary>
    /// Data parsed from github api response
    /// </summary>
    public ParsedVersionData? ParsedVersionData { get; init; }
}
