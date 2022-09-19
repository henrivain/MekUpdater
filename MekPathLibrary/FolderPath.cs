// Copyright 2022 Henri Vainio 
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MekPathLibrary
{
    /// <summary>
    /// Path to folder that can exist in windows device, all file names are removed automatically
    /// </summary>
    public class FolderPath : LocalPath
    {

        /// <summary>
        /// Create new FolderPath    
        /// </summary>
        public FolderPath() { }

        /// <summary>
        /// Create path to path/To/directory/ by removing filename from given path (ending in 
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException"></exception>
        public FolderPath(string path)
        {
            FullPath = path;
        }



        /// <inheritdoc/>
        public override string FullPath
        {
            get => base.FullPath;
            protected internal set
            {
                var path = FromString(value);
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentException($"Given path is not valid ; was given {value}");
                base.FullPath = path;
            }
        }

        /// <summary>
        /// Checks weather current inctance is valid folder path
        /// </summary>
        /// <returns>true if is valid, else false</returns>
        public override bool IsValid()
        {
            return IsValidFolderPath(this);
        }



        /// <summary>
        /// Convert string to right format 
        /// by removing file names and add AltDirectorySeparatorChar at end
        /// </summary>
        /// <param name="path"></param>
        /// <returns>full path to folder if path valid, else string.Empty</returns>
        private static string FromString(string path)
        {
            var full = GetFullPath(TryReadDirectoryPath(path));
            if (full.EndsWith(Path.DirectorySeparatorChar) is false
                && full.EndsWith(Path.AltDirectorySeparatorChar) is false)
            {
                full += Path.DirectorySeparatorChar;
            }
            return string.IsNullOrWhiteSpace(full) ? string.Empty : full;
        }

        /// <summary>
        /// Get path to last directory from given path (remove filename if needed).
        /// Writes all exceptions to console.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>path to directory or string.Empty if path not valid</returns>
        private static string TryReadDirectoryPath(string path)
        {
            if (path.Last() is '/' || path.Last() is '\\') return path; // path is already to folder
            try
            {
                return Path.GetDirectoryName(path) ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{path}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Check weather defined path exist or not (folder must be also rigth file type: f.zip named folder != f.zip named file)
        /// </summary>
        /// <returns>true if path exist, else false (false also in case that user has no permission regardless of path existence)</returns>
        public override bool PathExist => Directory.Exists(FullPath);

        /// <inheritdoc/>
        public override void Append(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            FullPath = Path.Combine(FullPath, path);
        }

        /// <summary>
        /// Combine FullPath with given path
        /// </summary>
        /// <param name="path"></param>
        public override void RemoveNameAndAppend(string path) => Append(path);

        /// <summary>
        /// Use this folder path to create new filepath with pathWithFileName combined.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pathWithFileName"></param>
        /// <exception cref="ArgumentException">thrown if given path with file name does not meet requirements of constructing file path</exception>
        /// <returns>T type file path with full folder path combined with given path</returns>
        public virtual T ToFilePath<T>(string pathWithFileName) where T : FilePath, new()
        {
            string path = Path.Combine(FullPath, pathWithFileName);
            return new T()
            {
                FullPath = path
            };
        }
    }
}
