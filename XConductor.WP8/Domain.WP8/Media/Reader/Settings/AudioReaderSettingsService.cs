using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;

namespace XConductor.Domain.W8.Media.Reader.Settings
{
    public class AudioReaderSettingsService : ISettingsService
    {
        public Task<ISettings> GetSettings(object parameters)
        {
            return new Task<ISettings>(() =>
            {
                if (parameters is string)
                {
                    StorageFolder store = KnownFolders.MusicLibrary;
                    IRandomAccessStream straem = store.GetFileAsync((string)parameters).AsTask().Result.OpenAsync(FileAccessMode.Read).AsTask().Result;

                    var settings = new AudioReaderSettings
                    {
                        BufferSize = 64,
                        MFReaderSettings = new MediaFoundationReaderSettings(),
                        FileStream = straem,
                    };

                    return settings;
                }
                return null;
            });
        }
    }
}
