using System;
using System.Threading.Tasks;

using Windows.Storage;

using XConductor.Domain.Seedwork.Abstractions.Settings;

namespace XConductor.Domain.W8.Capturing.Settings
{
    public class CaptureSettingsService : ISettingsService
    {
        public Task<ISettings> GetSettings(object parameters)
        {
            return new Task<ISettings>(() =>
            {
                if (parameters is string)
                {
                    StorageFolder store = KnownFolders.MusicLibrary;
                    StorageFile file = store.CreateFileAsync((string)parameters, CreationCollisionOption.ReplaceExisting).AsTask().Result;

                    var settings = new CaptureSettings
                    {
                        OutputFile = file,
                    };

                    return settings;
                }
                return null;
            });
        }
    }
}
