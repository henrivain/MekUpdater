namespace MekUpdater.Check;

public class DataParseResult : UpdateResult
{
    internal DataParseResult(bool success) : base(success) { }

    public ParsedVersionData? ParsedVersionData { get; init; }
}
