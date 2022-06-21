/// Copyright 2021 Henri Vainio 
using MekUpdater.Exceptions;
using MekUpdater.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekUpdater.InstallUpdates
{
    /// <summary>
    /// Get path to your applications setup file
    /// </summary>
    internal class SetupFilePath
    {
        /// <summary>
        /// Initialize new SetupFilePath by trying to take new path "extractPath/-RepoOwnerName-RepoName/setup.exe"
        /// Where file name must include "setup" and must be ".exe". 
        /// </summary>
        /// <param name="pathToExtractFolder"></param>
        /// <param name="repoOwnerName"></param>
        /// <param name="repoName"></param>
        internal SetupFilePath(string pathToExtractFolder, string repoOwnerName, string repoName)
        {
            PathToExtractFolder = pathToExtractFolder;
            RepoOwnerName = repoOwnerName;
            _repoName = repoName;
            TryGetSetupPath();
        }

        private string PathToExtractFolder { get; set; }
        private string RepoOwnerName { get; set; }
        private string _repoName { get; set; }


        internal string SetupPath { get; private set; } = string.Empty;
        internal string SetupFileName { get; private set; } = string.Empty;
        internal string ErrorMessage { get; private set; } = string.Empty;

        /// <summary>
        /// Checks if given path is valid windows path, has ".exe" extension and word "setup" in name
        /// </summary>
        /// <param name="pathToSetup"></param>
        /// <returns>true if is setup file, else false</returns>
        internal static bool IsCorrectSetupPath(string pathToSetup)
        {
            try
            {
                Validator.GetCorrectWindowsPath(pathToSetup);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return IsSetupFile(Path.GetFileName(pathToSetup));
        }

        /// <summary>
        /// Try get path to setup from "extractFolder/-RepoOwnerName-RepoName/setup.exe"
        /// </summary>
        private void TryGetSetupPath()
        {
            string folderName = GetSetupContainingFolderName(PathToExtractFolder);

            if (folderName is null) return;
            
            string setupFolderPath = Path.Combine(PathToExtractFolder, folderName);
            string setupName = GetSetupFileName(setupFolderPath);

            if (setupName is null) return;

            SetupPath = Path.Combine(setupFolderPath, setupName);
            SetupFileName = setupName;
        }

        /// <summary>
        /// Get name of folder containing setup file
        /// </summary>
        /// <param name="pathToExtractFolder"></param>
        /// <returns>Name of folder containing setup file or null if not found</returns>
        public string? GetSetupContainingFolderName(string pathToExtractFolder)
        {
            //string folderPath = Validator.GetCorrectFolderPath(pathToExtractedFolder);
            List<string> folders = new();

            try
            {
                folders = Directory.EnumerateDirectories(pathToExtractFolder).ToList<string>();
            }
            catch (Exception ex)
            {
                ErrorMessage = AppError.Text(ex.Message);
                return null;
            }

            foreach (string folder in GetFileNames(folders))
            {
                if (IsExtractedFolderMatch(folder)) return folder;
            }

            ErrorMessage = AppError.Text("Could not find matching folder from given path", PathToExtractFolder);
            return null;
        }

        /// <summary>
        /// Get name of setup file
        /// </summary>
        /// <param name="pathToSetupFolder"></param>
        /// <returns>Name of setup file or null if not found</returns>
        private string? GetSetupFileName(string pathToSetupFolder)
        {
            List<string> files;

            try
            {
                files = Directory.EnumerateFiles(pathToSetupFolder).ToList<string>();
            }
            catch (Exception ex)
            {
                ErrorMessage = AppError.Text(ex.Message);
                return null;
            }

            foreach (string file in GetFileNames(files))
            {
                if (IsSetupFile(file)) return file;
            }

            ErrorMessage = AppError.Text($"Could not find setup file from {Path.Combine(pathToSetupFolder)}");
            return null;
        }

        /// <summary>
        /// Return all file names from paths
        /// </summary>
        /// <param name="paths"></param>
        /// <returns>List<string> file names taken from paths</returns>
        private static List<string>? GetFileNames(List<string> paths)
        {
            if (paths is null) return null;
            return paths.Select(x => Path.GetFileName(x)).ToList<string>();
        }

        /// <summary>
        /// Check if folder name includes "repoOwnerName-RepoName-" where repoOwnerName and RepoName are from your repository
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns>true if is right format, else false</returns>
        private bool IsExtractedFolderMatch(string folderName)
        {
            return folderName.Trim().StartsWith($"{RepoOwnerName}-{_repoName}-");
        }

        /// <summary>
        /// Check if filename contains word setup and is .exe file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>true if contains both, else false</returns>
        private static bool IsSetupFile(string fileName)
        {
            bool isZip = (Path.GetExtension(fileName) is ".exe");
            bool isSetup = (fileName.ToLower().Contains("setup"));
            return (isZip && isSetup);
        }

    }
}
