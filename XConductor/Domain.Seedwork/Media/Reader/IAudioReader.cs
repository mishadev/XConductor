using XConductor.Domain.Seedwork.Abstractions;

namespace XConductor.Domain.Seedwork.Media.Reader
{
    public interface IAudioReader : IObservableDomainService<byte[]>
    { }
}
