using System;
using System.IO;

namespace MekPathLibrary.Extensions
{
    /// <summary>
    /// Class to store extensions for folder path
    /// </summary>
    public static class FolderPathExtensions
    {
        /// <summary>
        /// Try append string path to folder path.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="path"></param>
        /// <param name="result"></param>
        /// <returns>true if success, otherwise false.</returns>
        public static bool TryAppend(this FolderPath instance, string path, out FolderPath? result)
        {
            result = null;
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }
            if (path.EndsInDirectorySeparator() is false)
            {
                path += Path.DirectorySeparatorChar;
            }
            try
            {
                result = new FolderPath(instance.FullPath + path);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}


