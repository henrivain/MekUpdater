/// Copyright 2021 Henri Vainio 
using MekPathLibrary;

namespace MekUpdater.Tests
{
    public partial class PathTests
    {
        [TestMethod]
        public void ZipPath_Zip()
        {
            string formedValue = new ZipPath(ZipPath).FullPath;
            Assert.AreEqual(ZipPath, formedValue);
        }

        [TestMethod]
        public void ZipPath_Txt()
        {
            string formedValue = new ZipPath(TxtPath).FullPath;
            Assert.AreEqual(ZipPath, formedValue);  // should change extension
        }

        [TestMethod]
        public void ZipPath_Folder()
        {
            ILocalPath formedValue = new ZipPath(FolderPath);
            Assert.AreEqual(Path.Combine(FolderPath, "file.zip"), formedValue.ToString());
        }

        [TestMethod]
        public void ZipPath_NoDrive()
        {
            string formedValue = new ZipPath(NodrivePath).FullPath;
            Assert.AreNotEqual(NodrivePath, formedValue);
        }

        [TestMethod]
        public void ZipPath_Relative()
        {
            string formedValue = new ZipPath(RelativeFolderPath).FullPath;
            Assert.AreNotEqual(RelativeFolderPath, formedValue);
        }

        [TestMethod]
        public void ZipPath_RelativeTxt()
        {
            string formedValue = new ZipPath(RelativeTxtPath).FullPath;
            Assert.AreNotEqual(RelativeTxtPath, formedValue);
        }

        [TestMethod]
        public void ZipPath_BadFileName()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                string formedValue = new ZipPath(InvalidNamePath).FullPath;
            });
        }

        [TestMethod]
        public void ZipPath_BadPath()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                string formedValue = new ZipPath(InvalidPath).FullPath;
            });
        }
    }
}
