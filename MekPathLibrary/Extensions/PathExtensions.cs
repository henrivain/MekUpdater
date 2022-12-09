using System.IO;

namespace MekPathLibrary.Extensions
{
    internal static class PathExtensions
    {
        internal static bool EndsInDirectorySeparator(this string path)
        {
            return path.EndsWith(Path.DirectorySeparatorChar) || path.EndsWith(Path.AltDirectorySeparatorChar);
        }
    }
}
