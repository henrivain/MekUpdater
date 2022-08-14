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
    Other, 
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
    CantInstallPreview,
    SetupNotFound
}
