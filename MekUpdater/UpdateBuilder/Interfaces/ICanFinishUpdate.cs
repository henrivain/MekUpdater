namespace MekUpdater.UpdateBuilder.Interfaces;

/// <summary>
/// Build update or choose to remove cache files after update, part of update builder fluent api
/// </summary>
public interface ICanFinishUpdate : ICanBuild
{
    /// <summary>
    /// Set remove cache files used during the update to TRUE
    /// </summary>
    /// <returns>calling instance</returns>
    ICanBuild TidiesUpIsTrue();

    /// <summary>
    /// Set remove cache files used during the update to FALSE
    /// </summary>
    /// <returns>calling instance</returns>
    ICanBuild TidiesUpIsFalse();
}
