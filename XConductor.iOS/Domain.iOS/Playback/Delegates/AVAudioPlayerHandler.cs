using MonoTouch.AVFoundation;
using MonoTouch.Foundation;

using System;

namespace XConductor.Domain.iOS.Playback.Delegates
{
    public class AVAudioPlayerHandler : AVAudioPlayerDelegate
    {
        public event Action<AVAudioPlayer, bool> OnPlayFinish;
        public event Action<AVAudioPlayer> OnInterruptionBegin;
        public event Action<AVAudioPlayer, NSError> OnDecoderError;
        public event Action<AVAudioPlayer, AVAudioSessionInterruptionFlags> OnInterruption;

        private void PlayFinished(AVAudioPlayer player, bool flag)
        {
            if (this.OnPlayFinish != null)
            {
                this.OnPlayFinish(player, flag);
            }
        }

        private void InterruptionBegined(AVAudioPlayer player)
        {
            if (this.OnInterruptionBegin != null)
            {
                this.OnInterruptionBegin(player);
            }
        }

        private void DecoderErrored(AVAudioPlayer player, NSError error)
        {
            if (this.OnDecoderError != null)
            {
                this.OnDecoderError(player, error);
            }
        }

        private void InterruptionEnded(AVAudioPlayer player, AVAudioSessionInterruptionFlags flags)
        {
            if (this.OnInterruption != null)
            {
                this.OnInterruption(player, flags);
            }
        }

        public override void FinishedPlaying(AVAudioPlayer player, bool flag)
        {
            this.PlayFinished(player, flag);
        }

        public override void BeginInterruption(AVAudioPlayer player)
        {
            this.InterruptionBegined(player);
        }

        public override void EndInterruption(AVAudioPlayer player, AVAudioSessionInterruptionFlags flags)
        {
            this.InterruptionEnded(player, flags);
        }

        public override void DecoderError(AVAudioPlayer player, NSError error)
        {
            this.DecoderErrored(player, error);
        }
    }
}
