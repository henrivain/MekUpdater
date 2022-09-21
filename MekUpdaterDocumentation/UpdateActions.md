# Update Actions - Api documentation

Namespace: <span style="color:#75B6E7"> MekUpdater</span>  
Assembly: <span style="color:#75B6E7"> MekUpdater.dll</span>  
Released 21/9/2022

<hr/>  
<br/>

UpdateActions class provides easy access to classes used in different parts during the update process.  
You can quickly customise your update with update actions.

<br/>
<hr/>
<br/>

## CheckForUpdates

Make awaitable request to GitHub api and get back data about available versions.

```csharp
public static async Task<UpdateCheckResult> CheckForUpdates(string repoOwner, string repoName)
{
}
```

### Parameters:

If any parameter is null or empty, process returns failed result

- <span style="color:#9CDCFE">repoOwner:</span> username of repository owner
- <span style="color:#9CDCFE">repoName:</span> name of a GitHub repository that is owned by user whose username is given

### Returns:

- UpdateCheckResult that gives information about available release, process success and possible fail reasons

<br/>
<hr/>
<br/>

## DownloadZip

Download source code to your device as a zip file from github api

```csharp
public static async Task<DownloadUpdateFilesResult> DownloadZip(string downloadUrl, ZipPath zipPath)
{
}
```

### Parameters:

- <span style="color:#9CDCFE">downloadUrl:</span> url path to GitHub Api
  Example of download url:

  ```
  "https://api.github.com/repos/matikkaeditorinkaantaja/Matikkaeditorinkaantaja/zipball/v3.1.6"
  ```

- <span style="color:#9CDCFE">zipPath:</span> Path to file with ".zip" extension. If path does not exist yet, it will be created. <span style="color:#4EC9B0">ZipPath</span> class can be found on <span style="color:#75B6E7">MekPathLibrary</span> namespace.

### Exceptions

- ArgumentException

  - if downloadUrl is null or empty
  - if zipPath does not have value

### Returns:

- DownloadUpdateFilesResult that gives information about process success and possible fail reasons

<br/>
<hr/>
<br/>

## ExtractZipFile

Extract given zip file to given folder async

```csharp
public static async Task<ZipExtractionResult> ExtractZipFile(ZipPath zipPath, FolderPath extractPath)
{
}

```

### Parameters:

if any of parameters does not have value, returns failed result.

- <span style="color:#9CDCFE">zipPath:</span> Path to file with ".zip" extension.

  If path does not exist yet, it will be created. <span style="color:#4EC9B0">ZipPath</span> class can be found on <span style="color:#75B6E7">MekPathLibrary</span> namespace.

- <span style="color:#9CDCFE">extractPath:</span> Path to folder where zip file will be extracted.

  <span style="color:#4EC9B0">FolderPath</span> class can be found on <span style="color:#75B6E7">MekPathLibrary</span> namespace. If path doesn't exist, it will be created.

### Returns:

- ZipExtractionResult that gives information about process success and possible fail reasons with stack trace

<br/>
<hr/>
<br/>

## LaunchSetup

Launch setup.exe process from given path

```csharp
public static StartSetupResult LaunchSetup(SetupExePath path, bool requireAdmin = true)
{
}
```

### Parameters:

- <span style="color:#9CDCFE">path:</span> Represents full path to setup.exe file. <span style="color:#4EC9B0">SetupExePath</span> class can be found on <span style="color:#75B6E7">MekPathLibrary</span> namespace.
- <span style="color:#9CDCFE">requireAdmin:</span> boolean value representing if process should be launched automatically in Admin mode

<br/>
<br/>

## Need more help?

To get more help, open issue with "help wanted" label in [GitHub issues page](https://github.com/matikkaeditorinkaantaja/MekUpdater/issues/)  
or contact email matikkaeditorinkaantaja(at)gmail.com
