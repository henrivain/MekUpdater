/// Copyright 2021 Henri Vainio 
using MekUpdater.ValueTypes;

namespace MekUpdater.Tests;

public partial class PathTests
{
    [TestMethod]
    public void FolderPath_Normal()
    {
        string formedValue = new FolderPath(FolderPath).FullPath;
        Assert.AreEqual(FolderPath, formedValue);
    }

    [TestMethod]
    public void FolderPath_FromRelative()
    {
        string formedValue = new FolderPath(RelativeFolderPath).FullPath;
        // returns path to assemblyLocation\FooterItem
        Assert.AreNotEqual(RelativeFolderPath, formedValue);
    }

    [TestMethod]
    public void FolderPath_BadFolderPath()
    {
        FolderPath path = new FolderPath(ZipPath);
        Assert.AreEqual(@"C:\Users\user\Downloads\temp", path.ToString());
    }



}
