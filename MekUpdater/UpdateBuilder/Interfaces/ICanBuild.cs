namespace MekUpdater.UpdateBuilder.Interfaces;

/// <summary>
/// Build update, part of update builder fluent api
/// </summary>
public interface ICanBuild
{
    /// <summary>
    /// Build update to be ready to be run
    /// </summary>
    /// <returns>update generated from update builder</returns>
    Update Build();
}
