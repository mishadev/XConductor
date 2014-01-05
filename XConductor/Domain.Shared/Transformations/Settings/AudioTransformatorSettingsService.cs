using System.Threading.Tasks;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Shared.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Domain.Shared.Transformations.Settings
{
    public class AudioTransformatorSettingsService : ChainedDomainSettingsService<byte[], float[]>
    {
        protected override IChainedDomainSettings<byte[], float[]> DefaultSettings(IDataObservable<byte[]> source, IAudioFormatDescription format, object settings)
        {
            return new AudioTransformatorSettings
            {
                PackageSize = format.BytesPerFrame * 512,
                OutputSize = 512,
                Source = source,
                FormatDescription = format,
            };
        }
    }
}
