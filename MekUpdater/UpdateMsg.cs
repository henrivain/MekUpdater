namespace MekUpdater;

/// <summary>
/// Reason for process exit
/// </summary>
public enum UpdateMsg
{
    None, 
    Unknown, 

    BadUrl, 
    PathNotOpen, 
    FileReadOnly, 
    FileNotFound,
    PathTooLong, 
    NoDirectory, 
    NoPermission, 
    OutsideMachine,
    ServerTimeout, 
    NetworkError, 
    UnSupportedDataType,
    InvalidPathChars, 
    FileAlreadyExistOrBadName, 
    BadPathFormat,
    ObjectDisposed, 
    NoFileName, 
    ErrorWhileOpening, 
    NoShellSupport,
    PathNullOrEmpty, 
    UpdateAlreadyInstalled,
    SetupNotFound,

    Completed,
    DeleteFileFailed
}
