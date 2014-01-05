using System;
using System.Threading.Tasks;
using XConductor.Application.Shared.Service.EventArgs;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Media.EventArgs;

namespace XConductor.Application.Shared.Service.Abstractions
{
    public interface IProcessingService : IDisposable
    {
        event EventHandler<ProcessingEventArgs> OnProcessingStart;
        event EventHandler<ProcessingEventArgs> OnProcessingStop;
        event EventHandler<ProcessingEventArgs> OnProcessingDataAvailable;
        event EventHandler<ProcessingEventArgs> OnProcessingResultsAvailable;
        
        Task Start(object key, object settings);
        Task Stop(object key);

        void Create(object key, IObservableDomainService service);
        void Add(object key, IChainedDomainService chain);
        void AddRange(object key, params IChainedDomainService[] chains);
        void Clear(object key);
        void Clear();

        bool IsInitialized { get; }
    }
}
