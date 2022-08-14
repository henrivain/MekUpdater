/// Copyright 2021 Henri Vainio 
namespace MekUpdater.Helpers;

internal class UrlValidator
{
    /// <summary>
    /// Validate url path is type "https://api.github.com/repos/{author}/{repo}/releases/latest"
    /// </summary>
    /// <param name="url"></param>
    /// <returns>input string if valid, else throws Argument exception</returns>
    /// <exception cref="ArgumentException"></exception>
    internal static string IsRepositoryUrlValid(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Repository url can't be null, empty or whitspace");
        }
        if (url.Trim().StartsWith("https://api.github.com/repos") is false)
        {
            throw new ArgumentException($"Bad url path. Url must include 'https://api.github.com/repos', given url was '{url}'");
        }

        if (url.Contains("releases/latest") is false)
        {
            throw new ArgumentException($"Bad url path. Must include 'releases/latest', given url was '{url}'");
        }
        return url;
    }

    /// <summary>
    /// Validates github version download url
    /// <para/>must use type "https://api.github.com/repos/{author}/{repo}/zipball/{version}"
    /// </summary>
    /// <param name="url"></param>
    /// <returns>input string if valid, else throws Argument exception</returns>
    /// <exception cref="ArgumentException"></exception>
    internal static string IsDownloadUrlValid(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Download url can't be null, empty or whitspace");
        }
        if (url.Contains("https://api.github.com/repos/") is false)
        {
            throw new ArgumentException($"Download url must include 'https://api.github.com/repos/', given url was '{url}'");
        }
        if (url.Contains("zipball") is false)
        {
            throw new ArgumentException($"Download url must include 'zipball', given url was '{url}'");
        }
        return url;
    }
}
