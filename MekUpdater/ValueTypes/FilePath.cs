/// Copyright 2021 Henri Vainio 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekUpdater.ValueTypes
{
    public class FilePath : LocalPath, IFilePath
    {
        public FilePath() { }

        public FilePath(string path) 
        {
            FullPath = path;
        }

        public string FileName { get; protected set; } = string.Empty;

        public FolderPath FolderPath { get; protected set; } = new();

        public override string FullPath 
        { 
            get => base.FullPath; 
            set 
            {
                string path = FromString(value);
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
        public virtual string FileExtension => ".";

        /// <summary>
        /// File name that is used, if path ends in directory
        /// </summary>
        public string DefaultFileName => $"file{FileExtension}";


        /// <summary>
        /// Get full path, add extension and validate path
        /// </summary>
        /// <param name="path"></param>
        /// <returns>valid path or string.Empty if not valid</returns>
        protected string FromString(string path)
        {
            string full = AddExtension(GetFullPath(path));
            return (string.IsNullOrWhiteSpace(full)) ? string.Empty : full;
        }


        /// <summary>
        /// Add extension to file if needed
        /// </summary>
        /// <param name="path"></param>
        /// <returns>path with extension valid, else string.Empty</returns>
        protected string AddExtension(string path)
        {
            if (Path.EndsInDirectorySeparator(path)) return Path.Combine(path, DefaultFileName);
            return Path.ChangeExtension(path, FileExtension) ?? string.Empty;
        }

        /// <summary>
        /// Checks weather current inctance is valid folder path
        /// </summary>
        /// <returns>true if is valid, else false</returns>
        public override bool IsValid()
        {
            return IsValidFilePath(this) && FullPath.Contains(FileExtension);
        }
    }
}
