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
    ///  Class representing the <see cref="VisualizerFactory"/>
    /// </summary>
    public class VisualizerFactory
    {
        /// <summary>
        /// Creates the visualizer.
        /// </summary>
        /// <returns>An instance of the <see cref="Visualizer"/></returns>
        public static IVisualizer CreateVisualizer()
        {
            return new Visualizer();
        }
    }
}