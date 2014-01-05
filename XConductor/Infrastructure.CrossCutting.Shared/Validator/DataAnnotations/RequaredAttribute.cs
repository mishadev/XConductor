using System;

namespace XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations
{
    public class RequaredAttribute : GenericValidationAttribute
    {
        public RequaredAttribute()
            : base(GenericValidationComparation.NotEquals, null)
        { }

        public RequaredAttribute(string errorMessage)
            : base(GenericValidationComparation.NotEquals, null, errorMessage)
        { }
    }
}
