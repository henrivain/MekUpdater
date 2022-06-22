using MekUpdater.ValueTypes;
using MekUpdater.Fluent;
using ExampleProject;
using MekUpdater.ValueTypes.PathValues;
using MekUpdater.Helpers;

VersionTag tag = VersionTag.GetEntryAssemblyVersion();

//MekUpdateProcess process = new("matikkaeditorinaantaja", "Matikkaeditorinaantaja");

string shit = Environment.GetEnvironmentVariable("USERPROFILE")?.ToString() + @"\Downloads" + Path.DirectorySeparatorChar;
string shit2 = new FolderPath(Environment.GetEnvironmentVariable("USERPROFILE")?.ToString() + @"\Downloads" + Path.DirectorySeparatorChar).ToString();

Console.WriteLine("gg" + Helper.DownloadsFolder.ToString());
Console.WriteLine(shit2);
Console.WriteLine(new FolderPath(shit).ToString());
Console.WriteLine();

Console.WriteLine(shit);
ZipPath zipPath = new(
                Path.Combine(Helper.DownloadsFolder.ToString(), $"\\temp\\update.zip"));

Console.WriteLine(zipPath.ToString());
UpdateHandler.Run();


Console.ReadKey();
