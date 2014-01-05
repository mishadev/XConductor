using System;
using System.Threading.Tasks;
using XConductor.Domain.Seedwork.Common;
using XConductor.Domain.Seedwork.Media.EventArgs;

namespace XConductor.Application.Shared.Service.Abstractions
{
    public interface IDelayService
    {
        event EventHandler<MediaEventArgs> OnDetermineDelayStart;
        event EventHandler<DataEventArgs<IDelayServiceResults>> OnDetermineDelayStop;

        IDelayServiceSettings Settings { get; }

        Task StartDetermineDelay(string sampleName, string captureName);
        Task StopDetermineDelay();

        IAudioService AudioService { get; }
        IProcessingService ProcessingService { get; }
    }
}
