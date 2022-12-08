using System.Text.Json;
using MekUpdater.GithubClient.ApiResults;
using MekUpdater.GithubClient.FileManagers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MekUpdater.GithubClient;

/// <summary>
/// New client for handling requests to github api
/// </summary>
public class GithubApiClient : IDisposable
{
    /// <summary>
    /// Initialize new client to call github api
    /// </summary>
    /// <param name="githubUserName"></param>
    /// <param name="githubRepositoryName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private protected GithubApiClient(string githubUserName, string githubRepositoryName)
    {
        if (string.IsNullOrWhiteSpace(githubUserName))
            throw new ArgumentNullException(nameof(githubUserName));
        if (string.IsNullOrWhiteSpace(githubRepositoryName))
            throw new ArgumentNullException(nameof(githubUserName));
        BaseAddress = $"https://api.github.com/repos/{githubUserName}/{githubRepositoryName}";

        HttpClient client = new(new HttpClientHandler()
        {
            UseDefaultCredentials = true,
            UseProxy = false
        })
        {
            BaseAddress = new Uri(BaseAddress)
        };
        client.DefaultRequestHeaders.Add("User-Agent", "request");
        Client = client;

    }

    /// <summary>
    /// Initialize new client to call github api. Provided logger will be used.
    /// </summary>
    /// <param name="githubUserName"></param>
    /// <param name="githubRepositoryName"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException">If username or repository name is null or whitspace</exception>
    private protected GithubApiClient(
        string githubUserName, string githubRepositoryName, ILogger<GithubApiClient> logger)
        : this(githubUserName, githubRepositoryName)
    {
        Logger = logger;
        Logger.LogInformation("New {className} with username: '{githubUserName}' and repo name: '{githubRepositoryName}'",
            nameof(GithubApiClient), githubUserName, githubRepositoryName);
    }

    /// <summary>
    /// Base url apiRoute to api (includes repo owner name and repo name)
    /// </summary>
    public string BaseAddress { get; }
    private HttpClient Client { get; }
    private protected ILogger<GithubApiClient> Logger { get; } = NullLogger<GithubApiClient>.Instance;

    /// <summary>
    /// Download file form given url into given file pat. ResultPath will be created or old data will be overritten.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="path"></param>
    /// <returns>DownloadResult representing download status and used data.</returns>
    private protected virtual async Task<DownloadResult<T>> DownloadFileAsync<T>(string url, T path) where T : IFilePath
    {
        Logger.LogInformation("Download file from '{url}' to path '{path}'.", url, path.FullPath);
        FolderHandler folderHandler = new(path.FolderPath, Logger);
        var dirResult = folderHandler.TryCreateDirectory();
        if (dirResult.NotSuccess())
        {
            Logger.LogError("Cannot download file, destination path invalid.");
            return new(ResponseMessage.CannotCreateFile)
            {
                Message = dirResult.Message,
                DownloadUrl = url
            };
        }

        using var response = await GetResponse(url);
        if (response.NotSuccess())
        {
            Logger.LogWarning("Cannot download file, because the http request failed");
            return new(response.ResponseMessage)
            {
                Message = response.Message,
                DownloadUrl = url
            };
        }
        FileHandler fileHandler = new(path, Logger);
        using Stream? stream = await response.Content!.Content.ReadAsStreamAsync();
        var streamCopyResult = await fileHandler.WriteStreamAsync(stream);

        if (streamCopyResult.NotSuccess())
        {
            Logger.LogWarning("Failed to copy data to file '{file}', message: '{message}'",
                path.FullPath, streamCopyResult.Message);
            return new(ResponseMessage.CannotCopyStream, streamCopyResult.Message, path, url);
        }
        Logger.LogInformation("File downloaded successfully.");
        return new(ResponseMessage.Success, string.Empty, path, url);
    }

    /// <summary>
    /// Make request to github api and parse result into T type.
    /// </summary>
    /// <typeparam name="T">
    /// Type of response data used when parsing object from json.
    /// </typeparam>
    /// <param name="url">
    /// Full url path to Github api apiRoute.
    /// </param>
    /// <returns>
    /// GithubApiTResult of given type.
    /// This result represents response data and request status.
    /// </returns>
    private protected virtual async Task<GithubApiTResult<T?>> GetApiResultAsync<T>(string url)
    {
        using var response = await GetResponse(url);
        if (response.NotSuccess())
        {
            Logger.LogWarning("Github api request failed. '{reason}': '{explanation}'",
            response.ResponseMessage, response.Message);
            return new GithubApiTResult<T?>(response.ResponseMessage, response.Message);
        }
        var parsed = await ParseJsonAsync<T>(response.Content);
        if (parsed.NotSuccess())
        {
            Logger.LogWarning("Parsing github api response json into '{type}' failed " +
                "because of '{reason}': '{explanation}'.", typeof(T), parsed.ResponseMessage, parsed.Message);
        }
        return parsed;
    }

