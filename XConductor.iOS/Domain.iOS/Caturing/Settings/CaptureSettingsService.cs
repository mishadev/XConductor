using MonoTouch.AudioToolbox;
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;

using System;
using System.IO;
using System.Threading.Tasks;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Shared.Abstractions.Settings;

namespace XConductor.Domain.iOS.Caturing.Settings
{
    public class CaptureSettingsService : SettingsService
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

                var format = AudioStreamBasicDescription.CreateLinearPCM();
                var settings = new CaptureSettings
                {
                    Url = new NSUrl(Path.Combine(dirPath, (string)parameters.GetValue(0)), isDir: false),
                    AudioSettings = new AudioSettings
                    {
                        Format = format.Format,
                        SampleRate = (float)format.SampleRate,
                        NumberChannels = format.ChannelsPerFrame,

                        LinearPcmBitDepth = format.BitsPerChannel,
                        LinearPcmBigEndian = (format.FormatFlags & AudioFormatFlags.LinearPCMIsBigEndian) != 0,
                        LinearPcmFloat = (format.FormatFlags & AudioFormatFlags.LinearPCMIsFloat) != 0
                    }
                };
                return settings;
            }
            return null;
        }
    }
}
