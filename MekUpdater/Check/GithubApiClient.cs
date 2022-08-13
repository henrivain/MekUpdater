/// Copyright 2021 Henri Vainio 
using MekPathLibraryTests.UpdateRunner;
using static MekPathLibraryTests.UpdateDownloadInfo;


namespace MekPathLibraryTests.Check
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
            VersionUrl = Validator.IsRepositoryUrlValid(repositoryVersionUrl);
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
            string message = string.Empty;
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
                VersionData = data,
                
            };
        }

        private async Task<ParsedVersionData?> GetAndParseData()
        {
            using HttpClient client = new HttpClient(new HttpClientHandler()
            {
                UseDefaultCredentials = true,
                UseProxy = false
            });
            client.DefaultRequestHeaders.Add("User-Agent", "request");
            HttpResponseMessage response = await client.GetAsync(VersionUrl);

            if (response.IsSuccessStatusCode)
            {
                return new GithubApiDataParser(await response.Content.ReadAsStringAsync()).Data;
            }
            return null;
        }

        private static ErrorMsg ExceptionToErrorMsg(Exception ex)
        {
            return ex switch
            {
                InvalidOperationException => ErrorMsg.BadUrl,
                HttpRequestException => ErrorMsg.NetworkError,
                TaskCanceledException => ErrorMsg.ServerTimeout,
                DataParseException => ErrorMsg.UnSupportedDataType,
                _ => ErrorMsg.Unknown
            };
        }
    }
}
