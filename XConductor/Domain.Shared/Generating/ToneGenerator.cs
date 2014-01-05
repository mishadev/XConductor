using System.Threading.Tasks;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Generating;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Shared.Abstractions;
using XConductor.Domain.Shared.Generating.Settings;
using XConductor.Domain.Shared.Tools.Generators;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.Shared.Generating
{
    public class ToneGenerator : ObservableDomainService<byte[]>, IToneGenerator
    {
        private AudioGenerator m_generator;
        private bool m_canceled;
        private ToneGeneratorSettings m_settings;

        public ToneGenerator(ToneGeneratorSettingsService settingsService)
            : base(settingsService)
        { }

        protected async override Task InitializeHook()
        {
			this.m_generator = new AudioGenerator(
				this.m_settings.Amplitude,
				this.m_settings.Configurations,
				this.m_settings.UsePhaseShifting,
				this.m_settings.PeakCount,
				this.m_settings.PeakGap,
				this.m_settings.PeakChannelGap,
				this.m_settings.PeakTime,
				this.m_settings.AudioFormat);
        }

        public override IAudioFormatDescription AudioFormat
        {
            get { return this.m_settings.AudioFormat; }
        }

        protected override async Task StopHook()
        {
			this.m_canceled = true;
        }

        protected override async Task StartHook()
        {
            this.m_canceled = false;

            int count = this.m_settings.BufferSize;
            long generated = 0;
            while (count != 0 && !this.m_canceled)
            {
				byte[] buffer = new byte[this.m_settings.BufferSize];

                count = this.m_generator.Generate(buffer, generated);

                generated += count;

                this.DataAvalable(buffer);
            }

            await this.Stop();
        }

        protected override void InitSettings(ISettings settings)
        {
            this.m_settings = settings as ToneGeneratorSettings;
        }

        public override ISettings Settings
        {
            get { return this.m_settings; }
        }
    }
}
