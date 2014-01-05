using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;
using XConductor.Domain.Shared.Abstractions.Settings;

namespace XConductor.Domain.W8.Playback.Settings
{
    public class AudioPlayerSettingsService : SettingsService
    {
        private MediaElement m_mediaElement;

        public AudioPlayerSettingsService(MediaElement mediaElement)
        {
            this.m_mediaElement = mediaElement;
        }

        protected override ISettings DefaultSettings(params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0 && parameters.GetValue(0) is string)
            {
                StorageFolder store = KnownFolders.MusicLibrary;
                IReadOnlyList<StorageFile> fiels = store.GetFilesAsync().GetResults();

                var settings = new AudioPlayerSettings
                {
                    InputFile = fiels.FirstOrDefault(f => f.Name.Equals((string)parameters.GetValue(0), StringComparison.OrdinalIgnoreCase)),
                    MediaElement = this.m_mediaElement,
                };

                return settings;
            }
            return null;
        }
    }
}
