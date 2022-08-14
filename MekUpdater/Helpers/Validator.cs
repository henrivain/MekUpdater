
using MekUpdater.Exceptions;
/// Copyright 2021 Henri Vainio 
namespace MekUpdater.Helpers;

internal class Validator
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
            throw new ArgumentException(AppError.Text("Repository url can't be null, empty or whitspace", 2));
        }
        if (url.Trim().StartsWith("https://api.github.com/repos") is false)
        {
            throw new ArgumentException(
                AppError.Text(
                    "Bad url path. Url must include \"https://api.github.com/repos\"", 2, url
                ));
        }

        if (url.Contains("releases/latest") is false)
        {
            throw new ArgumentException(
                AppError.Text(
                    "Bad url path. Must include \"releases/latest\"", 2, url
                    ));
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
            throw new ArgumentException(AppError.Text("Download url can't be null, empty or whitspace", 2));
        }
        if (url.Contains("https://api.github.com/repos/") is false)
        {
            throw new ArgumentException(AppError.Text("Download url must include \"https://api.github.com/repos/\"", 2, url));
        }
        if (url.Contains("zipball") is false)
        {
            throw new ArgumentException(AppError.Text("Download url must include \"zipball\"", 2, url));
        }
        return url;
    }
}
