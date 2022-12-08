using MekUpdater.GithubClient.ApiResults;

namespace MekUpdater.Helpers;

/// <summary>
/// Extensions for updater results
/// </summary>
public static class Extensions
{
    internal static bool IsSuccess(this ResponseMessage msg)
    {
        return msg is ResponseMessage.Success;
    }
    internal static bool NotSuccess(this ResponseMessage msg)
    {
        return msg.IsSuccess() is false;
    }

    /// <summary>
    /// Check weather result returnes as successful
    /// </summary>
    /// <param name="result"></param>
    /// <returns>true if ResponseMessage.Success, otherwise false</returns>
    public static bool IsSuccess(this UpdaterApiResult result)
    {
        return result.ResponseMessage.IsSuccess();
    }

    /// <summary>
    /// Check weather result returnes as unsuccessful
    /// </summary>
    /// <param name="result"></param>
    /// <returns>false if ResponseMessage.Success, otherwise true</returns>
    public static bool NotSuccess(this UpdaterApiResult result)
    {
        return result.ResponseMessage.NotSuccess();
    }

    internal static bool IsSuccess(this HttpResponseMessage? msg)
    {
        return msg?.IsSuccessStatusCode is true;
    }
    internal static bool NotSuccess(this HttpResponseMessage? msg)
    {
        return msg.IsSuccess() is false;
    }

    internal static FolderPath AppendSingleFolder(this FolderPath path, string folderName)
    {
        if (Path.EndsInDirectorySeparator(folderName) is false)
        {
            folderName += Path.DirectorySeparatorChar;
        }
        var temp = path;
        temp.Append(folderName);
        return temp;
    }
}
