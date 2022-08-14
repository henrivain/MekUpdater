/// Copyright 2021 Henri Vainio 
using MekUpdater.Exceptions;

namespace MekUpdater.Check
{
    /// <summary>
    /// RepositoryInfo stores data about your repository 
    /// </summary>
    public struct RepositoryInfo
    {
        /// <summary>
        /// Initialize new RepositoryInfo to store info about repository owner, 
        /// name and zipball download Url. Validates all values are not null, empty or whitspace
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="name"></param>
        /// <param name="downloadUrl"></param>
        /// <exception cref="ArgumentException"/>
        public RepositoryInfo(string owner, string name, string downloadUrl)
        {
            RepoOwner = owner;
            RepoName = name;
            DownloadUrl = downloadUrl;
        }

        string _downloadUrl = string.Empty;
        string _repoOwner = string.Empty;
        string _repoName = string.Empty;

        /// <summary>
        /// Repository owner's name
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public string RepoOwner
        {
            get => _repoOwner;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(AppError.Text($"{nameof(RepoOwner)} can't empty"));
                }
                _repoOwner = value;
            }
        }

        /// <summary>
        /// Name of the repository
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public string RepoName
        {
            get => _repoName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(AppError.Text($"{nameof(RepoName)} can't be empty"));
                }
                _repoName = value;
            }
        }

        /// <summary>
        /// Url to download zipball from (full url)
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public string DownloadUrl
        {
            get => _downloadUrl;
            set => _downloadUrl = UrlValidator.IsDownloadUrlValid(value);
        }
    }
}
