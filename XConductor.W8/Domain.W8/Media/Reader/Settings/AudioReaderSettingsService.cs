using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Shared.Abstractions.Settings;

namespace XConductor.Domain.W8.Media.Reader.Settings
{
    public class AudioReaderSettingsService : SettingsService
    {
        protected override ISettings DefaultSettings(params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0 && parameters.GetValue(0) is string)
            {
                StorageFolder store = KnownFolders.MusicLibrary;
                IRandomAccessStream straem = store.GetFileAsync((string)parameters.GetValue(0)).AsTask().Result.OpenAsync(FileAccessMode.Read).AsTask().Result;

                var settings = new AudioReaderSettings
                {
                    BufferSize = 64,
                    MFReaderSettings = new MediaFoundationReaderSettings(),
                    FileStream = straem,
                };

                return settings;
            }
            return null;
        }
    }
}
