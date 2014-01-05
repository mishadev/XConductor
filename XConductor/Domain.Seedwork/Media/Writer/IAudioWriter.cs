using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Media.Writer.Settings;
using XConductor.Domain.Seedwork.Transformations.Settings;

namespace XConductor.Domain.Seedwork.Media.Writer
{
    public interface IAudioWriter : IChainedDomainService<IAudioWriterSettings, byte[], byte[]>
    { }
}
