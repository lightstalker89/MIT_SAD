// *******************************************************
// * <copyright file="CustomAVLTreeNode.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace AVLTree
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// </summary>
    /// <typeparam name="Type">
    /// </typeparam>
    public class CustomAVLTreeNode<Type>
        where Type : IComparable<Type>
    {
        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        public CustomAVLTreeNode(Type value)
        {
            NodeValue = value;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return Convert.ToString(NodeValue);
        }

        /// <summary>
        /// </summary>
        internal Type NodeValue { get; set; }

        /// <summary>
        /// </summary>
        internal CustomAVLTreeNode<Type> Parent { get; set; }

        /// <summary>
        /// </summary>
        internal CustomAVLTreeNode<Type> LeftChild { get; set; }

        /// <summary>
        /// </summary>
        internal CustomAVLTreeNode<Type> RightChild { get; set; }
    }
}