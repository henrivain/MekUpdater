/// Copyright 2021 Henri Vainio 
using MekUpdater.Exceptions;
using MekUpdater.Helpers;
using MekUpdater.ValueTypes;

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
        /// <param name="extractFolder"></param>
        /// <param name="repoOwner"></param>
        /// <param name="repoName"></param>
        internal SetupFilePath(FolderPath extractFolder, string repoOwner, string repoName)
        {
            ExtractFolder = extractFolder;
            RepoOwner = repoOwner;
            _repoName = repoName;
            TryGetSetupPath();
        }

        private FolderPath ExtractFolder { get; set; }
        private string RepoOwner { get; set; }
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
            string? folderName = GetSetupContainingFolderName(ExtractFolder);

            if (folderName is null) return;
            
            FolderPath setupPath = new(Path.Combine(ExtractFolder.ToString(), folderName));
            string? setupName = GetSetupFileName(setupPath);

            if (setupName is null) return;

            SetupPath = Path.Combine(setupPath.ToString(), setupName);
            SetupFileName = setupName;
        }

        /// <summary>
        /// Get name of folder containing setup file
        /// </summary>
        /// <param name="ExtractPath"></param>
        /// <returns>Name of folder containing setup file or null if not found</returns>
        public string? GetSetupContainingFolderName(FolderPath ExtractPath)
        {
            List<string> folders;
            try
            {
                folders = Directory.EnumerateDirectories(ExtractPath.ToString()).ToList();
            }
            catch (Exception ex)
            {
                ErrorMessage = AppError.Text(ex.Message);
                return null;
            }

            foreach (string folder in GetFileNames(folders) ?? Enumerable.Empty<string>())
            {
                if (IsExtractedFolderMatch(folder)) return folder;
            }

            ErrorMessage = AppError.Text("Could not find matching folder from given path", ExtractFolder.ToString());
            return null;
        }

        /// <summary>
        /// Get name of setup file
        /// </summary>
        /// <param name="SetupPath"></param>
        /// <returns>Name of setup file or null if not found</returns>
        private string? GetSetupFileName(FolderPath SetupPath)
        {
            List<string> files;
            try
            {
                files = Directory.EnumerateFiles(SetupPath.ToString()).ToList();
            }
            catch (Exception ex)
            {
                ErrorMessage = AppError.Text(ex.Message);
                return null;
            }

            foreach (string file in GetFileNames(files) ?? Enumerable.Empty<string>())
            {
                if (IsSetupFile(file)) return file;
            }

            ErrorMessage = AppError.Text($"Could not find setup file from {Path.Combine(SetupPath.ToString())}");
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
            return paths.Select(x => Path.GetFileName(x)).ToList();
        }

        /// <summary>
        /// Check if folder name includes "repoOwnerName-RepoName-" where repoOwnerName and RepoName are from your repository
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns>true if is right format, else false</returns>
        private bool IsExtractedFolderMatch(string folderName)
        {
            return folderName.Trim().StartsWith($"{RepoOwner}-{_repoName}-");
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
