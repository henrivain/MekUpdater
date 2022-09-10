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
}
