// *******************************************************
// * <copyright file="TextToSpeechService.cs" company="MDMCoWorks">
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
    using System.Threading.Tasks;

    /// <summary>
    /// Class representing the <see cref="TextToSpeechService"/>
    /// </summary>
    public class TextToSpeechService : ITextToSpeechService
    {
        #region Private Fields


        /// <summary>
        /// The is in progress
        /// </summary>
        internal bool IsInProgress = false;

        /// <summary>
        /// The speech synthesizer
        /// </summary>
        private readonly SpeechSynthesizer speechSynthesizer;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TextToSpeechService"/> class.
        /// </summary>
        /// <param name="speechSynthesizer">
        /// The speech synthesizer.
        /// </param>
        public TextToSpeechService(SpeechSynthesizer speechSynthesizer)
        {
            this.speechSynthesizer = speechSynthesizer;
            this.speechSynthesizer.SetOutputToDefaultAudioDevice();
            this.speechSynthesizer.Rate = 0;
            this.speechSynthesizer.Volume = 100;
            this.speechSynthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
        }

        #region Methods

        /// <inheritdoc/>
        public void Speak(string text)
        {
            this.speechSynthesizer.SpeakAsyncCancelAll();

            this.speechSynthesizer.SpeakAsync(text);
        }

        #endregion
    }
}