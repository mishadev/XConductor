using System.Collections.Generic;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Validator.DataAnnotations
{
    public interface IValidatableObject
    {
        IEnumerable<IValidationResult> Validate();

        bool IsValid();
    }
}
