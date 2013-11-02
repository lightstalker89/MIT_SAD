// *******************************************************
// * <copyright file="DirectoryVolumeComparator.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

    /// <summary>
    /// Class representing the comparator for directories
    /// Refer to: 
    /// http://pinvoke.net/default.aspx/kernel32.CreateFile
    /// http://pinvoke.net/default.aspx/kernel32.GetFileInformationByHandle
    /// http://pinvoke.net/default.aspx/kernel32.CloseHandle
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", 
        Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", 
        Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1606:ElementDocumentationMustHaveSummaryText", 
        Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
        Justification = "Reviewed.")]
    public class DirectoryVolumeComparator
    {
        /// <summary>
        /// Value of an invalid file handle
        /// </summary>
        public const short INVALID_HANDLE_VALUE = -1;

        /// <summary>
        /// </summary>
        [Flags]
        public enum EFileAccess : uint
        {
            /// <summary>
            /// </summary>
            GenericRead = 0x80000000, 

            /// <summary>
            /// </summary>
            GenericWrite = 0x40000000, 

            /// <summary>
            /// </summary>
            GenericExecute = 0x20000000, 

            /// <summary>
            /// </summary>
            GenericAll = 0x10000000
        }

        /// <summary>
        /// </summary>
        [Flags]
        public enum EFileShare : uint
        {
            /// <summary>
            /// </summary>
            None = 0x00000000, 

            /// <summary>
            /// </summary>
            Read = 0x00000001, 

            /// <summary>
            /// </summary>
            Write = 0x00000002, 

            /// <summary>
            /// </summary>
            Delete = 0x00000004
        }

        /// <summary>
        /// </summary>
        [Flags]
        public enum EFileAttributes : uint
        {
            /// <summary>
            /// </summary>
            Readonly = 0x00000001, 

            /// <summary>
            /// </summary>
            Hidden = 0x00000002, 

            /// <summary>
            /// </summary>
            System = 0x00000004, 

            /// <summary>
            /// </summary>
            Directory = 0x00000010, 

            /// <summary>
            /// </summary>
            Archive = 0x00000020, 

            /// <summary>
            /// </summary>
            Device = 0x00000040, 

            /// <summary>
            /// </summary>
            Normal = 0x00000080, 

            /// <summary>
            /// </summary>
            Temporary = 0x00000100, 

            /// <summary>
            /// </summary>
            SparseFile = 0x00000200, 

            /// <summary>
            /// </summary>
            ReparsePoint = 0x00000400, 

            /// <summary>
            /// </summary>
            Compressed = 0x00000800, 

            /// <summary>
            /// </summary>
            Offline = 0x00001000, 

            /// <summary>
            /// </summary>
            NotContentIndexed = 0x00002000, 

            /// <summary>
            /// </summary>
            Encrypted = 0x00004000, 

            /// <summary>
            /// </summary>
            Write_Through = 0x80000000, 

            /// <summary>
            /// </summary>
            Overlapped = 0x40000000, 

            /// <summary>
            /// </summary>
            NoBuffering = 0x20000000, 

            /// <summary>
            /// </summary>
            RandomAccess = 0x10000000, 

            /// <summary>
            /// </summary>
            SequentialScan = 0x08000000, 

            /// <summary>
            /// </summary>
            DeleteOnClose = 0x04000000, 

            /// <summary>
            /// </summary>
            BackupSemantics = 0x02000000, 

            /// <summary>
            /// </summary>
            PosixSemantics = 0x01000000, 

            /// <summary>
            /// </summary>
            OpenReparsePoint = 0x00200000, 

            /// <summary>
            /// </summary>
            OpenNoRecall = 0x00100000, 

            /// <summary>
            /// </summary>
            FirstPipeInstance = 0x00080000
        }

        /// <summary>
        /// </summary>
        public enum ECreationDisposition : uint
        {
            /// <summary>
            /// </summary>
            New = 1, 

            /// <summary>
            /// </summary>
            CreateAlways = 2, 

            /// <summary>
            /// </summary>
            OpenExisting = 3, 

            /// <summary>
            /// </summary>
            OpenAlways = 4, 

            /// <summary>
            /// </summary>
            TruncateExisting = 5
        }

        /// <summary>
        /// Compares two directories
        /// </summary>
        /// <param name="directory1">
        /// First directory
        /// </param>
        /// <param name="directory2">
        /// Second directory
        /// </param>
        /// <returns>
        /// A value indicating whether the directories are on the same physical disc or not
        /// </returns>
        public bool CompareDirectories(string directory1, string directory2)
        {
            bool result = false;

            IntPtr fileHandle1 = CreateFile(
                directory1, 
                (uint)EFileAccess.GenericRead, 
                (uint)EFileShare.Read, 
                IntPtr.Zero, 
                (uint)ECreationDisposition.OpenExisting, 
                (uint)(EFileAttributes.Directory | EFileAttributes.BackupSemantics), 
                IntPtr.Zero);
            if (fileHandle1.ToInt32() != INVALID_HANDLE_VALUE)
            {
                BY_HANDLE_FILE_INFORMATION fileInfoFile1;
                bool rc = GetFileInformationByHandle(fileHandle1, out fileInfoFile1);
                if (rc)
                {
                    IntPtr fileHandle2 = CreateFile(
                        directory2, 
                        (uint)EFileAccess.GenericRead, 
                        (uint)EFileShare.Read, 
                        IntPtr.Zero, 
                        (uint)ECreationDisposition.OpenExisting, 
                        (uint)(EFileAttributes.Directory | EFileAttributes.BackupSemantics), 
                        IntPtr.Zero);
                    if (fileHandle2.ToInt32() != INVALID_HANDLE_VALUE)
                    {
                        BY_HANDLE_FILE_INFORMATION fileInfoFile2;
                        rc = GetFileInformationByHandle(fileHandle2, out fileInfoFile2);
                        if (rc)
                        {
                            if (fileInfoFile1.VolumeSerialNumber == fileInfoFile2.VolumeSerialNumber)
                            {
                                result = true;
                            }
                        }
                    }

                    CloseHandle(fileHandle2);
                }
            }

            CloseHandle(fileHandle1);

            return result;
        }

        /// <summary>
        /// Retrieves file information for the specified file.
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/aa364952%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="hFile">
        /// A handle to the file that contains the information to be retrieved. This handle should not be a pipe handle
        /// </param>
        /// <param name="lpFileInformation">
        /// A pointer to a <see cref="BY_HANDLE_FILE_INFORMATION"/> structure that receives the file information
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero and file information data is contained in the buffer pointed to by the lpFileInformation parameter. If the function fails, the return value is zero. To get extended error information, call GetLastError
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetFileInformationByHandle(
            IntPtr hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

        /// <summary>
        /// Creates or opens a file or I/O device. The most commonly used I/O devices are as follows: file, file stream, directory, physical disk, volume, console buffer, tape drive, communications resource, mailslot, and pipe. The function returns a handle that can be used to access the file or device for various types of I/O depending on the file or device and the flags and attributes specified.
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/aa363858%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="lpFileName">
        /// The name of the file or device to be created or opened. You may use either forward slashes (/) or backslashes (\) in this name
        /// </param>
        /// <param name="dwDesiredAccess">
        /// The requested access to the file or device, which can be summarized as read, write, both or neither zero)
        /// </param>
        /// <param name="dwShareMode">
        /// The requested sharing mode of the file or device, which can be read, write, both, delete, all of these, or none (refer to the following table). Access requests to attributes or extended attributes are not affected by this flag
        /// </param>
        /// <param name="lpSecurityAttributes">
        /// A pointer to a SECURITY_ATTRIBUTES structure that contains two separate but related data members: an optional security descriptor, and a Boolean value that determines whether the returned handle can be inherited by child processes
        /// </param>
        /// <param name="dwCreationDisposition">
        /// An action to take on a file or device that exists or does not exist
        /// </param>
        /// <param name="dwFlagsAndAttributes">
        /// The file or device attributes and flags, FILE_ATTRIBUTE_NORMAL being the most common default value for files
        /// </param>
        /// <param name="hTemplateFile">
        /// A valid handle to a template file with the GENERIC_READ access right. The template file supplies file attributes and extended attributes for the file that is being created
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is an open handle to the specified file, device, named pipe, or mail slot. If the function fails, the return value is INVALID_HANDLE_VALUE. To get extended error information, call GetLastError
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, 
            SetLastError = true)]
        private static extern IntPtr CreateFile(
            string lpFileName, 
            uint dwDesiredAccess, 
            uint dwShareMode, 
            IntPtr lpSecurityAttributes, 
            uint dwCreationDisposition, 
            uint dwFlagsAndAttributes, 
            IntPtr hTemplateFile);

        /// <summary>
        /// Closes an open object handle.
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/ms724211%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="hObject">
        /// A valid handle to an open object
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.If the function fails, the return value is zero. To get extended error information, call GetLastError
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// See "http://msdn.microsoft.com/en-us/library/windows/desktop/aa363788%28v=vs.85%29.aspx"
        /// </summary>
        private struct BY_HANDLE_FILE_INFORMATION
        {
            /// <summary>
            /// </summary>
            public uint FileAttributes;

            /// <summary>
            /// </summary>
            public FILETIME CreationTime;

            /// <summary>
            /// </summary>
            public FILETIME LastAccessTime;

            /// <summary>
            /// </summary>
            public FILETIME LastWriteTime;

            /// <summary>
            /// </summary>
            public uint VolumeSerialNumber;

            /// <summary>
            /// </summary>
            public uint FileSizeHigh;

            /// <summary>
            /// </summary>
            public uint FileSizeLow;

            /// <summary>
            /// </summary>
            public uint NumberOfLinks;

            /// <summary>
            /// </summary>
            public uint FileIndexHigh;

            /// <summary>
            /// </summary>
            public uint FileIndexLow;
        }
    }
}