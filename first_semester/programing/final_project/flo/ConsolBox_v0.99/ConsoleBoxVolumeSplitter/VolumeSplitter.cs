// *******************************************************
// * <copyright file="VolumeSplitter.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxVolumeSplitter
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Interface representing the <see cref="VolumeSplitter"/>
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
    public class VolumeSplitter : IVolumeSplitter
    {
        /// <summary>
        /// The invalid handle value
        /// </summary>
        public const short InvalidHandleValue = -1;

        /// <summary>
        /// The EFileAccess enumeration
        /// </summary>
        [Flags]
        public enum EFileAccess : uint
        {
            /// <summary>
            /// The generic read
            /// </summary>
            GenericRead = 0x80000000,

            /// <summary>
            /// The generic write
            /// </summary>
            GenericWrite = 0x40000000,

            /// <summary>
            /// The generic execute
            /// </summary>
            GenericExecute = 0x20000000,

            /// <summary>
            /// The generic all
            /// </summary>
            GenericAll = 0x10000000
        }

        /// <summary>
        /// The EFileShare enumeration
        /// </summary>
        [Flags]
        public enum EFileShare : uint
        {
            /// <summary>
            /// The none
            /// </summary>
            None = 0x00000000,

            /// <summary>
            /// The read
            /// </summary>
            Read = 0x00000001,

            /// <summary>
            /// The write
            /// </summary>
            Write = 0x00000002,

            /// <summary>
            /// The delete
            /// </summary>
            Delete = 0x00000004
        }

        /// <summary>
        /// The EFileAttributes enumeration
        /// </summary>
        [Flags]
        public enum EFileAttributes : uint
        {
            /// <summary>
            /// The readonly
            /// </summary>
            Readonly = 0x00000001,

            /// <summary>
            /// The hidden
            /// </summary>
            Hidden = 0x00000002,

            /// <summary>
            /// The system
            /// </summary>
            System = 0x00000004,

            /// <summary>
            /// The directory
            /// </summary>
            Directory = 0x00000010,

            /// <summary>
            /// The archive
            /// </summary>
            Archive = 0x00000020,

            /// <summary>
            /// The device
            /// </summary>
            Device = 0x00000040,

            /// <summary>
            /// The normal
            /// </summary>
            Normal = 0x00000080,

            /// <summary>
            /// The temporary
            /// </summary>
            Temporary = 0x00000100,

            /// <summary>
            /// The sparse file
            /// </summary>
            SparseFile = 0x00000200,

            /// <summary>
            /// The reparse point
            /// </summary>
            ReparsePoint = 0x00000400,

            /// <summary>
            /// The compressed
            /// </summary>
            Compressed = 0x00000800,

            /// <summary>
            /// The offline
            /// </summary>
            Offline = 0x00001000,

            /// <summary>
            /// The not content indexed
            /// </summary>
            NotContentIndexed = 0x00002000,

            /// <summary>
            /// The encrypted
            /// </summary>
            Encrypted = 0x00004000,

            /// <summary>
            /// The write_ through
            /// </summary>
            WriteThrough = 0x80000000,

            /// <summary>
            /// The overlapped
            /// </summary>
            Overlapped = 0x40000000,

            /// <summary>
            /// The no buffering
            /// </summary>
            NoBuffering = 0x20000000,

            /// <summary>
            /// The random access
            /// </summary>
            RandomAccess = 0x10000000,

            /// <summary>
            /// The sequential scan
            /// </summary>
            SequentialScan = 0x08000000,

            /// <summary>
            /// The delete on close
            /// </summary>
            DeleteOnClose = 0x04000000,

            /// <summary>
            /// The backup semantics
            /// </summary>
            BackupSemantics = 0x02000000,

            /// <summary>
            /// The posix semantics
            /// </summary>
            PosixSemantics = 0x01000000,

            /// <summary>
            /// The open reparse point
            /// </summary>
            OpenReparsePoint = 0x00200000,

            /// <summary>
            /// The open no recall
            /// </summary>
            OpenNoRecall = 0x00100000,

            /// <summary>
            /// The first pipe instance
            /// </summary>
            FirstPipeInstance = 0x00080000
        }

        /// <summary>
        /// The ECreationDisposition enumeration
        /// </summary>
        public enum ECreationDisposition : uint
        {
            /// <summary>
            /// The new
            /// </summary>
            New = 1,

            /// <summary>
            /// The create always
            /// </summary>
            CreateAlways = 2,

            /// <summary>
            /// The open existing
            /// </summary>
            OpenExisting = 3,

            /// <summary>
            /// The open always
            /// </summary>
            OpenAlways = 4,

            /// <summary>
            /// The truncate existing
            /// </summary>
            TruncateExisting = 5
        }

        /// <inheritdoc/>
        public List<List<string>> SplitFoldersByVolumeSerial(List<string> folderList)
        {
            List<KeyValuePair<long, string>> folderVolume = new List<KeyValuePair<long, string>>();
            List<long> serials = new List<long>();
            foreach (var folder in folderList)
            {
                IntPtr fileHandle1 = CreateFile(folder, (uint)EFileAccess.GenericRead, (uint)EFileShare.Read, IntPtr.Zero, (uint)ECreationDisposition.OpenExisting, (uint)(EFileAttributes.Directory | EFileAttributes.BackupSemantics), IntPtr.Zero);
                if (fileHandle1.ToInt32() != InvalidHandleValue)
                {
                    ByHandleFileInformation fileInfoFile1;
                    bool rc = GetFileInformationByHandle(fileHandle1, out fileInfoFile1);
                    if (rc)
                    {
                        long result = (long)fileInfoFile1.VolumeSerialNumber;
                        folderVolume.Add(new KeyValuePair<long, string>(result, folder));
                        if (!serials.Contains(result))
                        {
                            serials.Add(result);
                        } 
                    }
                }

                CloseHandle(fileHandle1);
            }

            return serials.Select(serial => (from c in folderVolume
                where c.Key == serial
                select new
                {
                    Value = c.Value
                }).Select(x => x.Value).ToList()).ToList();
        }

        /// <summary>
        /// Retrieves file information for the specified file.
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/aa364952%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="hFile">
        /// A handle to the file that contains the information to be retrieved. This handle should not be a pipe handle
        /// </param>
        /// <param name="lpFileInformation">
        /// A pointer to a <see cref="ByHandleFileInformation"/> structure that receives the file information
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero and file information data is contained in the buffer pointed to by the lpFileInformation parameter. If the function fails, the return value is zero. To get extended error information, call GetLastError
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetFileInformationByHandle(IntPtr hFile, out ByHandleFileInformation lpFileInformation);

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
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern IntPtr CreateFile(String lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode, IntPtr lpSecurityAttributes, UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, IntPtr hTemplateFile);

        /// <summary>
        /// Closes an open object handle.
        /// See http://msdn.microsoft.com/en-us/library/windows/desktop/ms724211%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="hObject">
        /// A valid handle to an open object
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// The file information struct.
        /// </summary>
        private struct ByHandleFileInformation
        {
            /// <summary>
            /// The file attributes
            /// </summary>
            public uint FileAttributes;

            /// <summary>
            /// The creation time
            /// </summary>
            public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;

            /// <summary>
            /// The last access time
            /// </summary>
            public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;

            /// <summary>
            /// The last write time
            /// </summary>
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;

            /// <summary>
            /// The volume serial number
            /// </summary>
            public uint VolumeSerialNumber;

            /// <summary>
            /// The file size high
            /// </summary>
            public uint FileSizeHigh;

            /// <summary>
            /// The file size low
            /// </summary>
            public uint FileSizeLow;

            /// <summary>
            /// The number of links
            /// </summary>
            public uint NumberOfLinks;

            /// <summary>
            /// The file index high
            /// </summary>
            public uint FileIndexHigh;

            /// <summary>
            /// The file index low
            /// </summary>
            public uint FileIndexLow;
        }
    }
}
