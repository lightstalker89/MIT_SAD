// *******************************************************
// * <copyright file="WindowWithSigns.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace WindowManagement
{
    /// <summary>
    ///  WindowWithSigns child class and interaction logic
    /// </summary>
    public class WindowWithSigns : Window
    {
        /// <summary>
        /// Array holding the signs
        /// </summary>
        private readonly char[] signs;

        /// <summary>
        /// Specifies how often a sign should be drawn
        /// </summary>
        private readonly int occurence;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowWithSigns"/> class
        /// </summary>
        /// <param name="signs">
        /// Signs to be drawn on the screen
        /// </param>
        /// <param name="occurence">
        /// Specify  how often every sign should be drawn
        /// </param>
        public WindowWithSigns(char[] signs, int occurence)
        {
            this.signs = signs;
            this.occurence = occurence;
        }

        /// <inheritdoc />
        public override void Draw(char? leftTopChar = null, char? rightTopChar = null, char? leftBottomChar = null, char? rightBottomChar = null, char? leftChar = null, char? rightChar = null, char? topChar = null, char? bottomChar = null, char? shadowChar = null)
        {
            this.Content = this.signs.ToStringExtended(this.occurence);
            base.Draw(leftTopChar, rightTopChar, leftBottomChar, rightBottomChar, leftChar, rightChar, topChar, bottomChar, shadowChar);
        }
    }
}