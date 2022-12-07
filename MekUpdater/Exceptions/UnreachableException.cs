namespace MekUpdater.Exceptions;


/// <summary>
/// The exception that is thrown when some action should not have happened. 
/// This exception should be impossible to be reach.
/// </summary>
public class UnreachableException : Exception
{
    /// <summary>
    /// Initializes a new instance of the UnreachableException.
    /// </summary>
    public UnreachableException() { }

    /// <summary>
    /// Initializes a new instance of the UnreachableException. Takes error message as argument.
    /// </summary>
    /// <param name="message"></param>
    public UnreachableException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the UnreachableException.
    /// Takes error message and a reference to the inner exception that is the cause of this.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public UnreachableException(string message, Exception inner) : base(message, inner) { }
}



