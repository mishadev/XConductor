using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Media.EventArgs;

namespace XConductor.Application.Shared.Service.Abstractions
{
    public interface IAudioService : IDisposable
    {
        event EventHandler<MediaEventArgs> OnRecordingStart;
        event EventHandler<MediaEventArgs> OnRecordingStop;

        Task StartRecording(string sourcePath);
        Task StopRecording();

        event EventHandler<MediaEventArgs> OnPlayingStart;
        event EventHandler<MediaEventArgs> OnPlayingStop;

        Task StartPlaying(string sourcePath);
        Task PausePlaying();
        Task StopPlaying();

        bool IsInitialized { get; }
    }
}
