// *******************************************************
// * <copyright file="FileDirectoryClassHelper.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxDataManager
{
    using System.IO;
    using System.Linq;
    using System.Security.AccessControl;

    /// <summary>
    /// Class representing the <see cref="FileDirectoryClassHelper"/>
    /// </summary>
    internal class FileDirectoryClassHelper
    {
        /// <summary>
        /// Creates the directory path
        /// to the specific file or directory.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="sourceFolder">The source folder.</param>
        /// <param name="folderNames">The folder names.</param>
        public static void CreateDirectoryPath(DirectoryInfo[] info, string sourceFolder, string[] folderNames)
        {
            if (info.Length > 0)
            {
                for (int i = 0; i < info.Length; i++)
                {
                    if (folderNames[0] == info[i].Name)
                    {
                        if (!Directory.Exists(Path.Combine(sourceFolder, info[i].Name)))
                        {
                            Directory.CreateDirectory(Path.Combine(sourceFolder, info[i].Name), info[i].GetAccessControl());
                            CopyFolderSettings(new DirectoryInfo(Path.Combine(sourceFolder, info[i].Name)), info[i]);
                        }
                        
                        CreateDirectoryPath(info[i].GetDirectories(), Path.Combine(sourceFolder, info[i].Name), folderNames.Skip(1).ToArray());
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Copies the folder settings.
        /// </summary>
        /// <param name="destFolder">The destination folder.</param>
        /// <param name="sourceFolder">The source folder.</param>
        public static void CopyFolderSettings(DirectoryInfo destFolder, DirectoryInfo sourceFolder)
        {
            DirectorySecurity secSrcFolder = sourceFolder.GetAccessControl();
            secSrcFolder.SetAccessRuleProtection(true, true);
            destFolder.SetAccessControl(secSrcFolder);

            destFolder.Attributes = sourceFolder.Attributes;
            destFolder.CreationTime = sourceFolder.CreationTime;
            destFolder.CreationTimeUtc = sourceFolder.CreationTimeUtc;
            destFolder.LastAccessTime = sourceFolder.LastAccessTime;
            destFolder.LastAccessTimeUtc = sourceFolder.LastAccessTimeUtc;
            destFolder.LastWriteTime = sourceFolder.LastWriteTime;
            destFolder.LastWriteTimeUtc = sourceFolder.LastWriteTime;
        }

        /// <summary>
        /// Copies the file settings.
        /// </summary>
        /// <param name="destFile">The destination file.</param>
        /// <param name="sourceFile">The source file.</param>
        public static void CopyFileSettings(FileInfo destFile, FileInfo sourceFile)
        {
            FileSecurity secSrcFile = sourceFile.GetAccessControl();
            secSrcFile.SetAccessRuleProtection(true,true);
            destFile.SetAccessControl(secSrcFile);

            destFile.Attributes = sourceFile.Attributes;
            destFile.CreationTime = sourceFile.CreationTime;
            destFile.CreationTimeUtc = sourceFile.CreationTimeUtc;
            destFile.LastAccessTime = sourceFile.LastAccessTime;
            destFile.LastAccessTimeUtc = sourceFile.LastAccessTimeUtc;
            destFile.LastWriteTime = sourceFile.LastWriteTime;
            destFile.LastWriteTimeUtc = sourceFile.LastWriteTime;
        }
    }
}
