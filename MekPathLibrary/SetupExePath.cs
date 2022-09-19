using System;

namespace MekPathLibrary
{
    /// <summary>
    /// Windows path to file with specific name format 
    /// ('setup' is required as a part of file name and must have extension '.exe')
    /// </summary>
    public class SetupExePath : FilePathWithNameFormat
    {
        /// <inheritdoc/>
        public SetupExePath() { }

        /// <summary>
        /// Initialize new path that has .exe as extension and word 'setup' in name
        /// </summary>
        /// <param name="setupPath">path to be used</param>
        /// <exception cref="ArgumentException">thrown if path is invalid</exception>
        public SetupExePath(string setupPath) : base(setupPath) { }

        /// <inheritdoc/>
        public override string FileExtension  => ".exe";

        /// <inheritdoc/>
        public override string RequiredFileName => "setup";

        /// <inheritdoc/>
        public override bool MustBeFullMatch => false;

        /// <inheritdoc/>
        public override bool IsCaseSensitive => false;

        /// <inheritdoc/>
        public override string FullPath
        {
            get => base.FullPath;
            protected internal set => base.FullPath = value;
        }
    }
}
