namespace MekUpdater.UpdateRunner;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

/// <summary>
/// Reason for action exit
/// </summary>
public enum UpdateMsg
{
    None, Unknown, Completed,

    BadUrl, PathNotOpen,
    FileReadOnly, FileNotFound,
    PathTooLong, NoDirectory,
    NoPermission, OutsideMachine,
    ServerTimeout, NetworkError,
    UnSupportedDataType, InvalidPathChars,
    FileAlreadyExistOrBadName, BadPathFormat,
    ObjectDisposed, NoFileName,
    ErrorWhileOpening, NoShellSupport,
    PathNullOrEmpty, UpdateAlreadyInstalled,
    SetupNotFound, DeleteFileFailed,
    ParseJson, HttpRequestFailed,
    ParameterNullOrEmpty
}
