using MonoTouch.AVFoundation;
using MonoTouch.Foundation;

using System.Threading.Tasks;

using XConductor.Domain.iOS.Playback.Delegates;
using XConductor.Domain.iOS.Playback.Settings;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Shared.Playback;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.iOS.Playback
{
    public class AudioPlayeriOS : AudioPlayer
    {
        private AudioPlayerSettings m_settings;
        private AVAudioPlayer m_audioplayer;
		private AVAudioPlayerHandler m_delegates;
		private AVAudioSession m_session;

        public AudioPlayeriOS(AudioPlayerSettingsService settingsService)
            : base(settingsService)
        { }

        protected override async Task StartHook()
        {
			var doing = false;
			if (this.m_audioplayer.PrepareToPlay())
			{
				doing = this.m_audioplayer.Play();
			}

			if (!doing) {
				await this.Stop(); 
			}
        }

        protected override async Task StopHook()
        {
			this.m_audioplayer.Stop();
        }

		protected override async Task PauseHook()
        {
			this.m_audioplayer.Pause();
        }

		protected override async Task InitializeHook()
        {
			this.DisposeAVAudio();

			this.m_delegates = this.InitDelegate();

			NSError error;

			this.m_session = AVAudioSession.SharedInstance();
			this.m_session.SetCategory(AVAudioSession.CategoryPlayAndRecord, out error);
			this.m_session.SetActive(true, out error);

			this.m_audioplayer = AVAudioPlayer.FromUrl(this.m_settings.Url, out error);
			this.m_audioplayer.Delegate = this.m_delegates;
        }

        private AVAudioPlayerHandler InitDelegate()
        {
            var handler = new AVAudioPlayerHandler();

			handler.OnPlayFinish += async (player, flag) =>  await this.Stop();
            handler.OnInterruption += (player, flags) => this.Paused();
            handler.OnDecoderError += (player, error) => this.Error(error, error.ToString());

            return handler;
        }

        protected override void InitSettings(ISettings settings)
        {
            var set = settings as AudioPlayerSettings;
            this.m_settings = set != null && set.IsValid() ? set : null;
        }

        public override ISettings Settings
        {
            get { return this.m_settings; }
        }

        public override void Dispose()
        {
			this.DisposeAVAudio();

            base.Dispose();
        }

		private void DisposeAVAudio()
		{
			if (this.m_audioplayer != null)	this.m_audioplayer.Dispose();
			if (this.m_delegates != null) this.m_delegates.Dispose();

			this.m_audioplayer = null;
			this.m_delegates = null;
		}
    }
}
