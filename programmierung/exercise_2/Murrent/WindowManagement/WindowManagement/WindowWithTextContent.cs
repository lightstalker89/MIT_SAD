// *******************************************************
// * <copyright file="WindowWithTextContent.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

namespace WindowManagement
{
    using System;

    /// <summary>
    /// WindowWithTextContent child class and interaction logic
    /// </summary>
    public class WindowWithTextContent : Window
    {
        /// <inheritdoc/>
        public override void Draw(char? leftTopChar = null, char? rightTopChar = null, char? leftBottomChar = null, char? rightBottomChar = null, char? leftChar = null, char? rightChar = null, char? topChar = null, char? bottomChar = null, char? shadowChar = null)
        {
            base.Draw(leftTopChar, rightTopChar, leftBottomChar, rightBottomChar, leftChar, rightChar, topChar, bottomChar, shadowChar);
        }
    }
}