//-----------------------------------------------------------------------
// <copyright file="VdmGen.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;

    /// <summary>
    /// This class provides the template with access to the UML model store
    /// </summary>
    public partial class VdmGen
    {
        /// <summary>
        /// 
        /// </summary>
        private IModelStore store;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public VdmGen(IModelStore s)
        { 
            store = s;
        }
    }
}