    /// <summary>
    /// Get response from given http adress asynchronously. 
    /// Also handle exceptions and convert them into Content and messages.
    /// </summary>
    /// <param name="apiRoute">url apiRoute where request will be made</param>
    /// <returns>
    /// HttpRequestResult that represents response data and response status.
    /// Valid => "ResponseMessage.Success".
    /// Bad request => "ResponseMessage.HttpRequestUnsuccessful".
    /// Exception => Response message that represents exception reason.
    /// </returns>
    private protected virtual async Task<HttpRequestResult> GetResponse(string apiRoute)
    {
        Logger.LogInformation("Send request to api route '{apiRoute}'", apiRoute);
        try
        {
            var response = await Client.GetAsync(apiRoute);
            if (response.NotSuccess())
            {
                int status = (int)response.StatusCode;
                string? reason = response.ReasonPhrase;
                Logger.LogWarning("Request to api endpoint '{endpoint}' was unsuccessful with status '{status}': '{msg}'",
                    apiRoute, status, reason);

                return new(ResponseMessage.HttpRequestUnsuccessful)
                {
                    Message = $"Request to api route '{apiRoute}' was unsuccessful with status '{status}': '{reason}'",
                    Content = response
                };
            }
            Logger.LogInformation("Request successful.");
            return new(ResponseMessage.Success)
            {
                Content = response
            };
        }
        catch (Exception ex)
        {
            ResponseMessage error = ex switch
            {
                UriFormatException => ResponseMessage.BadUri,
                InvalidOperationException => ResponseMessage.UriNotAbsolute,
                TaskCanceledException => ResponseMessage.ServerTimedOut,
                HttpRequestException => ResponseMessage.NetworkError,
                _ => ResponseMessage.UnknownHttpRequestException
            };
            Logger.LogWarning("Http request to {address} failed because of exception '{ex}': '{msg}'",
                apiRoute, ex.GetType(), ex.Message);
            return new(error)
            {
                Message = $"Http request to '{apiRoute}' failed because of " +
                    $"{ex.GetType()}: {ex.Message}.",
                Content = null
            };
        }
    }

    /// <summary>
    /// Parse HttpResponseMessage content into wanted T object. 
    /// Also checks if request was successful.
    /// </summary>
    /// <typeparam name="T">Type of wanted result.</typeparam>
    /// <param name="response">HttpResponseMessage object from http request.</param>
    /// <returns>
    /// Success ResponseMessage with Parsed json as T object if response content valid. 
    /// <para/>Otherwise Result null, ResponseMessage different from success and Message explaining reason for error.
    /// </returns>
    private protected static async Task<GithubApiTResult<T?>> ParseJsonAsync<T>(
        HttpResponseMessage? response)
    {
        if (response is null)
        {
            return new(ResponseMessage.ResponseObjectNull, 
                $"Given HttpResponseMessage is null and cannot be parsed into desired type.");
        }
        if (response.NotSuccess())
        {
            string message = $"Http request made was unsuccessful with message '{response.RequestMessage}'";
            return new(ResponseMessage.UnsuccessfulRequest, message);
        }
        var json = await response.Content.ReadAsStringAsync();
        var (result, msg) = ParseJson<T>(json);
        if (msg.NotSuccess())
        {
            return new(msg, "Bad json string.");
        }
        return new(msg, result);
    }

    /// <summary>
    /// Parse json string into object with wanted type T
    /// </summary>
    /// <typeparam name="T">Type of wanted result.</typeparam>
    /// <param name="json">Json string.</param>
    /// <returns>ResponseMessage "Success" and Result with parsed data if given json string valid, else ResponseMessage not success and Result null</returns>
    private protected static (T? Result, ResponseMessage Msg) ParseJson<T>(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return (default, ResponseMessage.JsonStringNull);
        }
        try
        {
            return (JsonSerializer.Deserialize<T>(json), ResponseMessage.Success);
        }
        catch (Exception ex)
        {
            ResponseMessage msg = ex switch
            {
                ArgumentNullException => ResponseMessage.JsonStringNull,
                JsonException => ResponseMessage.InvalidJson,
                NotSupportedException => ResponseMessage.NoValidJsonConverter,
                _ => ResponseMessage.UnknownJsonParseException
            };
            return (default, msg);
        }
    }

    /// <summary>
    /// Dispose all managed and unmanaged resources from this GithubApiClient instance
    /// </summary>
    public virtual void Dispose()
    {
        Client?.Dispose();
        GC.SuppressFinalize(this);
    }
}

