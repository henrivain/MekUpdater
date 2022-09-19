using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
// Copyright 2022 Henri Vainio 

namespace MekPathLibrary
{
    /// <summary>
    /// Validated path that can exist in windows device
    /// </summary>
    public class LocalPath : ILocalPath
    {
        /// <summary>
        /// Initialize new local path that is currently empty
        /// </summary>
        public LocalPath() { }

        /// <summary>
        /// Initialize new local path with given path
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException"></exception>
        public LocalPath(string path)
        {
            FullPath = path;
        }

        /// <summary>
        /// Initialize new local path, where paths are firstly combined and then validated
        /// </summary>
        /// <param name="paths"></param>
        /// <exception cref="ArgumentException"></exception>
        public LocalPath(params string[] paths)
        {
            FullPath = Path.Combine(
                new string[] { FullPath }
                .Concat(paths)
                .ToArray());
        }

        string _fullPath = string.Empty;

        /// <summary>
        /// Validated full path, if no valid, throws ArgumentException
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public virtual string FullPath
        {
            get => _fullPath;
            protected internal set
            {
                _fullPath = GetFullPath(value);
                if (IsValidWindowsPath(this) is false)
                    throw new ArgumentException($"Given path is not valid ; was given {value}");
            }
        }

        /// <summary>
        /// Check weather instance of object has path value
        /// </summary>
        /// <returns>true if has value (value not string.Empty), else false</returns>
        public virtual bool HasValue => string.IsNullOrEmpty(FullPath) is false;

        /// <summary>
        /// Check if path exist as file or directory in user's device 
        /// </summary>
        public virtual bool PathExist => File.Exists(FullPath) || Directory.Exists(FullPath);

        /// <summary>
        /// Check if instance of object is valid
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid() => IsValidWindowsPath(this);

        /// <summary>
        /// Append given path to FullPath, removes file extension from old FullPath if has one
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException">thrown if combined path is invalid</exception>
        public virtual void Append(string path)
        {
            string fullPath = FullPath;
            if (string.IsNullOrWhiteSpace(path)) return;
            if (string.IsNullOrWhiteSpace(Path.GetExtension(FullPath)) is false)
            {
                fullPath = Path.Combine(
                    Path.GetDirectoryName(FullPath),
                    Path.GetFileNameWithoutExtension(FullPath));
            }
            FullPath = Path.Combine(fullPath, path);
        }

        /// <summary>
        /// Append given path to FullPath, removes file extension and file name from old FullPath if has one
        /// </summary>
        /// <param name="path"></param>
        public virtual void RemoveNameAndAppend(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            FullPath = Path.Combine(Path.GetDirectoryName(FullPath), path);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return FullPath.ToString();
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is LocalPath path &&
                   FullPath == path.FullPath;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(FullPath);
        }

        /// <inheritdoc/>
        public static bool operator ==(LocalPath? left, LocalPath? right)
        {
            if (left is null || right is null) return false;
            return EqualityComparer<LocalPath>.Default.Equals(left, right);
        }
        
        /// <inheritdoc/>
        public static bool operator !=(LocalPath? left, LocalPath? right)
        {
            return !(left == right);
        }




        /// <summary>
        /// Check weather path.FullPath is valid in windows
        /// </summary>
        /// <returns>true if is valid, else false</returns>
        protected static bool IsValidWindowsPath(ILocalPath path)
        {
            if (GetFullPath(path.FullPath) == string.Empty) return false;
            if (string.IsNullOrWhiteSpace(path.FullPath)) return false;
            if (HasInvalidChars(path.FullPath)) return false;
            return Path.IsPathFullyQualified(path.FullPath);
        }

        /// <summary>
        /// Check weather FullPath is valid path to folder (does not have file extension) in windows
        /// </summary>
        /// <returns>true if is valid, else false</returns>
        protected static bool IsValidFolderPath(ILocalPath path)
        {
            if (IsValidWindowsPath(path) is false) return false;
            return string.IsNullOrWhiteSpace(Path.GetExtension(path.FullPath));
        }

        /// <summary>
        /// Check weather FullPath is valid path to file 
        /// in windows (has file extension and name)
        /// </summary>
        /// <returns>true if is valid, else false</returns>
        protected static bool IsValidFilePath(ILocalPath path)
        {
            if (IsValidWindowsPath(path) is false) return false;
            return string.IsNullOrWhiteSpace(Path.GetExtension(path.FullPath)) is false;
        }

        /// <summary>
        /// If path has filename, checks weather it has invalid characters or not
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true if has invalid chars or too long, else false</returns>
        protected static bool HasInvalidChars(string path)
        {
            var fileName = Path.GetFileName(path);
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                return true;

            var invalidChars = new char[] { '|', '<', '>', '"', '*', '?' };
            return path.IndexOfAny(invalidChars) != -1;
        }

        /// <summary>
        /// Get full path from relative, if bad path, returns string.empty. 
        /// If given path is full path, returns it unchanced.
        /// Writes all exceptions in console.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>full path if given path valid or valid relative path, else string.Empty</returns>
        protected static string GetFullPath(string path)
        {
            try
            {
                return Path.GetFullPath(path) ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{path}");
                return string.Empty;
            }
        }
    }
}
