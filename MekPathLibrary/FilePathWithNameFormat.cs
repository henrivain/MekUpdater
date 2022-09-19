using System;
using System.IO;

namespace MekPathLibrary
{
    /// <summary>
    /// Windows path to file with specific name format (like always includes word 'setup')
    /// </summary>
    public class FilePathWithNameFormat : FilePath
    {
        
        /// <inheritdoc/>
        public FilePathWithNameFormat(){ }

        /// <summary>
        /// Initialize new path that has specific name format
        /// </summary>
        /// <param name="path">path to be used</param>
        /// <exception cref="ArgumentException">thrown if path is invalid</exception>
        public FilePathWithNameFormat(string path) : base(path) { }

        /// <summary>
        /// Initialize new path that must have requiredName in the file name
        /// </summary>
        /// <param name="path">path to be used</param>
        /// <param name="requiredName">substring that must be found form file name</param>
        /// <exception cref="ArgumentException">thrown if path is invalid</exception>
        public FilePathWithNameFormat(string path, string requiredName) 
        {
            RequiredFileName = requiredName;
            FullPath = path;
        }

        /// <summary>
        /// Initialize new path that must have requiredName in the file name
        /// </summary>
        /// <param name="path">path to be used</param>
        /// <param name="requiredName">substring that must be found form file name</param>
        /// <param name="mustBeFullMatch">should file name be required name or can it be only part of name</param>
        /// <param name="isCaseSensitive">is matching capital letter with lower letter acceptable</param>
        /// <exception cref="ArgumentException">thrown if path is invalid</exception>
        public FilePathWithNameFormat(
            string path, 
            string requiredName, 
            bool mustBeFullMatch = false, 
            bool isCaseSensitive = false)
        {
            
            IsCaseSensitive = isCaseSensitive;
            RequiredFileName = IsCaseSensitive ? 
                requiredName : requiredName.ToLower() 
                    ?? string.Empty;
            MustBeFullMatch = mustBeFullMatch;
            FullPath = path;
        }

        /// <summary>
        /// Name must contain this substring to be valid
        /// </summary>
        public virtual string RequiredFileName { get; } = string.Empty;

        /// <summary>
        /// Should file name be the same as RequiredFileName?
        /// </summary>
        public virtual bool MustBeFullMatch { get; } = false;

        /// <summary>
        /// Is file name validation case sensitive, 
        /// if false names 'myFile' and 'MyFiLe' have match and name is valid
        /// </summary>
        public virtual bool IsCaseSensitive { get; } = false;

        /// <summary>
        /// Gets full path and validates that the path is valid, has given file extension
        /// and name is in given format (if MustBeFullMatch is true, name should match format)
        /// </summary>
        public override string FullPath
        {
            get => base.FullPath;
            protected internal set
            {
                FilePath validated = new FilePath(value);
                if (HasRequiredName(validated))
                {
                    base.FullPath = validated.ToString();
                    return;
                }
                throw new ArgumentException($"filename of path '{value}' " +
                $"{(MustBeFullMatch ? "is not" : "does not have")} " +
                $"required name format '{RequiredFileName}'");
            }
        }
        private bool HasRequiredName(FilePath validated)
        {
            string fileName = Path.GetFileNameWithoutExtension(validated.FullPath);
            if (IsCaseSensitive is false) fileName = fileName.ToLower();
            if (MustBeFullMatch)
            {
                return fileName == RequiredFileName;
            }
            return fileName.Contains(RequiredFileName);
        }
    }
}
