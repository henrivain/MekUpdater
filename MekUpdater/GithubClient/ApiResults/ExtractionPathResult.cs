namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Data structure to store data about extraction where result folder is also validated
/// </summary>
/// <param name="ResponseMessage"></param>
/// <param name="Message"></param>
/// <param name="resultFolder"></param>
public class ExtractionPathResult : UpdaterApiResult
{
    internal ExtractionPathResult(ResponseMessage responseMessage) : base(responseMessage) { }

    internal ExtractionPathResult(ResponseMessage responseMessage, string message) : base(responseMessage, message) { }

    internal ExtractionPathResult(ResponseMessage responseMessage, string message, FolderPath resultFolder) : base(responseMessage, message)
    {
        ResultFolder = resultFolder;
    }

    /// <summary>
    /// Extracted "zip" folder
    /// <para/>Extract file MyStuff.zip to folder C:\Extract => C:\Extract\MyStuff (is ResultFolder)
    /// </summary>
    public FolderPath? ResultFolder { get; init; }
};
