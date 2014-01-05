using System;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Analyzations.Settings;
using XConductor.Domain.Seedwork.Common;

namespace XConductor.Domain.Seedwork.Analyzations
{
    public interface IAudioAnalyzer : IChainedDomainService<IAudioAnalyzerSettings, float[], float[]>
    { }
}
