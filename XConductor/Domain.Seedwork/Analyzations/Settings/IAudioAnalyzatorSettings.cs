using System;
using XConductor.Domain.Seedwork.Abstractions.Settings;

namespace XConductor.Domain.Seedwork.Analyzations.Settings
{
    public interface IAudioAnalyzerSettings : IChainedDomainSettings<float[], float[]>
    {
        int MaxDeviation { get; set; }

        int RangeDeviation { get; set; }

        int MinMeaningfulValue { get; set; }
    }
}
