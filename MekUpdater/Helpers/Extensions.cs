using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MekUpdater.GithubClient.ApiResults;

namespace MekUpdater.Helpers;

internal static class Extensions
{
    internal static bool IsSuccess(this ResponseMessage msg)
    {
        return msg is ResponseMessage.Success;
    }
    
}
