using System;
using System.Threading.Tasks;

using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Media.EventArgs;

namespace XConductor.Domain.Seedwork.Playback
{
    public interface IAudioPlayer : IDomainService
    {
        event EventHandler<MediaEventArgs> OnPaused;

        Task Pause();
    }
}
