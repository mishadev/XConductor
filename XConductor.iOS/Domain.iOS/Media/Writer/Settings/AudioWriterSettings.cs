using MonoTouch.AudioToolbox;
using MonoTouch.Foundation;

using System;

using XConductor.Domain.Seedwork.Media.Writer.Settings;
using XConductor.Domain.Shared.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;

namespace XConductor.Domain.iOS.Media.Writer.Settings
{
    public class AudioWriterSettings : ChainedDomainSettings<byte[], float[]>, IAudioWriterSettings, IDisposable
    {
        [Requared("Url")]
        public NSUrl Url { get; set; }

        public AudioFileType AudioFileType { get; set; }

        public void Dispose()
        {
            if (this.Url != null) this.Url.Dispose();
        }
    }
}
