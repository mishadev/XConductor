using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;
using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;

namespace XConductor.Domain.Shared.Abstractions.Settings
{
    public abstract class ChainedDomainSettings<TInput, TOutput> : ValidatableObject, IChainedDomainSettings<TInput, TOutput>
    {
        [Requared("FormatDescription")]
        public IAudioFormatDescription FormatDescription { get; set; }

        [Requared("Source")]
        public IDataObservable<TInput> Source { get; set; }

        public int OutputSize { get; set; }

        public int PackageSize { get; set; }
    }
}
