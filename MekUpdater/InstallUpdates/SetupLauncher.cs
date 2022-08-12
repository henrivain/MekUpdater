/// Copyright 2021 Henri Vainio 

using System.Diagnostics;
using MekUpdater.Exceptions;
using static MekUpdater.UpdateDownloadInfo;

namespace MekUpdater.InstallUpdates
{
    /// <summary>
    /// Tool for starting setup.exe from subfolder of Info.ExtractPath
    /// </summary>
    internal class SetupLauncher
    {
        /// <summary>
        /// Initialize new SetupLauncher to start setup.exe from extracted folder
        /// </summary>
        internal SetupLauncher(UpdateDownloadInfo info)
        {
            Info = info;
        }

        readonly UpdateDownloadInfo Info = new();

        /// <summary>
        /// Start setup.exe from given path OR calculated path from Info.ExtractPath
        /// </summary>
        /// <param name="pathToSetup"></param>
        /// <returns>UpdateDownloadInfo with updated information</returns>
        internal UpdateDownloadInfo StartSetup(string? pathToSetup = null)
        {
            pathToSetup = ValidateOrCreateSetupPath(pathToSetup);
            TryLaunchSetup(pathToSetup);
            return Info;
        }

        /// <summary>
        /// Tries to launch setup installer and updates info status
        /// </summary>
        /// <param name="pathToSetup"></param>
        private void TryLaunchSetup(string pathToSetup)
        {
            Info.Launching();
            try
            {
                TryLaunchProcess(pathToSetup);
                Info.LaunchingCompleted();
            }
            catch (Exception ex)
            {
                Info.Error = GetTryLaunchExceptionReason(ex);
                Info.LaunchingFailed();
            }
        }

        /// <summary>
        /// Takes pathToSetup as parameter and validates it OR creates new path from extractPath
        /// </summary>
        /// <param name="pathToSetup"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string ValidateOrCreateSetupPath(string? pathToSetup)
        {
            if (pathToSetup is null || SetupFilePath.IsCorrectSetupPath(pathToSetup) is false)
            {
                SetupFilePath setupFilePath = new(Info.ExtractPath, Info.RepoInfo.RepoOwner, Info.RepoInfo.RepoName);
                if (string.IsNullOrEmpty(setupFilePath.SetupPath))
                {
                    throw new ArgumentException(AppError.Text($"Was not able to find path to setup file ; {setupFilePath.ErrorMessage}"));
                }
                pathToSetup = setupFilePath.SetupPath;
            }

            return pathToSetup;
        }

        /// <summary>
        /// Get reason while exception was thrown
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>(FailState.Launching, ExceptionReason) or if exception not known (FailState.Launching, ErrorMsg.Unknown)</returns>
        private static (FailState state, ErrorMsg msg) GetTryLaunchExceptionReason(Exception ex)
        {
            return ex switch
            {
                ObjectDisposedException => (FailState.Launching, ErrorMsg.ObjectDisposed),
                InvalidOperationException => (FailState.Launching, ErrorMsg.NoFileName),
                System.ComponentModel.Win32Exception => (FailState.Launching, ErrorMsg.ErrorWhileOpening),
                PlatformNotSupportedException => (FailState.Launching, ErrorMsg.NoShellSupport),
                _ => (FailState.Launching, ErrorMsg.Unknown)
            };
        }

        /// <summary>
        /// Try launch application from specified path
        /// </summary>
        /// <param name="setupPath"></param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        private static void TryLaunchProcess(string setupPath)
        {
            Process process = new()
            {
                StartInfo = new()
                {
                    FileName = setupPath,
                    UseShellExecute = false,
                    Verb = "runas"
                }
            };
            process.Start();
        }
    }
}
