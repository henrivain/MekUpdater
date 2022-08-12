
using MekPathLibrary;
/// Copyright 2021 Henri Vainio 
namespace MekUpdater.Tests;

public partial class PathTests
{

    [TestMethod]
    public void LocalPath_Zip()
    {
        string formedValue = new LocalPath(ZipPath).FullPath;
        Assert.AreEqual(ZipPath, formedValue);
    }

    [TestMethod]
    public void LocalPath_Txt()
    {
        string formedValue = new LocalPath(TxtPath).FullPath;
        Assert.AreEqual(TxtPath, formedValue);
    }

    [TestMethod]
    public void LocalPath_FileNameMid()
    {
        string formedValue = new LocalPath(FileNameMidPath).FullPath;
        Assert.AreEqual(FileNameMidPath, formedValue);
    }

    [TestMethod]
    public void LocalPath_Folder()
    {
        string formedValue = new LocalPath(FolderPath).FullPath;
        Assert.AreEqual(FolderPath, formedValue);
    }

    [TestMethod]
    public void LocalPath_NoDrive()
    {
        string formedValue = new LocalPath(NodrivePath).FullPath;
        Assert.AreNotEqual(NodrivePath, formedValue);
    }

    [TestMethod]
    public void LocalPath_Relative()
    {
        string formedValue = new LocalPath(RelativeFolderPath).FullPath;
        Assert.AreNotEqual(RelativeFolderPath, formedValue);
    }
    
    [TestMethod]
    public void LocalPath_RelativeTxt()
    {
        string formedValue = new LocalPath(RelativeTxtPath).FullPath;
        Assert.AreNotEqual(RelativeTxtPath, formedValue);
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
