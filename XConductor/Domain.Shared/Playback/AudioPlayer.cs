using System;
using System.Threading.Tasks;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Domain.Seedwork.Playback;
using XConductor.Domain.Shared.Abstractions;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.Shared.Playback
{
    public abstract class AudioPlayer : DomainService, IAudioPlayer
    {
        public event EventHandler<MediaEventArgs> OnPaused;

        public AudioPlayer(ISettingsService settingsService)
            : base(settingsService)
        { }

        public async Task Pause()
        {
            if (this.Initialized && this.Runing)
            {
                this.Runing = false;

                await this.PauseHook();

                this.Paused();
            }
        }

        protected void Paused()
        {
            if (this.OnPaused != null)
            {
                this.OnPaused(this, MediaEventArgs.Empty);
            }
        }

        protected abstract Task PauseHook();
    }
}
