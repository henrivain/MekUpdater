// Copyright 2022 Henri Vainio 
using System;
using System.IO;

namespace MekPathLibrary
{
    /// <summary>
    /// Path to file that can exist in windows device
    /// </summary>
    public class FilePath : LocalPath, IFilePath
    {
        /// <summary>
        /// Initilize new path to file that can exist in windows device, will be initialized as empty
        /// </summary>
        public FilePath() { }

        /// <summary>
        /// Initilize new path to file that can exist in windows device using given path
        /// </summary>
        /// <param name="path"></param>
        public FilePath(string path)
        {
            FullPath = path;
        }

        /// <summary>
        /// Name of the file without extension
        /// </summary>
        public string FileName { get; protected set; } = string.Empty;

        /// <summary>
        /// Path to folder where file is situated
        /// </summary>
        public FolderPath FolderPath { get; protected set; } = new FolderPath();

        /// <inheritdoc/>
        public override string FullPath
        {
            get => base.FullPath;
            protected set
            {
                var path = FromString(value);
                if (string.IsNullOrWhiteSpace(path))
                    throw new ArgumentException($"Given path is not valid ; was given {value}");
                base.FullPath = path;

                FileName = Path.GetFileNameWithoutExtension(path);
                FolderPath = new FolderPath(path);
            }
        }

        /// <summary>
        /// Default file extension for this type of file
        /// </summary>
        public virtual string FileExtension { get; private set; } = ".";

        /// <summary>
        /// File name that is used, if path ends in directory
        /// </summary>
        public string DefaultFileName => $"file{FileExtension}";

        /// <inheritdoc/>
        public override bool PathExist => File.Exists(FullPath);

        /// <summary>
        /// Checks weather current inctance is valid folder path
        /// </summary>
        /// <returns>true if is valid, else false</returns>
        public override bool IsValid()
        {
            return IsValidFilePath(this) && FullPath.Contains(FileExtension);
        }

        /// <summary>
        /// Get full path, add extension and validate path
        /// </summary>
        /// <param name="path"></param>
        /// <returns>valid path or string.Empty if not valid</returns>
        protected string FromString(string path)
        {
            // Check if extension is not already defined by child class
            if (FileExtension is ".")
            {
                SetFileExtensionProperty(path);
                return path;
            }
            var full = AddExtension(GetFullPath(path));
            return string.IsNullOrWhiteSpace(full) ? string.Empty : full;
        }

        /// <summary>
        /// Add extension to file if needed
        /// </summary>
        /// <param name="path"></param>
        /// <returns>path with extension valid, else string.Empty</returns>
        protected string AddExtension(string path)
        {
            if (path.EndsWith(Path.DirectorySeparatorChar) || path.EndsWith(Path.AltDirectorySeparatorChar)) return Path.Combine(path, DefaultFileName);
            return Path.ChangeExtension(path, FileExtension) ?? string.Empty;
        }

        /// <summary>
        /// Set FileExtension property or add "." in the end of path to 
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException">thrown when path does not end in file extension</exception>
        private void SetFileExtensionProperty(string path)
        {
            var fileExtension = Path.GetExtension(path) ?? string.Empty;
            if (string.IsNullOrEmpty(fileExtension))
                throw new ArgumentException($"{nameof(FilePath)} must end in file extension ; was given {path}");
            FileExtension = fileExtension;
        }
    }
}
