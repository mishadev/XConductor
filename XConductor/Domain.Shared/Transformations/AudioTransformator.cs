using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Transformations;
using XConductor.Domain.Seedwork.Transformations.Settings;
using XConductor.Domain.Shared.Abstractions;
using XConductor.Domain.Shared.Tools.Dsp;
using XConductor.Domain.Shared.Transformations.Settings;

namespace XConductor.Domain.Shared.Transformations
{
    public class AudioTransformator : ChainedDomainService<IAudioTransformatorSettings, byte[], float[]>, IAudioTransformator
    {
        private FastFourierTransformator m_transformator;

        public AudioTransformator(AudioTransformatorSettingsService settingsService)
            : base(settingsService)
        { }

        protected override void ChainedInitializeHook()
        {
            this.m_transformator = new FastFourierTransformator(this.m_settings.FormatDescription, this.m_settings.OutputSize);
        }

        protected override float[] Processing(byte[] input)
        {
            return this.m_transformator.Transform(input);
        }

        protected override void InitSettings(ISettings settings)
        {
            this.m_settings = settings as AudioTransformatorSettings;
        }
    }
}
