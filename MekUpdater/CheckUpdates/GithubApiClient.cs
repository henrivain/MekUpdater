// Copyright 2021 Henri Vainio 
using MekUpdater.Exceptions;
using MekUpdater.UpdateRunner;

namespace MekUpdater.CheckUpdates;

/// <summary>
/// App version data handler for github api
/// </summary>
internal class GithubApiClient
{
    /// <summary>
    /// Path must be format "https://api.github.com/repos/{author}/{repo}/releases/latest"
    /// </summary>
    /// <param name="repositoryVersionUrl"></param>
    /// <exception cref="ArgumentException"></exception>
    internal GithubApiClient(string repositoryVersionUrl)
    {
        VersionUrl = UrlValidator.IsRepositoryUrlValid(repositoryVersionUrl);
    }

    /// <summary>
    /// Url to github api "latest version"
    /// </summary>
    private string VersionUrl { get; }

    /// <summary>
    /// Get repository version data from github using api route
    /// <para/>For example: "https://api.github.com/repos/matikkaeditorinkaantaja/Matikkaeditorinkaantaja/releases/latest"
    /// </summary>
    /// <returns>result that represents success and data gotten from api</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<UpdateCheckResult> GetVersionData()
    {
        string message;
        DataParseResult? result = null;
        try
        {
            result = await GetAndParseData();
            message = result.Message;
        }
        catch (Exception ex)
        {
            message = $"Get version data failed because of {ExceptionToErrorMsg(ex)}: {ex.Message}";
        }

        VersionTag availableVersion = VersionTag.Min;
        if (result?.ParsedVersionData?.tag_name is not null)
        {
            availableVersion = new(result.ParsedVersionData.tag_name);
        }

        return new(result?.ParsedVersionData is not null)
        {
            UsedUrl = VersionUrl,
            Message = message,
            DownloadUrl = result?.ParsedVersionData?.zipball_url,
            AvailableVersion = availableVersion
        };
    }

    private async Task<DataParseResult> GetAndParseData()
    {
        using var client = new HttpClient(new HttpClientHandler()
        {
            UseDefaultCredentials = true,
            UseProxy = false
        });
        client.DefaultRequestHeaders.Add("User-Agent", "request");
        var response = await client.GetAsync(VersionUrl);

        if (response.IsSuccessStatusCode)
        {
            return new GithubApiDataParser(
                await response.Content.ReadAsStringAsync()
                ).Parse();
        }
        return new(false)
        {
            Message = $"Request failed with status code {response.StatusCode} and message {response.ReasonPhrase}",
            UpdateMsg = UpdateMsg.HttpRequestFailed
        };
    }

    private static UpdateMsg ExceptionToErrorMsg(Exception ex)
    {
        return ex switch
        {
            InvalidOperationException => UpdateMsg.BadUrl,
            HttpRequestException => UpdateMsg.NetworkError,
            TaskCanceledException => UpdateMsg.ServerTimeout,
            DataParseException => UpdateMsg.UnSupportedDataType,
            _ => UpdateMsg.Unknown
        };
    }
}
