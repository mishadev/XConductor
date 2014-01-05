using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

using Windows.Storage;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;


namespace XConductor.Domain.W8.Playback.Settings
{
    public class AudioPlayerSettingsService : ISettingsService
    {
        private MediaElement m_mediaElement;

        public AudioPlayerSettingsService(MediaElement mediaElement)
        {
            this.m_mediaElement = mediaElement;
        }

        public Task<ISettings> GetSettings(object parameters)
        {
            return new Task<ISettings>(() =>
            {
                if (parameters is string)
                {
                    StorageFolder store = KnownFolders.MusicLibrary;
                    IReadOnlyList<StorageFile> fiels = store.GetFilesAsync().AsTask().Result;

                    var settings = new AudioPlayerSettings
                    {
                        InputFile = fiels.FirstOrDefault(f => f.Name.Equals((string)parameters, StringComparison.OrdinalIgnoreCase)),
                        MediaElement = this.m_mediaElement,
                    };

                    return settings;
                }
                return null;
            });
        }
    }
}
