namespace MekUpdaterTests;

public class VersionTagTests
{
    [Theory]
    [InlineData("v1.0.0")]
    [InlineData("v2.3.9")]
    [InlineData("v2.30.988")]
    public void ThreeNumberVersion_ParsedShouldReturnMatching(string version)
    {
        VersionTag tag = new(version);
        Assert.Equal(version, tag.Version);
    }

    [Theory]
    [InlineData("v1.0")]
    [InlineData("v2.3")]
    [InlineData("v2.30")]

    public void TwoNumberVersion_ShouldAddZero(string version)
    {
        VersionTag tag = new(version);
        Assert.Equal($"{version}.0", tag.Version);
    }


    [Theory]
    [InlineData("v1")]
    [InlineData("v20")]
    public void OneNumberVersion_ShouldAddZero(string version)
    {
        VersionTag tag = new(version);
        Assert.Equal($"{version}.0.0", tag.Version);
    }

    [Theory]
    [InlineData("v")]
    [InlineData("")]
    public void ZeroNumberVersion_ShouldThrowArgumentException(string version)
    {
        Assert.Throws<ArgumentException>(() =>
        {
            VersionTag tag = new(version);
        });
    }

    [Theory]
    [InlineData("1.0.0")]
    [InlineData("2.3.9")]
    [InlineData("2.30.988")]
    public void VersionWithout_V_ShouldAdd_V(string version)
    {
        VersionTag tag = new(version);
        Assert.Equal($"v{version}", tag.Version);
    }
}
