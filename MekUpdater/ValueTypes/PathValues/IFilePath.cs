/// Copyright 2021 Henri Vainio 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekUpdater.ValueTypes.PathValues;


public interface IFilePath : ILocalPath
{
    /// <summary>
    /// Filename without extension
    /// </summary>
    string FileName { get; }
    
    /// <summary>
    /// Path to folder, where file is situated
    /// </summary>
    FolderPath FolderPath { get; }
    
    /// <summary>
    /// Extension of the file
    /// </summary>
    static string FileExtension { get; } = string.Empty;
}
