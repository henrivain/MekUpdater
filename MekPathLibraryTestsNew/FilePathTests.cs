/// Copyright 2021 Henri Vainio 
using MekPathLibrary;

namespace MekPathLibraryTests;

public class FilePathTests
{
    [Fact]
    public void New_NoExtension_shouldThrow_ArgumentException()
    {
        // Arrange
        string path = @"C:\Users\user\Downloads\temp\";


        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            new FilePath(path);
        });
    }
    
    
    [Theory]
    [InlineData(@"C:\Users\user\Downloads\temp\update.txt")]
    [InlineData(@"C:\temp\update.exe")]
    [InlineData(@"C:\Users\user\Downloads\update.myFileExtension")]
    public void FullPath_WithValidPath_ShouldReturn_Input(string input)
    {
        // Arrange & Act
        FilePath path = new FilePath(input);

        // Assert
        Assert.Equal(input, path.FullPath);
    }

    [Fact]
    public void FileExtension_WithExePath_ShouldReturn_Exe()
    {
        // Arrange
        string path = @"C:\Users\user\Downloads\temp\update.exe";
        string shouldMatch = ".exe";

        // Act
        FilePath filePath = new(path);

        // Assert
        Assert.Equal(shouldMatch, filePath.FileExtension);
    }




    [Theory]
    [InlineData(@"C:\Users\user.gg", @"myapp\gg.txt", @"C:\Users\user\myapp\gg.gg")]    // File extension should not be changed when combining, but removed from middle
    [InlineData(@"C:\Users\user.zip", @"newFolder\myapp.txt", @"C:\Users\user\newFolder\myapp.zip")]
    public void Append_ShouldCombine_Paths_AndConserve_OldExtension(string path, string pathToAppend, string expectedReturn)
    {
        // Arrange
        FilePath localPath = new(path);

        // Act
        localPath.Append(pathToAppend);

        // Assert
        Assert.Equal(expectedReturn, localPath.FullPath);
    }

}
