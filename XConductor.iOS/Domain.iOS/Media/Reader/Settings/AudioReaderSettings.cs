using MonoTouch.AudioToolbox;
using MonoTouch.CoreFoundation;

using System;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;

namespace XConductor.Domain.iOS.Media.Reader.Settings
{
    public class AudioReaderSettings : ValidatableObject, ISettings, IDisposable
    {
        public bool UseCache { get; set; }

        public AudioFileType AudioFileType { get; set; }

        public int BufferSize { get; set; }

        [Requared("Url")]
        public CFUrl Url { get; set; }

        public void Dispose()
        {
            this.Url.Dispose();
        }
    }
}
