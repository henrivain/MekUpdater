namespace MekUpdater.ValueTypes;

public interface ILocalPath
{
    string FullPath { get; set; }
    bool HasValue();
    bool IsValid();
    int GetHashCode();
    string ToString();
    bool Equals(object? obj);
    bool PathExist();
}