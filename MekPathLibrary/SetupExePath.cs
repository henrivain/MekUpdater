using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MekPathLibrary
{
    public class SetupExePath : FilePathWithNameFormat
    {
        public SetupExePath()
        {

        }

        public SetupExePath(string setupPath) : base(setupPath) { }

        public override string FileExtension  => ".exe";
        public override string RequiredFileName => "setup";
        public override bool MustBeFullMatch => false;
        public override bool IsCaseSensitive => false;
        public override string FullPath
        {
            get => base.FullPath;
            protected set => base.FullPath = value;
        }
    }
}
