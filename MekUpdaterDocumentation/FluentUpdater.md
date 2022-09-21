# Fluent Updater - Api documentation

Namespace: <span style="color:#75B6E7"> MekUpdater.UpdateBuilder</span>  
Assembly: <span style="color:#75B6E7"> MekUpdater.dll</span>

- This part of the updater uses fluent api style where you can chain commands nicely.
- Build update fist with update builder and then run it.

## Examples

- Start building you update by initializing new Update builder
- Update builder is situated in "MekUpdater.UpdateBuilder" namespace
- You may also need to use MekPathLibrary nuget package

<br/>

### Run default update

Program.cs

```csharp
using MekUpdater.UpdateBuilder;
using MekPathLibrary;
using MekUpdater.Helpers;
using MekUpdater;

// 1)
// Start by defining repository info (case sensitive)
// Wrong data will cause update to be checked from wrong repository or fail
string repoOwner = "matikkaeditorinkaantaja";
string repoName = "MekUpdater";

// 2)
// Define where zip file is located in the user's device
// You can also use built in function to get environment paths like appdata
// ZipPath will automatically validate paths
// You can create path to folder and then combine with relative path as shown beneath
ZipPath zipPath = new FolderPath(WindowsPath.UserTempFolder).ToFilePath<ZipPath>(@"MekUpdater\MekUpdater.zip");

// path where zip file will be extracted
FolderPath extractionPath = new(WindowsPath.DownloadsFolder);

// 3)
// Define current version. Updater only downloads update that has bigger version than this
VersionTag requiredVersion = new("v1.0.7");


// 4)
// Build update
// Explanations of each step are shown below
var update = UpdateBuilder.Create(repoOwner, repoName)              // 4.1
                          .DownloadZipTo(zipPath)                   // 4.2
                          .UsingExtractionFolder(extractionPath)    // 4.3
                          //.AddLogger(myILogger)                   // 4.4
                          .RunUpdate()                              // 4.5
                          .IfNotPreview()                           // 4.6
                          .IfVersionBiggerThan(requiredVersion)     // 4.7
                          .StartsSetup()                            // 4.8
                          .IsTrue()                                 // 4.9
                          .TidiesUpIsTrue()                         // 4.10
                          .Build();                                 // 4.11

// 5)
// Run update with give argument async with default updater
var result = await update.RunDefaultUpdaterAsync();

// 6)
// Check results
Console.WriteLine($"Update success: {result.Success}")
Console.WriteLine($"Message: '{result.Message}'");
Console.WriteLine($"UpdateMsg: '{result.UpdateMsg}'");
Console.WriteLine($"Setup file at: '{update.CompletionInfo.SetupExePath}'");
```

<br/>
<br/>
<br/>

## 4) Build steps

This part includes 4 main steps some parts of which can be skipped. Some parts can also be skipped if needed.

### Step 4.1

- Create instance of update builder object.
- Repository owner's GitHub username and repository name are passed as arguments.

```csharp
var builder = UpdateBuilder.Create("myUserName", "myRepoName")
```

### Steps 4.2 - 4.5

- Give information about paths and logger to be used
- These commands can be used in any order
- zip path and extraction paths are validated using MekPathLibrary.ILocalPath derived classes
- See default values in section "Default variable values"

```csharp
.DownloadZipTo(new ZipPath(@"D:\Path\to\my\file.zip"))
.UsingExtractionFolder(new FolderPath("C:\MyFolder"))
```

- If logger should be used, you can pass any Microsoft.Extensions.Logging.ILogger using command

```
.AddLogger(myILogger)
```

- After you have defined wanted paths and logger, you have to move to next steps using:

```csharp
.RunUpdate()
```

### Steps 4.6 - 4.8

- Give conditions that can be met to execute update process
- If only full releases are wanted use

```
.IfNotPreview()
```

- To check that available version is bigger than current app version you can provide current version as MekUpdater.Helpers.VersionTag
- Available version have to be bigger than this version
- default value is v0.0.0-alpha

```csharp
.IfVersionBiggerThan(new VersionTag("v1.0.0"))
```

- After defining update conditions, define setup start mode using

```
.StartsSetup()
```

### Step 4.9

- Give setup start mode using
- Note that setup file name has to have word "setup" and extension ".exe" in it
- If your main file does not meet these requirements, you can not launch it directly from updater. Choose .IsFalse()

```csharp
.IsTrue()   // if setup.exe should be executed as a part of update
// or
.IsFalse()  // if no launch action is needed
```

### Step 4.10 - 4.11

- Define if updater should delete not needed working directories

```csharp
.TidiesUpIsTrue()   // if directories should be deleted
// or
.TidiesUpIsFalse()  // if directories should be kept on device
```

- Lastly you need to build the update
- After build action, update is ready to be run

```csharp
.Build()
```

<br/>
<br/>

## 5) Run Update

- When update is built, it can be run asynchronoysly
- Process uses default DefaultGithubUpdater found in MekUpdater.UpdateRunner namespace
- You can also implement your own IUpdater to run updates

```csharp
var result = await update.RunDefaultUpdaterAsync();
```

- If logger is defined, updater log information about update progression
- Updater also updates information in update property

```csharp
public IUpdateCompletionInfo CompletionInfo { get; }
```

<br/>
<br/>
<br/>

## Default variable values

- default value for zip path is found in WindowsPath.DefaultUpdaterZipFolder

```
C:\Users\%USERNAME%\Downloads\updates\[HostAppName]Setup.zip
```

- default value for extraction path is found in WindowsPath.DefaultUpdaterDestinationFolder

```
C:\Users\%USERNAME%\Downloads\updates
```

<br/>
<br/>

## Need more help?

To get more help, open issue with "help wanted" label in [GitHub issues page](https://github.com/matikkaeditorinkaantaja/MekUpdater/issues/)  
or contact email matikkaeditorinkaantaja(at)gmail.com
