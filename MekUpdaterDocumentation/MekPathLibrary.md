# Mek Path Library - Api documentation

Namespace: <span style="color:#75B6E7"> MekPathLibrary</span>  
Assembly: <span style="color:#75B6E7"> MekPathLibrary.dll</span>  
Released 21/9/2022

<hr/>  
<br/>

Mek path library is meant improve actions that require using paths in local device. With Mek path library all paths are validated on initialization and <span style="color:#4EC9B0">ArgumentException</span> is rised if path is not valid.You don't need until path is used just to realise path was invalid. Also combined paths are validated immediately.

<br/>
<hr/>
<br/>

## Mek path library offers easy way to validate your own paths

- Paths implement <span style="color:#B8D7A3">ILocalPath</span> interface, so you can make your own implementations that can function with others
- All paths are derived from <span style="color:#4EC9B0">LocalPath</span> base class that implements many validators
- File paths have their own <span style="color:#4EC9B0">FilePath</span> subclass that also gets file names and extensions with its own validators
- Derived types can their own constant file extension, which is automatically inserted
- <span style="color:#4EC9B0">FilePathWithNameFormat</span> class can also validate file names

## Examples

### Initialize different paths

Program.cs

```csharp
using MekPathLibrary;

ILocalPath path = new LocalPath(@"C:\Users\user\mystuff.txt");
Console.WriteLine(path.FullPath)                // output: C:\Users\user\mystuff.txt

// To string is overridden
Console.WriteLine(path.ToString())              // output: C:\Users\user\mystuff.txt


// Folder path removes file names
FolderPath folderPath = new(@"C:\Users\user\mystuff.txt");
Console.WriteLine(folderPath)                   // output: C:\Users\user\

// File path get extension
FilePath filePath = new(@"C:\Users\user\mystuff.bruh")
Console.WriteLine(zipPath.FileExtension)        // output: bruh

// File path changes right file extension
ZipPath zipPath = new(@"C:\Users\user\mystuff.txt")
Console.WriteLine(zipPath)                      // output: C:\Users\user\mystuff.zip

// File name can be validated
try
{
    // file name must include "setup" and extension must be ".exe"
    SetupExePath setupPath = new(@"C:\Users\user\mystuff.exe");
}
catch (System.ArgumentException)
{
    Console.WriteLine("invalid path");          // output: invalid path
}

// Append path (Combine), file extension is removed from middle
LocalPath myLocalPath = new(@"C:\Users\user.txt");
myLocalPath.Append("Downloads\\mystuff.zip");
Console.WriteLine(myLocalPath);                 // C:\Users\user\Downloads\\mystuff.zip

// Append path, but remove name from first path
LocalPath myOtherPath = new(@"C:\Users\user.txt");
myOtherPath.RemoveNameAndAppend("Downloads\\mystuff.zip");
Console.WriteLine(myOtherPath);                 // C:\Users\Downloads\\mystuff.zip

// Folder path to file path
FolderPath myFolderPath = new(@"C:\Users\user");
ZipPath myZipPath = myFolderPath.ToFilePath<ZipPath>(@"Downloads\mystuff.zip");
Console.WriteLine(myZipPath);                   // output: C:\Users\user\Downloads\mystuff.zip



```

## Need more help?

To get more help, open issue with "help wanted" label in [GitHub issues page](https://github.com/matikkaeditorinkaantaja/MekUpdater/issues/)  
or contact email matikkaeditorinkaantaja(at)gmail.com
