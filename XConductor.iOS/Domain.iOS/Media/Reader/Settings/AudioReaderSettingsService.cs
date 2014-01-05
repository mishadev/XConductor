using MonoTouch.AudioToolbox;
using MonoTouch.CoreFoundation;

using System;
using System.IO;
using System.Threading.Tasks;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Shared.Abstractions.Settings;

namespace XConductor.Domain.iOS.Media.Reader.Settings
{
    public class AudioReaderSettingsService : SettingsService
    {
        protected override ISettings DefaultSettings(params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0 && parameters.GetValue(0) is string)
            {
                var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                dirPath = Path.Combine(dirPath, "XConductorAudio");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                var settings = new AudioReaderSettings
                {
                    UseCache = false,
                    Url = CFUrl.FromFile(Path.Combine(dirPath, (string)parameters.GetValue(0))),
                    BufferSize = 512,
                    AudioFileType = AudioFileType.AIFF
                };
                return settings;
            }
            return null;
        }
    }
}
