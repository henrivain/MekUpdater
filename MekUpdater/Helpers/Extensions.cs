using System.Runtime.CompilerServices;
using MekUpdater.GithubClient.ApiResults;

namespace MekUpdater.Helpers;

internal static class Extensions
{
    internal static bool IsSuccess(this ResponseMessage msg)
    {
        return msg is ResponseMessage.Success;
    }
    internal static bool NotSuccess(this ResponseMessage msg)
    {
        return msg.IsSuccess() is false;
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
