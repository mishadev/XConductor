using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Storage.Streams;
using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Seedwork.Abstractions.Settings;

namespace XConductor.Domain.W8.Media.Reader.Settings
{
    public class AudioReaderSettings : ValidatableObject, ISettings
    {
        [GenericValidation(GenericValidationComparation.NotEquals, 0, "BufferSize")]
        public int BufferSize { get; set; }

        [Requared("MFReaderSettings")]
        public MediaFoundationReaderSettings MFReaderSettings { get; set; }

        [Requared("FileStream")]
        public IRandomAccessStream FileStream { get; set; }
    }
}
