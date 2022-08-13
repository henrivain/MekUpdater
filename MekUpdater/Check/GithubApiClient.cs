/// Copyright 2021 Henri Vainio 
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
        /// Parsed version of some of the data from github api
        /// </summary>
        internal ParsedVersionData? ParsedData { get; private set; } = null;
        /// <summary>
        /// Was last check successful
        /// </summary>
        internal bool IsSuccess { get; private set; } = false;



        private static readonly HttpClientHandler _clientHandler = new()
        {
            UseDefaultCredentials = true,
            UseProxy = false
        };



        /// <summary>
        /// Get repository version data from github using api route
        /// <para/>For example: "https://api.github.com/repos/matikkaeditorinkaantaja/Matikkaeditorinkaantaja/releases/latest"
        /// </summary>
        /// <returns>awaitable async Task</returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal async Task GetVersionData()
        {
            try
            {
                HttpClient client = new HttpClient(_clientHandler);
                client.DefaultRequestHeaders.Add("User-Agent", "request");
                HttpResponseMessage response = await client.GetAsync(VersionUrl);
                await HandleReturn(response);
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                throw new InvalidOperationException(AppError.Text(
                    $"Failed to get version data because of error {GetExceptionReason(ex)}. Path: \"{VersionUrl}\""
                    ));
            }
        }

        /// <summary>
        /// Parse data and return values
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task HandleReturn(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                ParsedData = new GithubApiDataParser(
                    await response.Content.ReadAsStringAsync())
                        .Data;

                IsSuccess = true;
                return;
            }
            IsSuccess = false;
            return;
        }

        /// <summary>
        /// Get reason for exception thrown in GetVersionData
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>fitting ErrorMsg or ErrorMsg.Unknown if exception not known</returns>
        private static ErrorMsg GetExceptionReason(Exception ex)
        {
            return ex switch
            {
                InvalidOperationException => ErrorMsg.BadUrl,
                HttpRequestException => ErrorMsg.NetworkError,
                TaskCanceledException => ErrorMsg.ServerTimeout,
                _ => ErrorMsg.Unknown
            };
        }
    }
}
