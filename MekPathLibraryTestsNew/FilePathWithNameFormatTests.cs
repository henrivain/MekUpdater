namespace MekPathLibraryTests;

public class FilePathWithNameFormatTests
{

    [Theory]
    [InlineData(@"C:\file.txt", "file")]
    [InlineData(@"C:\myFile.zip", "file")]
    [InlineData(@"C:\mySetup.zip", "setup")]
    [InlineData(@"C:\setup.zip", "SeTup")]
    [InlineData(@"C:\setup.zip", "setup", true)]
    [InlineData(@"C:\setup.zip", "seTuP", true)]
    [InlineData(@"C:\setup.zip", "setup", true, true)]
    [InlineData(@"C:\user\downloads\setup.zip", "setup", true, true)]
    public void New_WithValidData_ShouldReturnInput(
        string input, 
        string requiredName, 
        bool mustBeFullMatch = false,
        bool isCaseSensitive = false)
    {
        // Arrange & Act
        FilePathWithNameFormat path = new(
            input, requiredName, mustBeFullMatch, isCaseSensitive);

        // Assert
        Assert.Equal(input, path.FullPath);
        Assert.Equal(input, path.ToString());
    }

    [Theory]
    [InlineData(@"C:\files.txt", "file", true)]
    [InlineData(@"C:\mySetup.zip", "setup", true)]
    [InlineData(@"C:\Mysetup.zip", "SeTup", false, true)]
    [InlineData(@"C:\setup.zip", "SeTup", true, true)]
    [InlineData(@"C:\user\downloads\setup.zip", "Gsetup", true, true)]
    public void New_WithInValidData_ShouldThrow_ArgumentException(
    string input,
    string requiredName,
    bool mustBeFullMatch = false,
    bool isCaseSensitive = false)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            new FilePathWithNameFormat(
                input, requiredName, mustBeFullMatch, isCaseSensitive);
        });
    }
}
