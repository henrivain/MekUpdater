namespace MekUpdater.Helpers;

internal class SetupPathFinder
{
    internal readonly record struct SetupInfo(FolderPath ExtractionFolder, string RepoOwnerName, string RepoName);

    public SetupPathFinder(SetupInfo info)
    {
        Info = info;
    }
    
    
    public SetupInfo Info { get; }


    internal SetupPathFinderResult TryFindPath()
    {
        SetupFolderFinderResult folderFinderResult = GetSetupContainingFolderName(Info.ExtractionFolder);
        if (folderFinderResult.Success is false) return folderFinderResult;

        FolderPath setupFolder = new(Path.Combine(Info.ExtractionFolder.FullPath, folderFinderResult.SetupFolderName + "\\"));
        return GetSetupFileName(setupFolder);
    }

    /// <summary>
    /// Get name of name containing setup name
    /// </summary>
    /// <param name="extractPath"></param>
    /// <returns>Name of name containing setup name or null if not found</returns>
    private SetupFolderFinderResult GetSetupContainingFolderName(FolderPath extractPath)
    {
        List<string> folderNames;
        try
        {
            folderNames = Directory
                .EnumerateDirectories(extractPath.FullPath)
                .Select(x => Path.GetFileName(x))                   
                .ToList();
        }
        catch (Exception ex)
        {
            return new(false)
            {
                SetupPathMsg = SetupPathMsg.CantEnumerateFolders,
                Message = $"Can't enumerate dictionaries because of exception {ex}: {ex.Message}"
            };
        }
        foreach (var name in folderNames ?? Enumerable.Empty<string>())
        {
            if (IsExtractedFolderMatch(name)) return new(true)
            {
                SetupFolderName = name
            };
        }
        return new(false)
        {
            SetupPathMsg = SetupPathMsg.NoMatchingFolder,
            Message = $"Could not find folder matching: " +
            $"path '{extractPath}', repo owner '{Info.RepoOwnerName}', repo name '{Info.RepoName}'"
        };
    }

    private bool IsExtractedFolderMatch(string folderName)
    {
        return folderName.Trim().StartsWith($"{Info.RepoOwnerName}-{Info.RepoName}-");
    }

    /// <summary>
    /// Get name of setup name
    /// </summary>
    /// <param name="setupFolder"></param>
    /// <returns>Name of setup name or null if not found</returns>
    internal static SetupPathFinderResult GetSetupFileName(FolderPath setupFolder)
    {
        List<string> fileNames;
        try
        {
            fileNames = Directory
                .EnumerateFiles(setupFolder.FullPath)
                .Select(x => Path.GetFileName(x))
                .ToList();
        }
        catch (Exception ex)
        {
            return new(false)
            {
                SetupPathMsg = SetupPathMsg.CantEnumerateFiles,
                Message = "Could not enumerate files because of exception " +
                $"{ex}: {ex.Message}"
            };
        }
        foreach (var name in fileNames ?? Enumerable.Empty<string>())
        {
            try
            {
                SetupExePath result = new(Path.Combine(setupFolder.FullPath, name));
                return new(true)
                {
                    SetupPath = result,
                    Message = $"Found setup exe file at '{result}'"
                };
            }
            catch (ArgumentException)
            {
                continue;
            }
        }
        return new(false)
        {
            Message = $"Could not parse any setup file from folder {setupFolder}",
            SetupPathMsg = SetupPathMsg.NoMatchingFile
        };
    }
}
