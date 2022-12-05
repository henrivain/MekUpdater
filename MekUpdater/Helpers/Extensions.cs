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
}
