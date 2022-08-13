
namespace MekPathLibraryTests;

public class SetupExePathTests
{
    [Theory]
    [InlineData(@"C:\files\setup.exe")]
    [InlineData(@"C:\files\Mysetup.exe")]
    [InlineData(@"C:\files\CapitalisedSetup.exe")]
    [InlineData(@"C:\files\MyVeryLongSetup3.16.exe")]
    public void New_WithValidSetupPath_ShouldReturn_Input(string input)
    {
        // Arrange & Act
        SetupExePath setupExePath = new(input);

        // Assert
        Assert.Equal(input, setupExePath.FullPath);
        Assert.Equal(input, setupExePath.ToString());
    }

    [Theory]
    [InlineData(@"C:\files\Getup.exe")]
    [InlineData(@"C:\files\G.exe")]
    [InlineData(@"C:\files\AppUpdater3.1.6.exe")]
    public void New_WithNoSetup_InName_ShouldThrow_ArgumentException(string input)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            new SetupExePath(input);
        });
    }
    
    [Theory]
    [InlineData(@"C:\files\setup.txt", @"C:\files\setup.exe")]
    [InlineData(@"C:\files\AppSetup.gg", @"C:\files\AppSetup.exe")]
    public void New_WithNoExe_InName_ShouldChange_ExtensionExe(
        string input, string expected)
    {
        // Arrange & Act
        SetupExePath setupExePath = new(input);

        // Assert
        Assert.Equal(expected, setupExePath.FullPath);
        Assert.Equal(expected, setupExePath.ToString());
    }
}
