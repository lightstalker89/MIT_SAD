// *******************************************************
// * <copyright file="VisualizerFactory.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
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
    public class VisualizerFactory
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static IVisualizer CreateVisualizer()
        {
            return new Visualizer();
        }
    }
}