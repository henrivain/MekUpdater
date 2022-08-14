/// Copyright 2021 Henri Vainio 

using System.Diagnostics;
using MekUpdater.Exceptions;
using MekUpdater.UpdateRunner;
using static MekUpdater.UpdateDownloadInfo;

namespace MekUpdater.InstallUpdates
{
     
    /// <summary>
    /// Tool for starting setup.exe from subfolder of Info.ExtractPath
    /// </summary>
    internal class SetupLauncher
    {
        internal SetupLauncher(SetupExePath setupPath)
        {
            if (setupPath.HasValue is false) throw new ArgumentNullException(nameof(setupPath));
            Info = setupPath;
        }

        public SetupExePath Info { get; }

        internal StartSetupResult StartSetup()
        {
            try
            {
                TryLaunchProcess(Info.FullPath);
                return new(true);
            }
            catch (Exception ex)
            {
                UpdateMsg msg = GetExceptionReason(ex).msg;
                return new(false)
                {
                    UpdateMsg = msg,
                    Message = $"Launching setup.exe failed because of {msg}: {ex.Message}"
                };
            }
        }
      

        /// <summary>
        /// Get reason while exception was thrown
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>(FailState.Launching, ExceptionReason) or if exception not known (FailState.Launching, SetupPathMsg.Unknown)</returns>
        private static (FailState state, UpdateMsg msg) GetExceptionReason(Exception ex)
        {
            return ex switch
            {
                ObjectDisposedException => (FailState.Launching, UpdateMsg.ObjectDisposed),
                InvalidOperationException => (FailState.Launching, UpdateMsg.NoFileName),
                System.ComponentModel.Win32Exception => (FailState.Launching, UpdateMsg.ErrorWhileOpening),
                PlatformNotSupportedException => (FailState.Launching, UpdateMsg.NoShellSupport),
                _ => (FailState.Launching, UpdateMsg.Unknown)
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
