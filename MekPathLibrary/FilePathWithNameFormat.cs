using System;
using System.IO;

namespace MekPathLibrary
{
    public class FilePathWithNameFormat : FilePath
    {
        public FilePathWithNameFormat(){ }
        public FilePathWithNameFormat(string path) : base(path) { }
        public FilePathWithNameFormat(string path, string requiredName) 
        {
            RequiredFileName = requiredName;
            FullPath = path;
        }
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
            protected set
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
