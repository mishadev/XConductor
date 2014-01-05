using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media.Writer;
using XConductor.Domain.Seedwork.Media.Writer.Settings;
using XConductor.Domain.Shared.Abstractions;

namespace XConductor.Domain.Shared.Media.Writer
{
    public abstract class AudioWriter : ChainedDomainService<IAudioWriterSettings, byte[], byte[]>, IAudioWriter
    {
        public AudioWriter(ISettingsService settingsService)
            : base(settingsService)
        { }
    }
}
