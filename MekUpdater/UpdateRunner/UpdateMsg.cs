namespace MekUpdater.UpdateRunner;

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
    ParseJson, HttpRequestFailed
}
