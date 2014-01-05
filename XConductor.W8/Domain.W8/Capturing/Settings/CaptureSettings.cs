using System;
using System.Linq;
using System.Collections.Generic;

using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;

using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;
using XConductor.Domain.Seedwork.Abstractions.Settings;

namespace XConductor.Domain.W8.Capturing.Settings
{
    internal class CaptureSettings : ValidatableObject, ISettings
    {
        public CaptureType CaptureType { get; set; }

        public MediaCaptureInitializationSettings InitializationSettings { get; set; }

        [Requared("OutputFile")]
        public StorageFile OutputFile { get; set; }

        [Requared("MediaEncodingProfile")]
        public MediaEncodingProfile MediaEncodingProfile { get; set; }
    }
}
