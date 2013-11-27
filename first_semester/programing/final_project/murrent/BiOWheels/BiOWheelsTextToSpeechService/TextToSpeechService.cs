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
        /// The speech synthesizer
        /// </summary>
        private readonly SpeechSynthesizer speechSynthesizer;

        /// <summary>
        /// The is in progress
        /// </summary>
        internal bool IsInProgress = false;

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
            this.speechSynthesizer.SpeakCompleted += this.SpeechSynthesizerSpeakCompleted;
        }

        /// <summary>
        /// Speeches the synthesizer speak completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeakCompletedEventArgs"/> instance containing the event data.</param>
        protected void SpeechSynthesizerSpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            this.IsInProgress = false;
        }

        #region Methods

        /// <inheritdoc/>
        public void Speack(string text)
        {
            if (!this.IsInProgress)
            {
                this.IsInProgress = true;
                this.speechSynthesizer.SetOutputToDefaultAudioDevice();
                this.speechSynthesizer.Rate = 0;
                this.speechSynthesizer.Volume = 100;
                this.speechSynthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
                this.speechSynthesizer.SpeakAsync(text);
            }
        }

        #endregion
    }
}