# MekUpdater
 Way to download updates from github releases using easy to use fluent interface. 
 
 Documentation in code and [here](https://github.com/matikkaeditorinkaantaja/MekUpdater/tree/main/MekUpdaterDocumentation)
 
 ## Installation
 1) Download ready for use Nuget packages (not yet in Nuget.org) to your local device from releases page. 
 2) Add local nuget feed to your visual studio and add package or extract nuget package and add project dll reference.
 3) Import and use, for examples see [MekUpdaterDocumentation](https://github.com/matikkaeditorinkaantaja/MekUpdater/tree/main/MekUpdaterDocumentation)

## Example
``` csharp
using MekPathLibrary;
using MekUpdater;
using MekUpdater.Helpers;
using MekUpdater.UpdateBuilder;



// path where update zip file will be downloaded, these files will be deleted if TidiesUp() is called
var zipPath = WindowsPath.UserTempFolder.ToFilePath<ZipPath>(@"MekUpdater\MekUpdater.zip");

// Path where setup.exe will end up with other extracted files, this is the folder where setup will be launched
var extractionPath = WindowsPath.DownloadsFolder;
extractionPath.Append("updates");


// Start update configuration using fluent api 
var update = UpdateBuilder
   .Create("matikkaeditorinkaantaja", "MekUpdater") // create using repository info (repository owner github username and repository name)
   .DownloadZipTo(zipPath)                          // define where update zip file from github will be downloaded
   .UsingExtractionFolder(extractionPath)           // define where files above will be extracted
   //.AddLogger(logger)                             // add logger to follow update progression
   .RunUpdate()                                     // Run update if conditions
   .IfNotPreview()                                  // run if newest available update is not preview
   .IfVersionBiggerThan(new VersionTag("v1.0.7"))   // run if update version tag is bigger than (must be format vX.X.X) othervise you must check version some other way
   .StartsSetup()                                   // Start setup argument
   .IsTrue()                                        // argument value
   .TidiesUpIsTrue()                                // remove used cache files 
   .Build();                                        // build update

var result = await update.RunDefaultUpdaterAsync();                        // run update with defined parameters


Console.WriteLine($"Success: '{result.Success}'");                         // info from result
Console.WriteLine($"Message: '{result.Message}'");
Console.WriteLine($"UpdateMsg: '{result.UpdateMsg}'");
Console.WriteLine($"Setup file at: '{update.CompletionInfo.SetupExePath}'");
```

## Want to contribute
- Open issue with enchancement tag and tell how you want to help or what kind of feature you want to add 

## Need more help?
- To get more help, open issue with "help wanted" label in GitHub issues page
- Contact via email matikkaeditorinkaantaja(at)gmail.com
