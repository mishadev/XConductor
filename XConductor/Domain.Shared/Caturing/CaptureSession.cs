using System;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Capturing;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Domain.Shared.Abstractions;

namespace XConductor.Domain.Shared.Caturing
{
    public abstract class CaptureSession : DomainService, ICaptureSession
    {
        public event EventHandler<MediaEventArgs> OnSoundLeveChanged;
        public event EventHandler<MediaEventArgs> OnInterrupt;

        public CaptureSession(ISettingsService settingsService)
            : base(settingsService)
        { }

        protected void SoundLeveChanged(object state)
        {
            if (this.OnSoundLeveChanged != null)
            {
                this.OnSoundLeveChanged(this, new MediaEventArgs(state));
            }
        }

        protected void Interrupted(object state)
        {
            if (this.OnInterrupt != null)
            {
                this.OnInterrupt(this, new MediaEventArgs(state));
            }
        }
    }
}
