using System;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media.EventArgs;

namespace XConductor.Domain.Seedwork.Abstractions
{
    public interface IChainedDomainService : IObservableDomainService
    {
        event EventHandler<MediaEventArgs> OnResultsAvalable;

        Type Input { get; }
    }

    public interface IChainedDomainService<TSettings, TInput, TOutput> : IObservableDomainService<TOutput>, IChainedDomainService
        where TSettings : IChainedDomainSettings<TInput, TOutput>
    { }
}
