using System.Collections.Generic;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Validator.DataAnnotations
{
    public interface IValidationResult
    {
        IEnumerable<string> MemberNames { get; }

        string ErrorMessage { get; }
    }
}
