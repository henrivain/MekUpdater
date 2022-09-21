// Copyright 2021 Henri Vainio 

using System.Diagnostics;
using MekUpdater.UpdateRunner;

namespace MekUpdater.InstallUpdates
{

    /// <summary>
    /// Tool for starting setup.exe from subfolder of ExtractPath
    /// </summary>
    internal class SetupLauncher
    {
        /// <summary>
        /// Initialize new Setup launcher with given path
        /// </summary>
        /// <param name="setupPath"></param>
        /// <exception cref="ArgumentNullException">thrown if path does not have value </exception>
        internal SetupLauncher(SetupExePath setupPath)
        {
            if (setupPath.HasValue is false) throw new ArgumentNullException(nameof(setupPath));
            SetupPath = setupPath;
        }

        /// <summary>
        /// Initialize new Setup launcher with given path and boolean value representing if running as Admin is required
        /// </summary>
        /// <param name="setupPath"></param>
        /// <param name="requireAdmin"></param>
        /// <exception cref="ArgumentNullException">thrown if path does not have value </exception>
        internal SetupLauncher(SetupExePath setupPath, bool requireAdmin) : this(setupPath)
        {
            RequireAdmin = requireAdmin;
        }

        public SetupExePath SetupPath { get; }

        public bool RequireAdmin { get; } = false;

        internal StartSetupResult StartSetup()
        {
            try
            {
                TryLaunchProcess();
                return new(true)
                {
                    SetupExePath = SetupPath
                };
            }
            catch (Exception ex)
            {
                UpdateMsg msg = GetExceptionReason(ex);
                return new(false)
                {
                    UpdateMsg = msg,
                    Message = $"Launching setup.exe failed because of {msg}: {ex.Message}",
                    SetupExePath = SetupPath
                };
            }
        }

        /// <summary>
        /// Get reason while exception was thrown
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>(FailState.Launching, ExceptionReason) or if exception not known (FailState.Launching, SetupPathMsg.Unknown)</returns>
        private static UpdateMsg GetExceptionReason(Exception ex)
        {
            return ex switch
            {
                ObjectDisposedException => UpdateMsg.ObjectDisposed,
                InvalidOperationException => UpdateMsg.NoFileName,
                System.ComponentModel.Win32Exception => UpdateMsg.ErrorWhileOpening,
                PlatformNotSupportedException => UpdateMsg.NoShellSupport,
                _ => UpdateMsg.Unknown
            };
        }

        /// <summary>
        /// Try launch application from specified path
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        private void TryLaunchProcess()
        {
            Process process = new()
            {
                StartInfo = new()
                {
                    FileName = SetupPath.FullPath,
                    UseShellExecute = false,
                    Verb = RequireAdmin ? "runas" : string.Empty
                }
            };
            process.Start();
        }
    }
}
