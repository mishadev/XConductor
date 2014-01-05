using System;
using System.Threading.Tasks;

using Windows.Media.MediaProperties;
using Windows.Storage;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Shared.Abstractions.Settings;

namespace XConductor.Domain.W8.Capturing.Settings
{
    public class CaptureSettingsService : SettingsService
    {
        protected override ISettings DefaultSettings(params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0 && parameters.GetValue(0) is string)
            {
                StorageFolder store = KnownFolders.MusicLibrary;
                StorageFile file = store.CreateFileAsync((string)parameters.GetValue(0), CreationCollisionOption.ReplaceExisting).AsTask().Result;

                var settings = new CaptureSettings
                {
                    CaptureType = CaptureType.File,
                    OutputFile = file,
                    MediaEncodingProfile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.Auto)
                };

                return settings;
            }
            return null;
        }
    }
}
