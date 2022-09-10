/// Copyright 2021 Henri Vainio 
namespace MekPathLibraryTests;

public class ZipPathTests
{
    [Fact]
    public void New_WithValid_ShouldReturn_Input()
    {
        // Arrange
        var input = @"C:\Users\user\Downloads\temp\update.zip";

        // Act
        ZipPath zipPath = new(input);

        // Assert
        Assert.Equal(input, zipPath.FullPath);
        Assert.Equal(input, zipPath.ToString());
    }

    [Fact]
    public void New_WithWrongExtension_ShouldChange_Extension()
    {
        // Arrange
        var input = @"C:\Users\user.txt";
        var shouldMatch = @"C:\Users\user.zip";

        // Act
        ZipPath zipPath = new(input);

        // Assert
        Assert.Equal(shouldMatch, zipPath.FullPath);
        Assert.Equal(shouldMatch, zipPath.ToString());
    }

    [Fact]
    public void New_WithFolderPath_ShouldAdd_File_Zip()
    {
        // Arrange
        var input = @"C:\Users\temp\";
        var shouldMatch = @"C:\Users\temp\file.zip";

        // Act
        ZipPath zipPath = new(input);

        // Assert
        Assert.Equal(shouldMatch, zipPath.FullPath);
        Assert.Equal(shouldMatch, zipPath.ToString());
    }

    [Theory]
    [InlineData(@"./FooterItem/g:g.txt")]
    [InlineData(@"./Foote<?>rItem/gg.txt")]
    public void New_WithInvalidPath_ShouldThrow_ArgumentException(string input)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            new ZipPath(input);
        });
    }

    [Theory]
    [InlineData(@"./FooterItem")]
    [InlineData(@"./FooterItem/gg.txt")]
    [InlineData(@"./FooterItem/gg.zip")]
    public void New_WithRelativePath_ShouldNotReturn_Input(string input)
    {
        // Arrange & Act
        ZipPath zipPath = new(input);

        // Assert
        Assert.NotEqual(input, zipPath.FullPath);
        Assert.NotEqual(input, zipPath.ToString());
    }
}
