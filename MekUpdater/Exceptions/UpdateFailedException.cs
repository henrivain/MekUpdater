// Copyright 2021 Henri Vainio 

namespace MekUpdater.Exceptions;

/// <summary>
/// The exception that is thrown when update downloader fails execute 
/// one of the steps in update process
/// </summary>
public class UpdateFailedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the MekUpdater.Exceptions.UpdateFailedException class
    /// </summary>
    public UpdateFailedException() { }

    /// <summary>
    /// Initializes a new instance of the System.UpdateFailedException class with a specified
    /// error message.
    /// </summary>
    /// <param name="message"></param>
    public UpdateFailedException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the System.UpdateFailedException class with a specified
    /// error message and a reference to the inner exception that is the cause of this.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public UpdateFailedException(string message, Exception inner) : base(message, inner) { }
}
