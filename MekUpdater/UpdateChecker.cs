/// Copyright 2021 Henri Vainio 
using MekUpdater.CheckUpdates;
using MekUpdater.Exceptions;
using MekUpdater.Helpers;
using System;
using System.Threading.Tasks;

namespace MekUpdater
{
    /// <summary>
    /// Get information about repository version from github api
    /// </summary>
    public class UpdateChecker
    {
        /// <summary>
        /// Initialize new update checker. Give repository owner name and repository name 
        /// </summary>
        /// <param name="repoOwner"></param>
        /// <param name="repoName"></param>
        public UpdateChecker(string repoOwner, string repoName)
        {
            // Also validates repoUrl when initialising client
            Client = new($"https://api.github.com/repos/{repoOwner}/{repoName}/releases/latest"); 
        }



        /// <summary>
        /// Info from latest check
        /// </summary>
        ParsedVersionData? LatestInfo { get; set; }

        GithubApiClient Client { get; init; }



        /// <summary>
        /// Was last CheckForUpdates() Successful
        /// </summary>
        public bool IsSuccess { get; private set; } = false;
        public string? DownloadLink { get => LatestInfo?.zipball_url; }
        public string? VersionPageLink { get => LatestInfo?.html_url; }
        public string? VersionString { get => LatestInfo?.tag_name; }
        public string? NewVersionName { get => LatestInfo?.name; }
        public DateTime? UpdateReleased { get => LatestInfo?.published_at; }
        public bool? IsPreRelease { get => LatestInfo?.prerelease; }
        public string? ReleaseBranch { get => LatestInfo?.target_commitish; }
        public DateTime? LastTimeChecked { get; private set; }

        /// <summary>
        /// Get version data from github api
        /// <para/>throws InvalidOperationException if getting version data is failed
        /// <para/>throws DataParseException if not able to parse data
        /// </summary>
        /// <returns>task of UpdateChecker</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="DataParseException"></exception>
        public async Task<UpdateChecker> CheckForUpdates()
        {
            try
            {
                await Client.GetVersionData();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

            if (Client.ParsedData is null)
            {
                throw new DataParseException(AppError.Text($"ParsedData is null (json data was probably damaged or in wrong format)"));
            }

            LatestInfo = Client.ParsedData;
            IsSuccess = true;
            LastTimeChecked = DateTime.Now;
            return this;
        }
    }
}
