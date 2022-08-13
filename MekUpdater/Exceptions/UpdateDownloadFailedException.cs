/// Copyright 2021 Henri Vainio 
using System;

namespace MekPathLibraryTests.Exceptions
{
    /// <summary>
    /// The exception that is thrown when update downloader fails to 
    /// download or copy update files to machine
    /// </summary>
    public class UpdateDownloadFailedException : UpdateFailedException
    {
        /// <summary>
        /// Initializes a new instance of the MekUpdater.Exceptions.UpdateDownloadFailedException class
        /// </summary>
        public UpdateDownloadFailedException() { }

        /// <summary>
        /// Initializes a new instance of the System.ZipExtractionException class with a specified
        /// error message.
        /// </summary>
        /// <param name="message"></param>
        public UpdateDownloadFailedException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the System.ZipExtractionException class with a specified
        /// error message and a reference to the inner exception that is the cause of this.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public UpdateDownloadFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
