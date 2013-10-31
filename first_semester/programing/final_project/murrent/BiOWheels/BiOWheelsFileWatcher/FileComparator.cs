// *******************************************************
// * <copyright file="FileComparator.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace BiOWheelsFileWatcher
{
    /// <summary>
    /// Class representing the <see cref="FileComparator"/> and its interaction logic
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
        /// 
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="destinationFile"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal bool Compare(string sourceFile, string destinationFile, int offset)
        {
            //int actualOffset = offset;

            //using (FileStream fileStreamSource = new FileStream(sourceFile, FileMode.Open),
            //       fileStreamDestination = new FileStream(destinationFile, FileMode.Open))
            //{

            //    byte[] bufferSource = new byte[this.BlockSize];
            //    byte[] bufferDestination = new byte[this.BlockSize];

            //    actualOffset += bufferSource.Length;

            //    int resultSource = fileStreamSource.Read(bufferSource, actualOffset, bufferSource.Length);
            //    int resultDestination = fileStreamDestination.Read(bufferDestination, actualOffset, bufferDestination.Length);

            //    if (!(this.md5Hasher.ComputeHash(bufferSource).Equals(this.md5Hasher.ComputeHash(bufferDestination))))
            //    {
            //        // TODO: copy;
            //        return false;
            //    }

            //    this.Compare(sourceFile, destinationFile, actualOffset);
            //}

            return true;
        }

        #endregion
    }
}