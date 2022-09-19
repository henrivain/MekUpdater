/// Copyright 2021 Henri Vainio 
namespace MekPathLibraryTests;

public class LocalPathTests
{

    [Theory]
    [InlineData(@"C:\Users\user\Downloads\temp\update.zip")]
    [InlineData(@"C:\Users\update.txt")]
    [InlineData(@"C:\update.exe")]
    [InlineData(@"C:\Users\user\Downloads\temp\")]
    [InlineData(@"C:\temp\text.txt\gg.txt")]
    public void New_ValidPath_ShouldReturn_Input(string input)
    {
        // Arrange & Act
        LocalPath localPath = new(input);

        // Assert
        Assert.Equal(input, localPath.FullPath);
        Assert.Equal(input, localPath.ToString());
    }

    [Theory]
    [InlineData(@"./FooterItem")]
    [InlineData(@"./FooterItem/gg.txt")]
    public void New_RelativePath_ShouldNotReturn_Input_OrEmpty(string input)
    {
        // Arrange & Act
        LocalPath localPath = new(input);

        // Assert
        Assert.NotEqual(input, localPath.FullPath);
        Assert.NotEqual(input, localPath.ToString());
        Assert.NotEmpty(localPath.ToString());
        Assert.NotEmpty(localPath.FullPath);
    }

    [Theory]
    [InlineData(@"./FooterItem/g:g.txt")]   // colon mid path
    [InlineData(@"./Foote<?>rItem/gg.txt")]   // question mark
    public void New_WithInvalidPath_ShouldThrow_ArgumentException(string input)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            new LocalPath(input);
        });
    }
}
