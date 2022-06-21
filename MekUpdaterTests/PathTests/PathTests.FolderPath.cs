/// Copyright 2021 Henri Vainio 
using MekUpdater.ValueTypes;

namespace MekUpdater.Tests;

public partial class PathTests
{
    [TestMethod]
    public void FolderPath_Normal()
    {
        string formedValue = new FolderPath(FolderPath).FullPath;
        Assert.AreEqual(formedValue, FolderPath);
    }

    [TestMethod]
    public void FolderPath_FromRelative()
    {
        string formedValue = new FolderPath(RelativeFolderPath).FullPath;
        // returns path to assemblyLocation\FooterItem
        Assert.AreNotEqual(formedValue, RelativeFolderPath);
    }

    [TestMethod]
    public void FolderPath_BadFolderPath()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            FolderPath path = new FolderPath(ZipPath);

        });
    }



}
