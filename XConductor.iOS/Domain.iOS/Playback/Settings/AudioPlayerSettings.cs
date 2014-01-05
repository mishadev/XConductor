using MonoTouch.Foundation;

using System;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;

namespace XConductor.Domain.iOS.Playback.Settings
{
    public class AudioPlayerSettings : ValidatableObject, ISettings, IDisposable
    {
        [Requared("Url")]
        public NSUrl Url { get; set; }

        public void Dispose()
        {
            if (this.Url != null) this.Url.Dispose();
        }
    }
}
