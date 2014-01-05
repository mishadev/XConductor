using MonoTouch.AudioToolbox;
using MonoTouch.Foundation;

using System;
using System.IO;
using System.Threading.Tasks;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Shared.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Domain.iOS.Media.Writer.Settings
{
    public class AudioWriterSettingsService : ChainedDomainSettingsService<byte[], byte[]>
    {
        protected override IChainedDomainSettings<byte[], byte[]> DefaultSettings(IDataObservable<byte[]> source, IAudioFormatDescription format, object settings)
        {
            var chainSettings = settings as string;

            if (!string.IsNullOrWhiteSpace(chainSettings))
            {
                var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                dirPath = Path.Combine(dirPath, "XConductorAudio");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                var outputSize = 512;

                var serviceSettings = new AudioWriterSettings
                {
                    Url = new NSUrl(Path.Combine(dirPath, chainSettings), isDir: false),
                    PackageSize = outputSize,
                    OutputSize = outputSize,
                    Source = source,
                    FormatDescription = format,
                    AudioFileType = AudioFileType.AIFF
                };

                return serviceSettings;
            }

            return null;
        }
    }
}
