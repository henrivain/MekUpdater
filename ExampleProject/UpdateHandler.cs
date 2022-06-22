/// Copyright 2021 Henri Vainio 
using MekUpdater.Fluent;
using MekUpdater.ValueTypes;


namespace ExampleProject
{
    internal class UpdateHandler
    {
        internal static async void Run()
        {
            (await (await MekUpdateProcess.New("matikkaeditorinkaantaja", "Matikkaeditorinkaantaja")
                .UseLogging()
                .CheckUpdates())
                .DownloadIfNewVersion(VersionTag.GetEntryAssemblyVersion()))
                .StartSetup();
        }
    }
}
