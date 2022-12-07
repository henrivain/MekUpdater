// Copyright 2021 Henri Vainio 
using System.IO.Compression;
using MekUpdater.UpdateRunner;

namespace MekUpdater.InstallUpdates;

internal class ZipExtracter
{
    internal ZipExtracter(ZipPath zipPath, FolderPath extractPath)
    {
        ZipPath = zipPath;
        ExtractPath = extractPath;
    }
    ZipPath ZipPath { get; }
    FolderPath ExtractPath { get; }

    internal async Task<ZipExtractionResult> ExtractAsync()
    {
        try
        {
            if (Directory.Exists(ExtractPath.FullPath) is false)
            {
                Directory.CreateDirectory(ExtractPath.FullPath);
            }
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(ZipPath.FullPath, ExtractPath.FullPath, true);
            });
            return new(true);
        }
        catch (Exception ex)
        {
            var msg = GetExceptionReason(ex);

            return new(false)
            {
                Message = "Extracting zip" +
                          $"\n\tfrom {ZipPath}" +
                          $"\n\tto {ExtractPath}" +
                          $"\n\tfailed because of {msg}: {ex.Message}",
                UpdateMsg = msg,
                StackTrace = ex.StackTrace
            };

        }
    }

    /// <summary>
    /// Get matching error code for ZipPath.ExtractToDirectory()
    /// </summary>
    /// <param name="ex"></param>
    /// <returns>ErroMsg with case matching error code or ErroMsg.Other if Exception not found</returns>
    private static UpdateMsg GetExceptionReason(Exception ex)
    {
        return ex switch
        {
            ArgumentException => UpdateMsg.InvalidPathChars,
            PathTooLongException => UpdateMsg.PathTooLong,
            DirectoryNotFoundException => UpdateMsg.FileNotFound,
            FileNotFoundException => UpdateMsg.FileNotFound,
            IOException => UpdateMsg.FileAlreadyExistOrBadName,
            UnauthorizedAccessException => UpdateMsg.NoPermission,
            NotSupportedException => UpdateMsg.BadPathFormat,
            InvalidDataException => UpdateMsg.UnSupportedDataType,
            _ => UpdateMsg.Unknown
        };
    }
}
