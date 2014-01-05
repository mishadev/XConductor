using MonoTouch.AVFoundation;
using MonoTouch.Foundation;

using System;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;

namespace XConductor.Domain.iOS.Caturing.Settings
{
    internal class CaptureSettings : ValidatableObject, ISettings, IDisposable
    {
        [Requared("AVAudioSettings")]
        public AudioSettings AudioSettings { get; set; }

        [Requared("Url")]
        public NSUrl Url { get; set; }

        public void Dispose()
        {
            if (this.Url != null) this.Url.Dispose();
        }
    }
}
