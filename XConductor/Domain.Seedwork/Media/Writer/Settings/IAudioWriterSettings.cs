using XConductor.Domain.Seedwork.Abstractions.Settings;

namespace XConductor.Domain.Seedwork.Media.Writer.Settings
{
    public interface IAudioWriterSettings : IChainedDomainSettings<byte[], byte[]>
    { }
}
