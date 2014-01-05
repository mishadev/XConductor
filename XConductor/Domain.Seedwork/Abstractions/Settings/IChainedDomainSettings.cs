using XConductor.Domain.Seedwork.Media;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Domain.Seedwork.Abstractions.Settings
{
    public interface IChainedDomainSettings<TInput, TOutput> : ISettings
    {
        IAudioFormatDescription FormatDescription { get; set; }

        IDataObservable<TInput> Source { get; set; }

        int OutputSize { get; set; }

        int PackageSize { get; set; }
    }
}
