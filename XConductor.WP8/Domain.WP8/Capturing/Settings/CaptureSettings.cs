using System;
using System.Linq;
using System.Collections.Generic;

using Windows.Storage;

using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;
using XConductor.Domain.Seedwork.Abstractions.Settings;

namespace XConductor.Domain.W8.Capturing.Settings
{
    internal class CaptureSettings : ValidatableObject, ISettings
    {
        [Requared("OutputFile")]
        public StorageFile OutputFile { get; set; }
    }
}
