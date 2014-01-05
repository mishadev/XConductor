using System;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Analyzations;
using XConductor.Domain.Seedwork.Analyzations.Settings;
using XConductor.Domain.Seedwork.Common;
using XConductor.Domain.Shared.Abstractions;
using XConductor.Domain.Shared.Analyzations.Settings;
using XConductor.Domain.Shared.Tools.Metrics;

namespace XConductor.Domain.Shared.Analyzations
{
    public class AudioAnalyzer : ChainedDomainService<IAudioAnalyzerSettings, float[], float[]>, IAudioAnalyzer
    {
        private long m_bytesProcessed;
        private JumpDetector m_detector;
        private int m_bytesPerSecond;

        public AudioAnalyzer(AudioAnalyzerSettingsService settingsService)
            : base(settingsService)
        { }

        protected override void ChainedInitializeHook()
        {
            this.m_bytesProcessed = 0;
            this.m_detector = new JumpDetector(this.m_settings.MaxDeviation, this.m_settings.RangeDeviation, this.m_settings.MinMeaningfulValue);
            this.m_bytesPerSecond =
                this.m_settings.FormatDescription.BytesPerFrame *
                this.m_settings.FormatDescription.SampleRate;
        }

        protected override float[] Processing(float[] input)
        {
            this.m_bytesProcessed += input.Length * this.m_settings.FormatDescription.BytesPerFrame;

            if (this.m_detector.IsJumpDetect(input))
            {
                this.ResultsAvalable(this.CurrentTime());
            }

            return input;
        }

        private double CurrentTime()
        {
            return this.m_bytesProcessed / (double)this.m_bytesPerSecond;
        }

        protected override void InitSettings(ISettings settings)
        {
            this.m_settings = settings as AudioAnalyzerSettings;
        }
    }
}
