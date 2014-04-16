// *******************************************************
// * <copyright file="IVolumeSplitter.cs" company="FGrill">
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

    /// <summary>
    /// Interface representing the <see cref="IVolumeSplitter"/>
    /// </summary>
    public interface IVolumeSplitter
    {
        /// <summary>
        /// Splits the folders by volume serial.
        /// </summary>
        /// <param name="folderList">The folders list.</param>
        /// <returns>Returns the distributed folder list</returns>
        List<List<string>> SplitFoldersByVolumeSerial(List<string> folderList);
    }
}
