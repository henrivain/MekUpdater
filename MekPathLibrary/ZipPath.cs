/// Copyright 2021 Henri Vainio 
namespace MekPathLibrary
{
    public class ZipPath : FilePath
    {
        /// <summary>
        /// Initialize new ZipPath 
        /// </summary>
        public ZipPath() { }

        /// <summary>
        /// Initialize new ZipPath where given file is situated in given folder path. 
        /// If has no extension or wrong extension, changes to or adds ".zip".
        /// If ends in \ or / (directory separator) adds file.zip in the end.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException"></exception>
        public ZipPath(string path) : base(path) { }

        public override string FileExtension => ".zip";

        /// <summary>
        /// Validated full ".zip" path OR givenPath\file.zip if givenPath ends in directory separator
        /// OR if not valid throws ArgumentException
        /// </summary>
        /// <exception cref="System.ArgumentException"></exception>
        public override string FullPath
        {
            get => base.FullPath; 
            protected set => base.FullPath = value;
        }
    }
}
