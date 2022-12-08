using System.Drawing;
using MekUpdater.GithubClient.DataModels;
using Microsoft.Extensions.Logging;

namespace MekUpdater.GithubClient.ApiResults;

/// <summary>
/// Result from action representing action status and used resultPath
/// </summary>
/// <typeparam name="T"></typeparam>
public class FileSystemResult<T>  : UpdaterApiResult where T : ILocalPath
{
    internal FileSystemResult(ResponseMessage responseMessage) : base(responseMessage) { }

    internal FileSystemResult(ResponseMessage responseMessage, string message) 
        : base(responseMessage, message) { }

    internal FileSystemResult(ResponseMessage responseMessage, T resultPath) 
        : this(responseMessage, string.Empty, resultPath) { }

    internal FileSystemResult(ResponseMessage responseMessage, string message, T resultPath) : this(responseMessage, message)
    {
        ResultPath = resultPath;
    }
    
    /// <summary>
    /// Result resultPath from action
    /// </summary>
    public T? ResultPath { get; init; }
}
