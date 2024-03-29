﻿using MekUpdater.GithubClient.ApiResults;
using MekUpdater.Interfaces;

namespace MekUpdater.GithubClient;

/// <summary>
/// Interface to guide implementations for managing github download operations
/// </summary>
public interface IGithubDownloadClient : IDisposable
{
    /// <summary>
    /// Download latest release zip source code to given folder.
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="destinationFolder"></param>
    /// <returns>DownloadResult of ZipPath representing request data and info</returns>
    Task<DownloadResult<ZipPath>> DownloadReleaseZip(VersionTag tag, FolderPath destinationFolder);



    /// <summary>
    /// Download asset form specific version to given destination. 
    /// Downloads first asset from given releasse that has matching name.
    /// </summary>
    /// <param name="tag">IVersion tag of the release.</param>
    /// <param name="path">ResultPath where asset will be downloaded.</param>
    /// <param name="assetName">Name that will be used to validate that the asset is the right one.</param>
    /// <param name="onlyFullMatch">Specifies weather asset name should fully match assetName or not.</param>
    /// <returns>DownloadResult of IFilePath representing download status and information about download.</returns>
    Task<DownloadResult<IFilePath>> DownloadAsset(VersionTag tag, FolderPath path, string assetName, bool onlyFullMatch = false);

    /// <summary>
    /// Download latest release into specified destination.
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="fileName">
    /// File name for downloaded zip file. Default file name is in format "release_{releaseVersion}.zip"
    /// Lacking .zip -extension will be automatically added.
    /// </param>
    /// <returns>DownloadResult of ZipPath representing download status and information about download.</returns>
    Task<DownloadResult<ZipPath>> DownloadLatestReleaseZip(FolderPath destination, string? fileName = null);


    /// <summary>
    /// Client to help getting information about releases.
    /// </summary>
    IGithubInfoClient InfoClient { get; }
}
