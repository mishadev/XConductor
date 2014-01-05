using System;
using System.Collections.Generic;

using XConductor.Domain.Seedwork.Transformations.Settings;
using XConductor.Domain.Shared.Abstractions.Settings;
using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;

namespace XConductor.Domain.Shared.Transformations.Settings
{
    public class AudioTransformatorSettings : ChainedDomainSettings<byte[], float[]>, IAudioTransformatorSettings
    {
        protected override void FillErroeList(List<ValidationResult> errors)
        {
            if (this.OutputSize < 0 || Math.Log(this.OutputSize, 2) % 1 != 0) //is power of two
            {
                errors.Add(new ValidationResult("FFTPackageSize is OutputSize"));
            }
            base.FillErroeList(errors);
        }
    }
}
