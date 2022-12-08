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

    [Theory]
    [InlineData("v1.0.0-preview", "v1.0.0-preview")]
    [InlineData("v2.3.9-alpha", "v2.3.9-alpha")]
    [InlineData("v2.30.0-beta", "v2.30.0-beta")]
    [InlineData("v2.30.0-Beta", "v2.30.0-beta")]
    [InlineData("2.30.0-Beta", "v2.30.0-beta")]
    [InlineData("v1.0-preview", "v1.0.0-preview")]
    [InlineData("v2-ALPHA", "v2.0.0-alpha")]
    [InlineData("v12-beta", "v12.0.0-beta")]
    [InlineData("v1beta", "v1.0.0-beta")]
    [InlineData("1beta", "v1.0.0-beta")]
    [InlineData("v1.0-full", "v1.0.0")]
    public void FullVersion_ShouldInclude_VersionId(string version, string expectedResult)
    {
        VersionTag tag = new(version);
        Assert.Equal(expectedResult, tag.FullVersion);
    }

    [Theory]
    [InlineData("v1.0.0", "v1.0.0")]
    [InlineData("v1.0.0-preview", "v1.0.0-PREVIEW")]
    [InlineData("v1.0", "v1")]
    [InlineData("v1.0.0", "v1")]
    [InlineData("v1.0.0-ALPHA", "v1-Alpha")]
    [InlineData("1.0.0", "1")]
    [InlineData("1.2.0-alpha", "1.2-alpha")]
    [InlineData("1.2.0", "1.2.0-full")]
    public void Equal_ShouldSeeSameValues_AsEqual(string right, string left)
    {
        VersionTag rightVersion = new(right);
        VersionTag leftVersion = new(left);


        bool equalSign1 = rightVersion == leftVersion;
        bool equalSign2 = leftVersion == rightVersion;

        bool equals1 = rightVersion.Equals(leftVersion);
        bool equals2 = leftVersion.Equals(rightVersion);

        bool equalsObject1 = rightVersion.Equals((object)leftVersion);
        bool equalsObject2 = leftVersion.Equals((object)rightVersion);

        Assert.True(equalSign1, "equal sign, right first");
        Assert.True(equalSign2, "equal sign, left first");
        Assert.True(equals1, "equals versionTag, right first");
        Assert.True(equals2, "equals versionTag, left first");
        Assert.True(equalsObject1, "equals object, right first");
        Assert.True(equalsObject2, "equals object, left first");
    }


    [Theory]
    [InlineData("v1.0.0", "v1.0.1")]
    [InlineData("v1.0.0", "v2.0.0")]
    [InlineData("v1.0.0-preview", "v1.0.0-alpha")]
    [InlineData("1.2.0-preview", "1.1.0-alpha")]
    [InlineData("v1-preview", "v1-alpha")]
    public void UnEqual_ShouldSeeDifferentValues_AsUnEqual(string right, string left)
    {
        VersionTag rightVersion = new(right);
        VersionTag leftVersion = new(left);

        bool equalSign1 = rightVersion != leftVersion;
        bool equalSign2 = leftVersion != rightVersion;
     
        Assert.True(equalSign1, "UnEqual sign, right first");
        Assert.True(equalSign2, "UnEqual sign, left first");
    }


    [Theory]
    [InlineData("v2", "v1")]
    [InlineData("v2.1", "v1.1")]
    [InlineData("v2.1.3", "v1.1.3")]
    [InlineData("v1.2.3", "v1.1.3")]
    [InlineData("v1.1.4", "v1.1.3")]
    [InlineData("v1.0.0", "v1.0.0-preview")]
    [InlineData("v1.0.0-preview", "v1.0.0-beta")]
    [InlineData("v1.0.0-beta", "v1.0.0-alpha")]
    public void Versions_ShouldBeOrderedBy_Major_Minor_Build_VersionId(string bigger, string smaller)
    {
        VersionTag biggerTag = new(bigger);
        VersionTag smallerTag = new(smaller);

        bool biggerIsBigger = biggerTag > smallerTag;
        bool smallerIsSmaller = smallerTag < biggerTag;

        Assert.True(biggerIsBigger, $"{biggerTag} > {smallerTag}");
        Assert.True(smallerIsSmaller, $"{smallerTag} < {biggerTag}");
    }


    [Theory]
    [InlineData("v1-beta", "v1-beta")]
    [InlineData("v1.1", "v1")]
    [InlineData("v1.1.1", "v1.1")]
    [InlineData("v1.1.0", "v1.1")]
    [InlineData("v1.1", "v1.1-preview")]
    public void RightVersions_ShouldBeBigger_OrEqual(string bigger, string smaller)
    {
        VersionTag biggerTag = new(bigger);
        VersionTag smallerTag = new(smaller);

        bool biggerIsBigger = biggerTag >= smallerTag;
        bool smallerIsSmaller = smallerTag <= biggerTag;

        Assert.True(biggerIsBigger, $"{biggerTag} >= {smallerTag}");
        Assert.True(smallerIsSmaller, $"{smallerTag} <= {biggerTag}");
    }


    [Theory]
    [InlineData("v1.2.3", "v1.2.3")]
    [InlineData("v1.2.3-preview", "v1.2.3-preview")]
    [InlineData("v1-beta", "v1.0.0-beta")]
    [InlineData("1", "v1.0.0")]
    [InlineData("1.3.3", "v1.3.3")]
    [InlineData("v100000.354.31", "v100000.354.31")]
    public void TryParse_ShouldReturn_TrueAnd_SameAsNew_IfValid(string validVersion, string expectedResult)
    {
        VersionTag tag = new(validVersion);

        bool canParse = VersionTag.TryParse(validVersion, out var parsed);

        Assert.True(canParse, $"Can parse tag from string '{validVersion}' using TryParse()");
        Assert.Equal(expectedResult, tag.FullVersion);
        Assert.Equal(tag.FullVersion, parsed.FullVersion);
    }

    [Theory]
    [InlineData("v-1.2.3")]
    [InlineData("this ain't a tag")]
    [InlineData("foo")]
    [InlineData("v.1.12.3")]
    [InlineData("v1.12.-3")]
    public void TryParse_ShouldReturn_FalseAndMin_IfInvalid(string invalidVersion)
    {
        bool canParse = VersionTag.TryParse(invalidVersion, out var parsed);

        Assert.False(canParse, $"Can parse tag from string '{invalidVersion}' using TryParse()");
        Assert.Equal(parsed.FullVersion, VersionTag.Min.FullVersion);
    }


    [Theory]
    [InlineData("v1.0.0", 0)]
    [InlineData("v1.0.0", 1)]
    [InlineData("v1.0.0", 2)]
    [InlineData("v1.0.0", 3)]

    public void SetVersionId_ShouldSet_VersionId(string version, int id)
    {
        VersionTag tag = new(version);

        tag.SetVersionId((VersionId)id);

        if (id is 0)
        {
            Assert.Equal(version, tag.FullVersion);
        }
        else
        {
            Assert.Equal($"{version}-{((VersionId)id).ToString().ToLower()}", tag.FullVersion);
        }
    }



    [Theory]
    [InlineData("v1.1.1", "v3.1.1")]
    [InlineData("v3.1.1", "v3.2.1")]
    public void BiggerThanSign_ShouldReturn_False_IfAnyLeftNumberSmaller(string left, string right)
    {
        var leftTag = new VersionTag(left);
        var rightTag = new VersionTag(right);

        bool result = leftTag > rightTag;

        Assert.False(result);
    }

}
