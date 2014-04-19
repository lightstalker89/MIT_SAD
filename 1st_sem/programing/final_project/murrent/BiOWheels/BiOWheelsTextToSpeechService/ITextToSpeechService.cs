// *******************************************************
// * <copyright file="ITextToSpeechService.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsTextToSpeechService
{
    /// <summary>
    /// Interface representing the <see cref="ITextToSpeechService"/>
    /// </summary>
    public interface ITextToSpeechService
    {
        /// <summary>
        /// Speaks the specified text.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        void Speak(string text);
    }
}