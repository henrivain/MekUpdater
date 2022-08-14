/// Copyright 2021 Henri Vainio 
using MekUpdater.Exceptions;
using MekUpdater.UpdateRunner;

namespace MekUpdater.Check
{
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
        /// <returns>awaitable async Task</returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal async Task<UpdateCheckResult> GetVersionData()
        {
            var message = string.Empty;
            ParsedVersionData? data = null;
            try
            {
                data = await GetAndParseData();
            }
            catch (Exception ex)
            {
                message = $"Get version data failed because of {ExceptionToErrorMsg(ex)}: {ex.Message}";
            }
            return new(data is not null)
            {
                UsedUrl = VersionUrl,
                Message = message,
                DownloadUrl = data?.zipball_url,
                AvailableVersion = (data?.tag_name is null) ? VersionTag.Min : new(data!.tag_name)

            };
        }

        private async Task<ParsedVersionData?> GetAndParseData()
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
                return new GithubApiDataParser(await response.Content.ReadAsStringAsync()).Data;
            }
            return null;
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
}
