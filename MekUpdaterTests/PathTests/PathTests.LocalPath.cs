/// Copyright 2021 Henri Vainio 
using MekUpdater.ValueTypes;

namespace MekUpdater.Tests;

public partial class PathTests
{

    [TestMethod]
    public void LocalPath_Zip()
    {
        string formedValue = new LocalPath(ZipPath).FullPath;
        Assert.AreEqual(formedValue, ZipPath);
    }

    [TestMethod]
    public void LocalPath_Txt()
    {
        string formedValue = new LocalPath(TxtPath).FullPath;
        Assert.AreEqual(formedValue, TxtPath);
    }

    [TestMethod]
    public void LocalPath_FileNameMid()
    {
        string formedValue = new LocalPath(FileNameMidPath).FullPath;
        Assert.AreEqual(formedValue, TxtPath);
    }

    [TestMethod]
    public void LocalPath_Folder()
    {
        string formedValue = new LocalPath(FolderPath).FullPath;
        Assert.AreEqual(formedValue, FolderPath);
    }

    [TestMethod]
    public void LocalPath_NoDrive()
    {
        string formedValue = new LocalPath(NodrivePath).FullPath;
        Assert.AreNotEqual(formedValue, NodrivePath);
    }

    [TestMethod]
    public void LocalPath_Relative()
    {
        string formedValue = new LocalPath(RelativeFolderPath).FullPath;
        Assert.AreNotEqual(formedValue, RelativeFolderPath);
    }
    
    [TestMethod]
    public void LocalPath_RelativeTxt()
    {
        string formedValue = new LocalPath(RelativeTxtPath).FullPath;
        Assert.AreNotEqual(formedValue, RelativeTxtPath);
    }
    
    [TestMethod]
    public void LocalPath_BadFileName()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            string formedValue = new LocalPath(InvalidNamePath).FullPath;
        });
    }
    
    [TestMethod]
    public void LocalPath_BadPath()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            string formedValue = new LocalPath(InvalidPath).FullPath;
        });
    }
}
