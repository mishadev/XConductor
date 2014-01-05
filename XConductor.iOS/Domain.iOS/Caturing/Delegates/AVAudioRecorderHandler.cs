using MonoTouch.AVFoundation;
using MonoTouch.Foundation;

using System;

namespace XConductor.Domain.iOS.Caturing.Delegates
{
    internal class AVAudioRecorderHandler : AVAudioRecorderDelegate
    {
        public event Action<AVAudioRecorder, bool> OnRecordFinish;
        public event Action<AVAudioRecorder> OnInterruptionBegin;
        public event Action<AVAudioRecorder, NSError> OnEncoderError;
        public event Action<AVAudioRecorder, AVAudioSessionInterruptionFlags> OnInterruption;

        private void RecordFinished(AVAudioRecorder recorder, bool flag)
        {
            if (this.OnRecordFinish != null)
            {
                this.OnRecordFinish(recorder, flag);
            }
        }

        private void InterruptionBegined(AVAudioRecorder recorder)
        {
            if (this.OnInterruptionBegin != null)
            {
                this.OnInterruptionBegin(recorder);
            }   
        }

        private void EncoderErrored(AVAudioRecorder recorder, NSError error)
        {
            if (this.OnEncoderError != null)
            {
                this.OnEncoderError(recorder, error);
            }
        }

        private void InterruptionEnded(AVAudioRecorder recorder, AVAudioSessionInterruptionFlags flags)
        {
            if (this.OnInterruption != null)
            {
                this.OnInterruption(recorder, flags);
            }
        }

        public override void FinishedRecording(AVAudioRecorder recorder, bool flag)
        {
            this.RecordFinished(recorder, flag);
        }

        public override void BeginInterruption(AVAudioRecorder recorder)
        {
            this.InterruptionBegined(recorder);
        }

        public override void EncoderError(AVAudioRecorder recorder, NSError error)
        {
			this.EncoderErrored (recorder, error);
        }

        public override void EndInterruption(AVAudioRecorder recorder, AVAudioSessionInterruptionFlags flags)
        {
            this.InterruptionEnded(recorder, flags);
        }
    }
}
