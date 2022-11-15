﻿namespace MekUpdater.GithubClient.ApiResults;
/// <summary>
/// Status for github api response
/// </summary>
public enum ResponseMessage
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    Success,
    Error,
    ServerError,
    None
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
