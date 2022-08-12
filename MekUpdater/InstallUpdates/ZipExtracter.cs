/// Copyright 2021 Henri Vainio 
using System.IO.Compression;
using MekPathLibrary;
using static MekUpdater.UpdateDownloadInfo;

namespace MekUpdater.InstallUpdates
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
        /// Initializes new ZipExtracter using information from given UpdateDownloadInfo
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

        /// <summary>
        /// Extract zipfile to path given in UpdateDownloadInfo Info
        /// </summary>
        /// <returns>awaitable Task</returns>
        internal async Task Extract()
        {
            Info.Extracting();
            try
            {
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(Info.ZipFilePath.ToString(), Info.ExtractPath.ToString(), true);
                }); 
                Info.ExtractionCompleted();
            }
            catch (Exception ex)
            {
                Info.Error = (FailState.Extracting, GetExceptionReason(ex));
                Info.ExtractionFailed();
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
