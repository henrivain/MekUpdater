using ExampleProject;
using MekPathLibrary;
using MekUpdater;
using MekUpdater.Helpers;
using MekUpdater.UpdateBuilder;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2254 // Template should be a static expression




// get logger
var logger = UpdateLogger.GetDefault();

// path where update zip file will be downloaded, these files will be deleted if TidiesUp() is called
var zipPath = new ZipPath(Path.Combine(WindowsPath.UserTempFolder.FullPath, @"MekUpdater\MekUpdater.zip"));

// Path where setup.exe will end up with other extracted files, this is the folder where setup will be launched
var extractionPath = new FolderPath(Path.Combine(WindowsPath.DownloadsFolder.FullPath, "updates"));




// Start update configuration using fluent api 
var update = UpdateBuilder.Create("henrivain", "Matikkaeditorinkaantaja")               // create using repository info (repository owner github username and repository name)
                          .DownloadZipTo(zipPath)                                       // define where update zip file from github will be downloaded
                          .UsingExtractionFolder(extractionPath)                        // define where files above will be extracted
                          .AddLogger(logger)                                            // add logger to follow update progression
                          .RunUpdate()                                                  // Run update if conditions
                          .IfNotPreview()                                               // run if newest available update is not preview
                          .IfVersionBiggerThan(new VersionTag("v3.1.5"))                // run if update version tag is bigger than (must be format vX.X.X) othervise you must check version some other way
                          .StartsSetup()                                                // Start setup argument
                          .IsTrue()                                                     // argument value
                          .TidiesUpIsTrue()                                             // remove used cache files 
                          .Build();                                                     // build update

var result = await update.RunDefaultUpdaterAsync();                                     // run update with defined parameters


logger.LogInformation($"Success: '{result.Success}'");                                  // info from result
logger.LogInformation($"Message: '{result.Message}'");
logger.LogInformation($"UpdateMsg: '{result.UpdateMsg}'");
logger.LogInformation($"Setup file at: '{update.CompletionInfo.SetupExePath}'");

Console.ReadKey();
