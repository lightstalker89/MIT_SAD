// *******************************************************
// * <copyright file="FileComparator.cs" company="MDMCoWorks">
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
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileComparator"/> class
    /// </summary>
    internal class FileComparator
    {
        #region Private Fields

        /// <summary>
        /// Field holding the MD5 hasher
        /// </summary>
        private MD5 md5Hasher;

        /// <summary>
        /// Field indicating the block size used for comparing files
        /// </summary>
        private long blockSize;

        #endregion

        /// <summary>
        /// </summary>
        public FileComparator()
        {
            this.MD5Hasher = MD5.Create();
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating the block size used for comparing files
        /// </summary>
        public long BlockSize
        {
            get
            {
                return this.blockSize;
            }

            set
            {
                this.blockSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the MD5 hasher
        /// </summary>
        internal MD5 MD5Hasher
        {
            get
            {
                return this.md5Hasher;
            }

            set
            {
                this.md5Hasher = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="sourceFile">
        /// </param>
        /// <param name="destinationFile">
        /// </param>
        /// <returns>
        /// </returns>
        internal void Compare(string sourceFile, string destinationFile)
        {
            using (Stream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read),
             destinationStream = new FileStream(destinationFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                long fileLengthSource = sourceStream.Length;
                long fileLengthDestination = destinationStream.Length;

                if (fileLengthSource > 0 && fileLengthDestination > 0)
                {
                    byte[] bufferSource = new byte[this.BlockSize];
                    byte[] bufferDestination = new byte[this.BlockSize];

                    while ((sourceStream.Read(bufferSource, 0, bufferSource.Length) != 0))
                    {
                        destinationStream.Read(bufferDestination, 0, bufferDestination.Length);

                        if (!(this.md5Hasher.ComputeHash(bufferSource).Equals(this.md5Hasher.ComputeHash(bufferDestination))))
                        {
                            int bufferLength = bufferSource.ToList().Count(p => p != 0);

                            destinationStream.Position = sourceStream.Position - bufferLength;
                            destinationStream.Write(bufferSource, 0, bufferLength);


                            // TODO: trigger event to log which block has been changed
                        }
                    }

                    destinationStream.Flush();
                }
                else
                {
                    // TODO: refactor QueueManager and create FileManager -> extract methods
                }
            }
        }

        #endregion
    }
}