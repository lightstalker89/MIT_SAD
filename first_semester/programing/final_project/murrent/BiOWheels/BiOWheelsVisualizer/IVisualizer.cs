// *******************************************************
// * <copyright file="IVisualizer.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsVisualizer
{
    /// <summary>
    /// </summary>
    public interface IVisualizer
    {
        /// <summary>
        /// </summary>
        void GetMenu();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        void WriteLog(string entry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        void WriteLine(string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        void WriteText(string text);
    }
}