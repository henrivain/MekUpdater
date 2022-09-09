using Matikkaeditorinkaantaja.Logging;
using MekPathLibrary;
using MekUpdater.Helpers;
using MekUpdater.UpdateBuilder;
using Microsoft.Extensions.Logging;

var logger = UpdateLogger.GetDefault();

var result = await UpdateBuilder.Create("matikkaeditorinkaantaja", "Matikkaeditorinkaantaja")
                                .Where(new FolderPath(@"C:\Users\henri\Downloads\updates"))
                                .Where(new ZipPath(@"C:\Users\henri\Downloads\updates\MekUpdate.zip"))
                                .AddLogger(logger)
                                .RunUpdate()
                                .IfNotPreview()
                                .IfVersionBiggerThan(new VersionTag("v3.1.5"))
                                .StartsSetupIsTrue()
                                .TidiesUp(true)
                                .Build()
                                .RunDefaultUpdaterAsync();

logger.LogInformation($"Success: '{result.Success}'");
logger.LogInformation($"Message: '{result.Message}'");
logger.LogInformation($"UpdateMsg: '{result.UpdateMsg}'");

Console.ReadKey();
