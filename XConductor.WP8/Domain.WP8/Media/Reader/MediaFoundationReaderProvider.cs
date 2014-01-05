using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Shared.Media;
using XConductor.Domain.Shared.Media.Reader;
using XConductor.Domain.W8.Media.Reader.Settings;
using XConductor.Domain.W8.MediaFoundation;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.W8.Media.Reader
{
    public class MediaFoundationAudioReader : AudioReader
    {
        private IRandomAccessStream m_fileStream;
        private MediaFoundationReaderSettings m_readerSettings;
        private IMFSourceReader m_pReader;
        private IDataReadable<byte[]> m_reader;
        private AudioReaderSettings m_settings;
        private IAudioFormatDescription m_audioFormat;
        private bool m_canceled = false;

        public MediaFoundationAudioReader(AudioReaderSettingsService settingsService)
            : base(settingsService)
        { }

        private AudioFormatDescription GetAudioFormatDescription(IMFSourceReader reader)
        {
            IMFMediaType uncompressedMediaType;
            reader.GetCurrentMediaType(MediaFoundationInterop.MF_SOURCE_READER_FIRST_AUDIO_STREAM, out uncompressedMediaType);

            var mt = new MediaType(uncompressedMediaType);
            AudioFormatDescription af = null;

            if (mt.SubType == AudioSubtypes.MFAudioFormat_PCM)
            {
                af = new AudioFormatDescription
                {
                    SampleRate = mt.SampleRate,
                    BitsPerChannel = (short)mt.BitsPerSample,
                    ChannelsPerFrame = (short)mt.ChannelCount,
                };
            }

            return af;
        }

        private IMFSourceReader CreateReader(IRandomAccessStream filePath, MediaFoundationReaderSettings settings)
        {
            IMFByteStream stream;
            MediaFoundationInterop.MFCreateMFByteStreamOnStreamEx(filePath, out stream);

            IMFSourceReader reader;
            MediaFoundationInterop.MFCreateSourceReaderFromByteStream(stream, null, out reader);

            reader.SetStreamSelection(MediaFoundationInterop.MF_SOURCE_READER_ALL_STREAMS, false);
            reader.SetStreamSelection(MediaFoundationInterop.MF_SOURCE_READER_FIRST_AUDIO_STREAM, true);

            MediaType expectedMeaditype = this.CreateMediaType();
            IMFMediaType mediaType = expectedMeaditype.MediaFoundationObject;

            reader.SetCurrentMediaType(MediaFoundationInterop.MF_SOURCE_READER_FIRST_AUDIO_STREAM, IntPtr.Zero, mediaType);

            var pv = PropVariant.FromLong(0L);
            reader.SetCurrentPosition(Guid.Empty, ref pv);

            Marshal.ReleaseComObject(stream);
            Marshal.ReleaseComObject(mediaType);

            return reader;
        }

        private MediaType CreateMediaType()
        {
            var partialMediaType = new MediaType();

            partialMediaType.MajorType = this.m_readerSettings.MajorType;
            partialMediaType.SubType = this.m_readerSettings.SubType;

            return partialMediaType;
        }

        private IDataReadable<byte[]> CreatePullSynchronizator(IMFSourceReader pReader)
        {
            var sReader = new MediaFoundationSampleReader(pReader);
            return new PullSynchronizator<byte[]>(sReader);
        }

        public override void Dispose()
        {
            this.m_fileStream.Dispose();
            Marshal.ReleaseComObject(this.m_pReader);

            base.Dispose();
        }

        protected override void InitSettings(ISettings settings)
        {
            this.m_settings = settings as AudioReaderSettings;
        }

        protected override void ObservableInitializeHook()
        {
            this.m_fileStream = this.m_settings.FileStream;
            this.m_readerSettings = this.m_settings.MFReaderSettings;

            MediaFoundationApi.Startup();
            this.m_pReader = this.CreateReader(this.m_fileStream, this.m_readerSettings);
            this.m_reader = this.CreatePullSynchronizator(this.m_pReader);

            this.m_audioFormat = this.GetAudioFormatDescription(this.m_pReader);
        }

        protected override Task StopHook()
        {
            return new Task(() =>
            {
                this.m_canceled = true;
            });
        }

        protected override Task StartHook()
        {
            return new Task(() =>
            {
                this.m_canceled = false;

                int count = this.m_settings.BufferSize;
                byte[] buffer = new byte[this.m_settings.BufferSize];
                while (count != 0 && !this.m_canceled)
                {
                    count = this.m_reader.Read(buffer, this.m_settings.BufferSize);

                    if (count == 0) break;

                    this.DataAvalable(buffer);
                }
                this.Stop().Await();
            });
        }

        public override ISettings Settings
        {
            get { return this.m_settings; }
        }

        public override IAudioFormatDescription AudioFormat
        {
            get { return this.m_audioFormat; }
        }
    }
}
