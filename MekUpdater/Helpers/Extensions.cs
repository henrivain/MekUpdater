using MekUpdater.GithubClient.ApiResults;

namespace MekUpdater.Helpers;

internal static class Extensions
{
    internal static bool IsSuccess(this ResponseMessage msg)
    {
        return msg is ResponseMessage.Success;
    }
}
