using MekPathLibrary;
using MekUpdater.Helpers;
using MekUpdater.UpdateBuilder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

var result = await UpdateBuilder.Create("matikkaeditorinkaantaja", "Matikkaeditorinkaantaja")
                                .Where(new FolderPath(@"C:\Users\henri\Downloads\updates"))
                                .Where(new ZipPath(@"C:\Users\henri\Downloads\updates\MekUpdate.zip"))
                                .AddLogger(NullLogger.Instance)
                                .RunUpdate()
                                .IfNotPreview()
                                .IfVersionBiggerThan(new VersionTag("v3.1.5"))
                                .StartsSetupIsTrue()
                                .TidiesUp(true)
                                .Build()
                                .RunDefaultUpdaterAsync();



Console.WriteLine(result.Success);
Console.WriteLine(result.Message);
Console.WriteLine(result.UpdateMsg);








Console.ReadKey();
