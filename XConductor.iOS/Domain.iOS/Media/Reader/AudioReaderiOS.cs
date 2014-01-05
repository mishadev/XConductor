using MonoTouch.AudioToolbox;

using System.Threading.Tasks;
using System;

using XConductor.Domain.iOS.Media.Reader.Settings;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Shared.Media.Reader;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.iOS.Media.Reader
{
    public class AudioReaderiOS : AudioReader
    {
        private AudioReaderSettings m_settings;
        private IAudioFormatDescription m_audioFormat;
        private AudioFile m_audioFile;
        private bool m_canceled = false;

        public AudioReaderiOS(AudioReaderSettingsService settingsService)
            : base(settingsService)
        { }

        protected override void InitSettings(ISettings settings)
        {
			this.m_settings = settings as AudioReaderSettings;
        }

        protected override async Task StopHook()
        {
			this.m_canceled = true;
        }

        protected override async Task StartHook()
        {
			byte[] buffer = new byte[this.m_settings.BufferSize];
			int readed = this.m_settings.BufferSize;
			long offset = 0;
			int size = buffer.Length;
			long totalLength = this.m_audioFile.Length;

			while (size < (totalLength - offset) && !this.m_canceled)
			{
				readed = this.m_audioFile.Read(offset, buffer, 0, size, useCache: false);
				offset += readed;

				AudioFormatDescriptionTools.PlatformReverseConvertation(buffer, this.m_audioFormat.BitsPerChannel, this.m_audioFormat.ChannelsPerFrame);

				this.DataAvalable(buffer);
			}
			await this.Stop();
        }

        public override ISettings Settings
        {
            get { return this.m_settings; }
        }

        public override IAudioFormatDescription AudioFormat
        {
            get { return this.m_audioFormat; }
        }

        protected override async Task InitializeHook()
        {
			this.m_canceled = false;

			this.m_audioFile = AudioFile.OpenRead(this.m_settings.Url, this.m_settings.AudioFileType);

			this.m_audioFormat = AudioFormatDescriptionTools.ConvertToStandsrtFormatDescription(this.m_audioFile.StreamBasicDescription);
        }
    }
}
