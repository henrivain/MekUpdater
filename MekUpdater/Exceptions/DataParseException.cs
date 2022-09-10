// Copyright 2021 Henri Vainio 
namespace MekUpdater.Exceptions;

/// <summary>
/// The exception that is thrown when update downloader fails to 
/// download or copy update files to machine
/// </summary>
public class DataParseException : UpdateFailedException
{
    /// <summary>
    /// Initializes a new instance of the MekUpdater.Exceptions.DataParseException class
    /// </summary>
    public DataParseException() { }

    /// <summary>
    /// Initializes a new instance of the System.ZipExtractionException class with a specified
    /// error message.
    /// </summary>
    /// <param name="message"></param>
    public DataParseException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the System.ZipExtractionException class with a specified
    /// error message and a reference to the inner exception that is the cause of this.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public DataParseException(string message, Exception inner) : base(message, inner) { }
}
