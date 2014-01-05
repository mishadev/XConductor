using System;

using XConductor.Domain.Seedwork.Common;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Domain.Seedwork.Abstractions
{
    public interface IObservableDomainService : IDomainService
    {
        Type Output { get; }

        IAudioFormatDescription AudioFormat { get; }

		event EventHandler<MediaEventArgs> OnDataAvalable;
    }

    public interface IObservableDomainService<TOutput> : IObservableDomainService, IDataObservable<TOutput>
    {
		new event EventHandler<DataEventArgs<TOutput>> OnDataAvalable;
    }
}
