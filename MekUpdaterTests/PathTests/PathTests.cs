/// Copyright 2021 Henri Vainio 
namespace MekUpdater.Tests;


[TestClass]
public partial class PathTests
{
    const string ZipPath = @"C:\Users\user\Downloads\temp\update.zip";
    const string TxtPath = @"C:\Users\user\Downloads\temp\update.txt";
    const string ExePath = @"C:\Users\user\Downloads\temp\update.exe";
    const string FolderPath = @"C:\Users\user\Downloads\temp\";
    
    const string FileNameMidPath = @"C:\temp\text.txt\gg.txt";

    const string NodrivePath = @"temp\text.txt\gg.txt";
    const string RelativeFolderPath = @"./FooterItem";
    const string RelativeTxtPath = @"./FooterItem/gg.txt";
    
    const string InvalidNamePath = @"./FooterItem/g:g.txt";
    const string InvalidPath = @"./Foote<?>rItem/gg.txt";

}
