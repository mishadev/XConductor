using System;

using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Media.EventArgs;

namespace XConductor.Domain.Seedwork.Capturing
{
    public interface ICaptureSession : IDomainService
    {
        event EventHandler<MediaEventArgs> OnSoundLeveChanged;
        event EventHandler<MediaEventArgs> OnInterrupt;
    }
}
