using System;
using System.IO;
using System.Linq;
/// Copyright 2021 Henri Vainio 
namespace MekPathLibrary
{
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




        public override string FullPath
        {
            get => base.FullPath;
            protected set
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

    }
}
