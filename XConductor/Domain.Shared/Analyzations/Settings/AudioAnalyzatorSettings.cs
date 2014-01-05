using System;
using XConductor.Domain.Seedwork.Analyzations.Settings;
using XConductor.Domain.Shared.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;

namespace XConductor.Domain.Shared.Analyzations.Settings
{
    public class AudioAnalyzerSettings : ChainedDomainSettings<float[], float[]>, IAudioAnalyzerSettings
    {
        [GenericValidation(GenericValidationComparation.NotEquals, 0, "MaxDeviation")]
        public int MaxDeviation { get; set; }

        [GenericValidation(GenericValidationComparation.NotEquals, 0, "RangeDeviation")]
        public int RangeDeviation { get; set; }

        [GenericValidation(GenericValidationComparation.NotEquals, 0, "MinMeaningfulValue")]
        public int MinMeaningfulValue { get; set; }
    }
}
