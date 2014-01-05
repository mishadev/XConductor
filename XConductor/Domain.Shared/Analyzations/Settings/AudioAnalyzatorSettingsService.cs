using System;
using System.Threading.Tasks;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Shared.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Domain.Shared.Analyzations.Settings
{
    public class AudioAnalyzerSettingsService : ChainedDomainSettingsService<float[], float[]>
    {
        protected override IChainedDomainSettings<float[], float[]> DefaultSettings(IDataObservable<float[]> source, IAudioFormatDescription format, object settings)
        {
            return new AudioAnalyzerSettings
            {
                PackageSize = 4,
                OutputSize = 64,
                FormatDescription = format,
                MinMeaningfulValue = 1,
                MaxDeviation = 20,
                RangeDeviation = 100,
                Source = source
            };
        }
    }
}
