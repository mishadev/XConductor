using MonoTouch.AVFoundation;
using MonoTouch.Foundation;

using System.Threading.Tasks;

using XConductor.Domain.iOS.Caturing.Delegates;
using XConductor.Domain.iOS.Caturing.Settings;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Shared.Caturing;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.iOS.Caturing
{
    public class CaptureSessioniOS : CaptureSession
    {
        private CaptureSettings m_settings;

        private AVAudioRecorder m_audioRecorder;
        private AVAudioRecorderHandler m_delegate;
		private AVAudioSession m_session;

        public override ISettings Settings
        {
            get { return this.m_settings; }
        }

        public CaptureSessioniOS(CaptureSettingsService settingsService)
            : base(settingsService)
        { }

        protected override void InitSettings(ISettings settings)
        {
            var set = settings as CaptureSettings;
            this.m_settings = set != null && set.IsValid() ? set : null;
        }

        protected override async Task InitializeHook()
        {
			this.m_delegate = this.InitDelegate();

			NSError error;

			this.m_session = AVAudioSession.SharedInstance();
			this.m_session.SetCategory(AVAudioSession.CategoryPlayAndRecord, out error);
			this.m_session.SetActive(true, out error);

			this.m_audioRecorder = AVAudioRecorder.Create(this.m_settings.Url, this.m_settings.AudioSettings, out error);
			this.m_audioRecorder.Delegate = this.m_delegate;
        }

        private AVAudioRecorderHandler InitDelegate()
        {
            var handler = new AVAudioRecorderHandler();

			handler.OnRecordFinish += (recorder, flag) => this.BaseStoped ();
            handler.OnInterruption += (recorder, flags) => this.Interrupted(flags);
            handler.OnEncoderError += (recorder, error) => this.Error(error, error.ToString());

            return handler;
        }

		private void BaseStoped()
		{
			base.Stoped();
		}

		protected override void Stoped() { }

        protected override async Task StartHook()
        {
			var doing = false;
			if (this.m_audioRecorder.PrepareToRecord())
			{
				doing = this.m_audioRecorder.Record();
			}

			if (!doing) {
				await this.Stop(); 
			}
        }

        protected override async Task StopHook()
        {
			this.m_audioRecorder.Stop();
        }

        public override void Dispose()
        {
			if (this.m_audioRecorder != null) this.m_audioRecorder.Dispose();
			if (this.m_delegate != null) this.m_delegate.Dispose();

			this.m_audioRecorder = null;
			this.m_delegate = null;

            base.Dispose();
        }
    }
}
