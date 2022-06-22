/// Copyright 2021 Henri Vainio 
using MekUpdater.ValueTypes.PathValues;

namespace MekUpdater.Tests;

public partial class PathTests
{
    [TestMethod]
    public void FilePath_NoExtension()
    {
        Assert.ThrowsException<ArgumentException>(() =>
        {
            FilePath path = new FilePath(FolderPath);
        });
    }
    
    
    [TestMethod]
    public void FilePath_ExeExtension()
    {
        FilePath path = new FilePath(ExePath);
        Assert.AreEqual(".exe", path.FileExtension);
    }
    
    [TestMethod]
    public void FilePath_Exe()
    {
        FilePath path = new FilePath(ExePath);
        Assert.AreEqual(ExePath, path.FullPath);
    }
    
    [TestMethod]
    public void FilePath_Txt()
    {
        FilePath path = new FilePath(TxtPath);
        Assert.AreEqual(TxtPath, path.FullPath);
    }

}
