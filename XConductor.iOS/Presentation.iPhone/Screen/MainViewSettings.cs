using System;
using System.Linq;
using System.Collections.Generic;

using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;

namespace XConductor.Presentation.IPhone.Screen
{
    public class MainViewSettings : ValidatableObject
    {
        [Requared("SampleFilePath")]
        public string SampleFilePath { get; set; }

        [Requared("CapturedFilePath")]
        public string CapturedFilePath { get; set; }
    }
}
