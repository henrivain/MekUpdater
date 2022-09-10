using Matikkaeditorinkaantaja.Logging;
using MekPathLibrary;
using MekUpdater.Helpers;
using MekUpdater.UpdateBuilder;
using MekUpdater;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2254 // Template should be a static expression

var logger = UpdateLogger.GetDefault();

// path where update zip file will be downloaded, these files will be deleted if TidiesUp() is called
var zipPath = new ZipPath();

// Path where setup.exe will end up with other extracted files, this is the folder where setup will be launched
var extractionPath = new FolderPath();  

var update = UpdateBuilder.Create("matikkaeditorinkaantaja", "Matikkaeditorinkaantaja")
                          .UsingExtractionFolder(new FolderPath(@"C:\Users\henri\Downloads\updates"))
                          .DownloadZipTo(new ZipPath(@"C:\Users\henri\Downloads\updates\MekUpdate.zip"))
                          .AddLogger(logger)
                          .RunUpdate()
                          .IfNotPreview()
                          .IfVersionBiggerThan(new VersionTag("v3.1.5"))
                          .StartsSetup()
                          .IsTrue()
                          .TidiesUp(true)
                          .Build();

var result = await update.RunDefaultUpdaterAsync();


logger.LogInformation($"Success: '{result.Success}'");
logger.LogInformation($"Message: '{result.Message}'");
logger.LogInformation($"UpdateMsg: '{result.UpdateMsg}'");
logger.LogInformation($"Setup file at: '{update.SetupExePath}'");

Console.ReadKey();
