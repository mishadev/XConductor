using MonoTouch.AudioToolbox;

using System;
using System.IO;

using XConductor.Domain.iOS.Media.Writer.Settings;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Shared.Media.Writer;

namespace XConductor.Domain.iOS.Media.Writer
{
    public class AudioWriteriOS : AudioWriter
    {
        private AudioFile m_audioFile;
		private int m_bytePassed;

        public AudioWriteriOS(AudioWriterSettingsService settings)
            : base(settings)
        { }

        protected override void ChainedInitializeHook()
        {
			this.ClearFileds();

            var settings = (AudioWriterSettings)this.m_settings;

			AudioStreamBasicDescription formatDescription = AudioFormatDescriptionTools.ConvertToPlatformFormatDescription(settings.FormatDescription);

			this.m_audioFile = AudioFile.Create(settings.Url, settings.AudioFileType, formatDescription, AudioFileFlags.EraseFlags);

            this.OnStop += (s, e) => { if (this.m_audioFile != null) this.m_audioFile.Dispose(); };
        }

		private void ClearFileds()
		{
			this.m_audioFile = default(AudioFile);
			this.m_bytePassed = default(int);
		}

        protected override byte[] Processing(byte[] input)
        {
			AudioFormatDescriptionTools.PlatformReverseConvertation(input, this.m_settings.FormatDescription.BitsPerChannel, this.m_settings.FormatDescription.ChannelsPerFrame);

			this.m_audioFile.Write(this.m_bytePassed, input, 0, input.Length, useCache: false);
            
			this.m_bytePassed += input.Length;

            return input;
        }

        protected override void InitSettings(ISettings settings)
        {
            this.m_settings = settings as AudioWriterSettings;
        }

        public override void Dispose()
        {
            if (this.m_audioFile != null) this.m_audioFile.Dispose();

            base.Dispose();
        }
    }
}
