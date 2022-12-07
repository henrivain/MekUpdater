namespace MekUpdater.Exceptions;

/// <summary>
/// The exception that is thrown when FileHandler file name is not initialized. 
/// (Only possible for inherited classes.)
/// </summary>
public class FileNameNotInitializedException : Exception
{
    /// <summary>
    /// Initializes a new FileNameNotInitializedException
    /// </summary>
    public FileNameNotInitializedException() { }

    /// <summary>
    /// Initializes a new FileNameNotInitializedException with given message.
    /// error message.
    /// </summary>
    /// <param name="message"></param>
    public FileNameNotInitializedException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new FileNameNotInitializedException with message and inner exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public FileNameNotInitializedException(string message, Exception inner) : base(message, inner) { }
}
