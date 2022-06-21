/// Copyright 2021 Henri Vainio 
using MekUpdater.Helpers;
using System.Collections.Generic;
using System;
using MekUpdater.Exceptions;

namespace MekUpdater
{
    /// <summary>
    /// UpdateDownloadInfo for handling update information during and after update
    /// </summary>
    public class UpdateDownloadInfo
    {
        /// <summary>
        /// Initialize new UpdateDownloadInfo for handling update information during and after update
        /// </summary>
        public UpdateDownloadInfo() { }

        /// <summary>
        /// Initialize new UpdateDownloadInfo with filepath, extractPath, and RepoInfo
        /// throws argument exception if any of them are invalid
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <param name="filePath"></param>
        /// <param name="extractPath"></param>
        /// <exception cref="ArgumentException"></exception>
        public UpdateDownloadInfo(string filePath, string extractPath, RepositoryInfo repoInfo) 
        {
            ZipFilePath = filePath;
            ExtractPath = extractPath;
            RepoInfo = repoInfo;
        }

        /// <summary>
        /// Initialize new UpdateDownloadInfo with known downloadUrl, filepath, extractPath, repoOwner, repoName
        /// throws argument exception if any of them are invalid
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <param name="filePath"></param>
        /// <param name="extractPath"></param>
        /// <exception cref="ArgumentException"></exception>
        public UpdateDownloadInfo(string downloadUrl, string filePath, 
            string extractPath, string repoOwner, string repoName)
        {
            ZipFilePath = filePath;
            ExtractPath = extractPath;
            RepoInfo = new(repoOwner, repoName, downloadUrl);
        }

        string _zipFilePath = string.Empty;
        string _extractPath = string.Empty;

        /// <summary>
        /// Place where file will be placed (with file extension .zip)
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public string ZipFilePath 
        { 
            get => _zipFilePath; 
            set => _zipFilePath = Validator.GetCorrectZipPath(value); 
        }

        /// <summary>
        /// Place where file will be extracted (must be path to folder)
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public string ExtractPath
        {
            get => _extractPath;
            set => _extractPath = Validator.GetCorrectFolderPath(value);
        }

        /// <summary>
        /// Info about repository's name, author and zipball url
        /// </summary>
        public RepositoryInfo RepoInfo;

        public bool IsUpdating { get; set; } = false;

        /// <summary>
        /// State where fail happened
        /// </summary>
        public enum FailState
        {
            Non, Download, Extracting, Launching
        }

        /// <summary>
        /// Reason for exception or other fail during update process
        /// </summary>
        public enum ErrorMsg
        {
            Unknown, BadUrl, PathNotOpen, FileReadOnly, FileNotFound,
            PathTooLong, NoDirectory, NoPermission, OutsideMachine,
            ServerTimeout, NetworkError, Other, UnSupportedDataType,
            InvalidPathChars, FileAlreadyExistOrBadName, BadPathFormat,
            ObjectDisposed, NoFileName, ErrorWhileOpening, NoShellSupport,
            PathNullOrEmpty
        }

        /// <summary>
        /// Possible statuses for updater to be on
        /// </summary>
        public enum CompletionStatus
        {
            Waiting, Downloading, Copying, DownloadCompleted, DownloadFailed,
            Extracting, ExtractionCompleted, ExtractionFailed,
            Launching, LaunchingCompleted, LaunchingFailed, 
            Completed, Failed
        }

        /// <summary>
        /// Statuses in right order (newest on top)
        /// </summary>
        public Stack<CompletionStatus> Statuses = new();

        /// <summary>
        /// Returns current status from statuses
        /// </summary>
        public CompletionStatus Status
        {
            get
            {
                if (Statuses.TryPeek(out CompletionStatus status))
                {
                    return status;
                }
                return CompletionStatus.Waiting;
            }
        }

        /// <summary>
        /// Contains information about where and what went wrong in case of fail
        /// <para/>default (Non, Unknown)
        /// </summary>
        public (FailState state, ErrorMsg msg) Error { get; set; } = (FailState.Non, ErrorMsg.Unknown);

        /// <summary>
        /// Set status CompletionStatus.Copying
        /// </summary>
        public void Copying()
        {
            Statuses.Push(CompletionStatus.Copying);
        }
        /// <summary>
        /// Set status CompletionStatus.DownloadFailed
        /// </summary>
        public void DownloadFailed()
        {
            Statuses.Push(CompletionStatus.DownloadFailed);
        }        
        /// <summary>
        /// Set status CompletionStatus.Completed
        /// </summary>
        public void DownloadCompleted()
        {
            Statuses.Push(CompletionStatus.DownloadCompleted);
        }
        /// <summary>
        /// Set status CompletionStatus.Downloading
        /// </summary>
        public void Downloading()
        {
            Statuses.Push(CompletionStatus.Downloading);
        }
        /// <summary>
        /// Set status CompletionStatus.Extracting
        /// </summary>
        public void Extracting()
        {
            Statuses.Push(CompletionStatus.Extracting);
        }
        /// <summary>
        /// Set status CompletionStatus.ExtractionCompleted
        /// </summary>
        public void ExtractionCompleted()
        {
            Statuses.Push(CompletionStatus.ExtractionCompleted);
        }
        /// <summary>
        /// Set status CompletionStatus.ExtractionFailed
        /// </summary>
        public void ExtractionFailed()
        {
            Statuses.Push(CompletionStatus.ExtractionFailed);
        }
        /// <summary>
        /// Set status CompletionStatus.Failed
        /// </summary>
        public void Failed()
        {
            Statuses.Push(CompletionStatus.Failed);
        }
        /// <summary>
        /// Set status CompletionStatus.Completed
        /// </summary>
        public void Completed()
        {
            Statuses.Push(CompletionStatus.Completed);
        }

        /// <summary>
        /// Set status CompletionStatus.Waiting
        /// </summary>
        public void Waiting()
        {
            Statuses.Push(CompletionStatus.Waiting);
        }
        /// <summary>
        /// Set status CompletionStatus.Launching
        /// </summary>
        public void Launching()
        {
            Statuses.Push(CompletionStatus.Launching);
        }

        /// <summary>
        /// Set status CompletionStatus.LaunchingCompleted
        /// </summary>
        public void LaunchingCompleted()
        {
            Statuses.Push(CompletionStatus.LaunchingCompleted);
        }

        /// <summary>
        /// Set status CompletionStatus.LaunchingFailed
        /// </summary>
        public void LaunchingFailed()
        {
            Statuses.Push(CompletionStatus.LaunchingFailed);
        }

        /// <summary>
        /// Check if last download was completed succesfully reading Info.statuses stack
        /// </summary>
        /// <returns>True if was succesful, false if failed download or no download made</returns>
        public bool IsDownloadCompletedSuccesfully()
        {
            foreach (var status in Statuses.ToArray())
            {
                if (status is CompletionStatus.DownloadCompleted) return true;
                if (status is CompletionStatus.DownloadFailed) return false;
            }
            return false;
        }
    }
}
