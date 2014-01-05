using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Transformations.Settings;

namespace XConductor.Domain.Seedwork.Transformations
{
    public interface IAudioTransformator : IChainedDomainService<IAudioTransformatorSettings, byte[], float[]>
    { }
}
