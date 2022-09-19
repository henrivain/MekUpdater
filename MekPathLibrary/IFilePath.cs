// Copyright 2022 Henri Vainio 

namespace MekPathLibrary
{
    /// <summary>
    /// Interface for file path implementations
    /// </summary>
    public interface IFilePath : ILocalPath
    {
        /// <summary>
        /// Filename without extension
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Path to folder, where file is situated
        /// </summary>
        FolderPath FolderPath { get; }

        /// <summary>
        /// Extension of the file
        /// </summary>
        static string FileExtension { get; } = string.Empty;
    }
}
