namespace MekUpdater.UpdateBuilder.Interfaces;

/// <summary>
/// Define mode for setup start, part of fluent api
/// </summary>
public interface IStartSetupMode
{
    /// <summary>
    /// Define, that setup.exe should be launched after update files are downloaded
    /// </summary>
    /// <returns>calling instance</returns>
    ICanFinishUpdate IsTrue();

    /// <summary>
    /// Define, that setup.exe should be NOT launched after update files are downloaded
    /// </summary>
    /// <returns>calling instance</returns>
    ICanFinishUpdate IsFalse();
}
