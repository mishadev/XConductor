using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Windows.Storage;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Playback;
using XConductor.Domain.Shared.Playback;
using XConductor.Domain.W8.Playback.Settings;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.W8.Playback
{
    public class AudioPlayerW8 : AudioPlayer
    {
        private RoutedEventHandler m_handler;
        private AudioPlayerSettings m_settings;

        public AudioPlayerW8(AudioPlayerSettingsService settingsService)
            : base(settingsService)
        { }

        protected override Task StartHook()
        {
            return new Task(() =>
                this.m_settings.MediaElement.Play()
            );
        }

        protected override Task StopHook()
        {
            return new Task(() =>
                this.m_settings.MediaElement.Stop()
            );
        }

        protected override Task PauseHook()
        {
            return new Task(() =>
                this.m_settings.MediaElement.Pause()
            );
        }

        protected override Task InitializeHook()
        {
            return new Task(() => {
                
                if(this.m_handler == null)
                {
                    this.m_handler = (s, e) => this.Stop().Await();
                    this.m_settings.MediaElement.MediaEnded += this.m_handler;
                } 

                var stream = this.m_settings.InputFile.OpenAsync(FileAccessMode.Read).GetResults();

                this.m_settings.MediaElement.SetSource(stream.AsStream());
            });
        }

        protected override void InitSettings(ISettings settings)
        {
            this.m_settings = settings as AudioPlayerSettings;
        }

        public override ISettings Settings
        {
            get { return this.m_settings; }
        }
    }
}
