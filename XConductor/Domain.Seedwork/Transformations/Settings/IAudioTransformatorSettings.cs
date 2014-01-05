using XConductor.Domain.Seedwork.Abstractions.Settings;

namespace XConductor.Domain.Seedwork.Transformations.Settings
{
    public interface IAudioTransformatorSettings : IChainedDomainSettings<byte[], float[]>
    { }
}
