// *******************************************************
// * <copyright file="TextToSpeechServiceFactory.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsTextToSpeechService
{
    using System.Speech.Synthesis;

    /// <summary>
    /// Represents the <see cref="TextToSpeechServiceFactory"/> class
    /// </summary>
    public class TextToSpeechServiceFactory
    {
        /// <summary>
        /// Creates the text to speech service.
        /// </summary>
        /// <returns>An instance of the <see cref="TextToSpeechService"/> class</returns>
        public static ITextToSpeechService CreateTextToSpeechService()
        {
            return new TextToSpeechService(new SpeechSynthesizer());
        }
    }
}