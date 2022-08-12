namespace MekPathLibrary
{
    /// <summary>
    /// Interface to define how local path value types should be implemented
    /// </summary>
    public interface ILocalPath
    {
        /// <summary>
        /// Value of the ILocalPath
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Check weather ILocalPath instance has FullPath value or not
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        /// Check weather defined path exist or not (file must be also rigth file type: f.zip named folder != f.zip named file)
        /// </summary>
        /// <returns>true if path exist, else false (false also in case that user has no permission regardless of file existence)</returns>
        bool PathExist { get; }

        /// <summary>
        /// Check if value of instance is valid path
        /// </summary>
        /// <returns>true if valid, else false</returns>
        bool IsValid();

        int GetHashCode();

        string ToString();

        bool Equals(object? obj);

    }
}
