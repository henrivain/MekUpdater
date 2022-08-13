/// Copyright 2021 Henri Vainio 
using System;

namespace MekPathLibraryTests.Exceptions
{
    /// <summary>
    /// The exception that is thrown when updater was not able to  
    /// extract zip file from given path
    /// </summary>
    public class ZipExtractionException : UpdateFailedException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// MekUpdater.Exceptions.ZipExtractionException class
        /// </summary>
        public ZipExtractionException() { }

        /// <summary>
        /// Initializes a new instance of the 
        /// System.ZipExtractionException class with a specified
        /// error message.
        /// </summary>
        /// <param name="message"></param>
        public ZipExtractionException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the 
        /// System.ZipExtractionException class with a specified
        /// error message and a reference to the inner exception 
        /// that is the cause of this.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="inner"></param>
        public ZipExtractionException(string Message, Exception inner) : base(Message, inner) { }
    }
}
