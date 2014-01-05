using MonoTouch.Foundation;

using System;
using System.IO;
using System.Threading.Tasks;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Shared.Abstractions.Settings;

namespace XConductor.Domain.iOS.Playback.Settings
{
    public class AudioPlayerSettingsService : SettingsService
    {
        protected override ISettings DefaultSettings(params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0 && parameters.GetValue(0) is string)
            {
                var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                dirPath = Path.Combine(dirPath, "XConductorAudio");
                NSUrl url = Directory.Exists(dirPath) ? new NSUrl(Path.Combine(dirPath, (string)parameters.GetValue(0)), isDir: false) : null;

                var settings = new AudioPlayerSettings
                {
                    Url = url,
                };

                return settings;
            }
            return null;
        }
    }
}
