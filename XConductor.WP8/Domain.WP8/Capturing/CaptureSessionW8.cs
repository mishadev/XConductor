using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Windows.Phone.Media;
using Windows.Phone.Media.Capture;
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
        private AudioVideoCaptureDevice m_captureDevice;
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
            var task = this.MediaCaptureInitializeAsync();

            this.InitEventHandler();

            return task;
        }

        private void InitEventHandler()
        {
            this.m_captureDevice.RecordingFailed += (s, e) => this.Error(s, e.ToString());
        }

        private Task MediaCaptureInitializeAsync()
        {
            return new Task(() =>
            {
                this.m_captureDevice = AudioVideoCaptureDevice.OpenForAudioOnlyAsync().GetResults();
            });
        }

        protected override Task StartHook()
        {
            var stream = this.m_settings.OutputFile.OpenAsync(FileAccessMode.ReadWrite).GetResults();
            return this.m_captureDevice.StartRecordingToStreamAsync(stream).AsTask();
        }

        protected override Task StopHook()
        {
            return this.m_captureDevice.StopRecordingAsync().AsTask();
        }
    }
}
