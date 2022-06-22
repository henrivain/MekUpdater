/// Copyright 2021 Henri Vainio 
using MekUpdater.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace MekUpdater.Helpers
{
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



        /// <summary>
        /// Validates, that path is valid windows path
        /// </summary>
        /// <param name="path"></param>
        /// <returns>input string if valid, else throws Argument exception</returns>
        /// <exception cref="ArgumentException"></exception>
        internal static string GetCorrectWindowsPath(string path)
        {
            PathInfo info = ValidateAndGetWindowsPath(path);
            if (info.IsPath is false)
            {
                throw new ArgumentException(AppError.Text(info.Msg, 2, info.Path));
            }
            return info.Path;
        }



        /// <summary>
        /// Gets full path and validates it
        /// </summary>
        /// <param name="path"></param>
        /// <returns>if path valid (true, "", path), else (false, whyPathFailed, path)</returns>
        private static PathInfo ValidateAndGetWindowsPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return new(false, "Path can't be null, empty or whitspace", path);
            }

            try
            {
                path = Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
                return new(false, ex.Message, path);
            }

            if (Path.IsPathFullyQualified(path) is false)
            {
                return new(false, "Path is not fully qualified", path);
            }
            return new(true, string.Empty, path);
        }

        /// <summary>
        /// Struct to 
        /// </summary>
        private struct PathInfo
        {
            /// <summary>
            /// Generate new PathInfo with parameters
            /// </summary>
            /// <param name="isPath"></param>
            /// <param name="msg"></param>
            /// <param name="path"></param>
            internal PathInfo(bool isPath, string msg, string path) 
            {
                IsPath = isPath;
                Msg = msg;
                Path = path;
            }
     
            /// <summary>
            /// Bool: is valid path?
            /// </summary>
            internal bool IsPath { get; }
            /// <summary>
            /// Message why path is not valid
            /// </summary>
            internal string Msg { get; }
            /// <summary>
            /// Full path if valid
            /// </summary>
            internal string Path { get; }
        }
    }
}
