/// Copyright 2021 Henri Vainio 

namespace MekPathLibraryTests;

public class FolderPathTests
{
    [Fact]
    public void New_WithFolderPath_ShouldReturn_Input()
    {
        // Arrange
        var input = @"C:\Users\user\Downloads\temp\";

        // Act
        FolderPath folderPath = new(input);

        // Assert
        Assert.Equal(input, folderPath.FullPath);
        Assert.Equal(input, folderPath.ToString());
    }

    [Fact]
    public void New_WithRelativePath_ShouldNotReturn_Input()
    {
        // Arrange
        var input = @"./FooterItem";

        // Act
        FolderPath folderPath = new(input);

        // Assert
        Assert.NotEqual(input, folderPath.FullPath);
        Assert.NotEqual(input, folderPath.ToString());
    }

    [Fact]
    public void New_ShouldRemove_FileName()
    {
        // Arrange
        var input = @"C:\Users\user\Downloads\temp\update.zip";
        var shouldMatch = @"C:\Users\user\Downloads\temp\";

        // Act
        FolderPath folderPath = new(input);

        // Assert
        Assert.Equal(shouldMatch, folderPath.FullPath);
        Assert.Equal(shouldMatch, folderPath.ToString());
    }

    [Theory]
    [InlineData(@"C:\Users\user\", @"myapp\", @"C:\Users\user\myapp\")]     // file names are removed when folder path is formed, so given path should always end in '\'
    [InlineData(@"C:\Users\user.zip", @"newFolder\myapp.txt", @"C:\Users\newFolder\")]
    public void Append_AndRemoveNameAndAppend_ShouldCombine_Paths_AndRemove_FileExtension_FromMiddle(string path, string pathToAppend, string expectedReturn)
    {
        // Arrange
        FolderPath folderPath1 = new(path);
        FolderPath folderPath2 = new(path);

        // Act
        folderPath1.Append(pathToAppend);
        folderPath2.Append(pathToAppend);

        // Assert
        Assert.Equal(expectedReturn, folderPath1.FullPath);
        Assert.Equal(expectedReturn, folderPath2.FullPath);
    }


    [Theory]
    [InlineData(@"C:\Users\user\", @"myapp.zip", @"C:\Users\user\myapp.zip")]     // file names are removed when folder path is formed, so given path should always end in '\'
    [InlineData(@"C:\Users\user.zip", @"newFolder\myapp.zip", @"C:\Users\newFolder\myapp.zip")]
    public void ToFilePath_CanConsturctZipPath(string path, string pathToAppend, string expectedReturn)
    {
        // Arrange
        FolderPath localPath = new(path);

        // Act
        var result = localPath.ToFilePath<ZipPath>(pathToAppend);

        // Assert
        Assert.Equal(expectedReturn, result.FullPath);
    }
    
    [Theory]
    [InlineData(@"C:\Users\user\", @"myappsetup.exe", @"C:\Users\user\myappsetup.exe")]
    [InlineData(@"C:\Users\user.zip", @"newFolder\setup.exe", @"C:\Users\newFolder\setup.exe")]
    public void ToFilePath_CanConsturctExe(string path, string pathToAppend, string expectedReturn)
    {
        // Arrange
        FolderPath localPath = new(path);

        // Act
        var result = localPath.ToFilePath<SetupExePath>(pathToAppend);

        // Assert
        Assert.Equal(expectedReturn, result.FullPath);
    }
}
