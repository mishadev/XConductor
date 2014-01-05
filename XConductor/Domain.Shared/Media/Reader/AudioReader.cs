using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media.Reader;
using XConductor.Domain.Shared.Abstractions;

namespace XConductor.Domain.Shared.Media.Reader
{
    public abstract class AudioReader : ObservableDomainService<byte[]>, IAudioReader
    {
        public AudioReader(ISettingsService settingsService)
            : base(settingsService)
        { }
    }
}
