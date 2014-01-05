using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Windows.Media;
using Windows.Media.Capture;
using Windows.Storage;

using XConductor.Domain.Seedwork.Capturing;
using XConductor.Domain.Shared.Caturing;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

using XConductor.Domain.W8.Capturing.Settings;
using XConductor.Domain.Seedwork.Abstractions.Settings;

namespace XConductor.Domain.W8.Capturing
{
    public class CaptureSessionW8 : CaptureSession
    {
        private MediaCapture m_mediaCapture;
        private CaptureSettings m_settings;

        public override ISettings Settings
        {
            get { return this.m_settings; }
        }

        public CaptureSessionW8(CaptureSettingsService settingsService)
            : base(settingsService)
        { }

        protected override void InitSettings(ISettings settings)
        {
            this.m_settings = settings as CaptureSettings;
        }

        protected override Task InitializeHook()
        {
            this.m_mediaCapture = new MediaCapture();

            var task = this.MediaCaptureInitializeAsync();

            this.InitEventHandler();

            return task;
        }

        private void InitEventHandler()
        {
            MediaControl.SoundLevelChanged += (s, e) => this.SoundLeveChanged(e);

            this.m_mediaCapture.RecordLimitationExceeded += e => this.Interrupted(e);
            this.m_mediaCapture.Failed += (s, e) => this.Error(s, e.Message);
        }

        private Task MediaCaptureInitializeAsync()
        {
            if (this.m_settings.InitializationSettings == null)
            {
                return this.m_mediaCapture.InitializeAsync().AsTask();
            }
            else
            {
                return this.m_mediaCapture.InitializeAsync(this.m_settings.InitializationSettings).AsTask();
            }
        }

        protected override Task StartHook()
        {
            switch (this.m_settings.CaptureType)
            {
                case CaptureType.File:
                    return this.m_mediaCapture.StartRecordToStorageFileAsync(this.m_settings.MediaEncodingProfile, this.m_settings.OutputFile).AsTask();
                case CaptureType.Stream:
                case CaptureType.Sink:
                default:
                    return TaskEx.Idel;
            }
        }

        protected override Task StopHook()
        {
            return this.m_mediaCapture.StopRecordAsync().AsTask();
        }
    }
}
