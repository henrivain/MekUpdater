/// Copyright 2021 Henri Vainio 
using System.Diagnostics;
using System.IO.Compression;
using MekPathLibrary;
using MekUpdater.InstallUpdates;
using static MekPathLibraryTests.UpdateDownloadInfo;

namespace MekPathLibraryTests.InstallUpdates
{
    internal class ZipExtracter
    {

        /// <summary>
        /// Initializes new ZipExtracter using given path
        /// <para/>Give path to file with zip extension, else throws ArgumentException
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException"></exception>
        internal ZipExtracter(ZipPath path)
        {
            Info.ZipFilePath = path;
        }

        /// <summary>
        /// Initializes new ZipExtracter using information from given ResetDownloadInfo
        /// <para/> 
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException"></exception>
        internal ZipExtracter(UpdateDownloadInfo info)
        {
            Info = info;
        }

        /// <summary>
        /// Information about update progression
        /// </summary>
        internal UpdateDownloadInfo Info = new();

        internal async Task<ZipExtractionResult> ExtractAsync()
        {
            Info.Extracting();
            try
            {
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(Info.ZipFilePath.ToString(), Info.ExtractPath.ToString(), true);
                }); 
                Info.ExtractionCompleted();
                return new(true);
            }
            catch (Exception ex)
            {
                ErrorMsg msg = GetExceptionReason(ex);

                Info.Error = (FailState.Extracting, msg);
                Info.ExtractionFailed();
                return new(false)
                {
                    Message = "Extracting zip" +
                              $"\n\tfrom {Info.ZipFilePath.FullPath}" +
                              $"\n\tto {Info.ExtractPath.FullPath}" +
                              $"\n\tfailed because of {msg}: {ex.Message}",
                    ErrorMsg = msg,
                    StackTrace = ex.StackTrace
                };
                
            }
        }

        /// <summary>
        /// Get matching error code for ZipFile.ExtractToDirectory()
        /// </summary>
        /// <param name="ex"></param>
        /// <returns>ErroMsg with case matching error code or ErroMsg.Other if Exception not found</returns>
        private static ErrorMsg GetExceptionReason(Exception ex)
        {
            return ex switch
            {
                ArgumentException => ErrorMsg.InvalidPathChars,
                PathTooLongException => ErrorMsg.PathTooLong,
                DirectoryNotFoundException => ErrorMsg.FileNotFound,
                FileNotFoundException => ErrorMsg.FileNotFound,
                IOException => ErrorMsg.FileAlreadyExistOrBadName,
                UnauthorizedAccessException => ErrorMsg.NoPermission,
                NotSupportedException => ErrorMsg.BadPathFormat,
                InvalidDataException => ErrorMsg.UnSupportedDataType,
                _ => ErrorMsg.Other
            };
        }
    }
}
