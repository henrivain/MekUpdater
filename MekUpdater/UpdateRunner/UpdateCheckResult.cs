﻿using MekPathLibraryTests.Check;
using MekPathLibraryTests.UpdateBuilder;
namespace MekPathLibraryTests.UpdateRunner;

public class UpdateCheckResult : UpdateResult
{
    public UpdateCheckResult(bool success) : base(success) { }
    public string UsedUrl { get; set; } = string.Empty;
    public ParsedVersionData? VersionData { get; set; } = null;
}