using System.Collections.Generic;
using System.Linq;
using XConductor.Infrastructure.CrossCutting.Seedwork.Validator.DataAnnotations;

namespace XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations
{
    public class ValidationResult : IValidationResult
    {
        public static readonly ValidationResult Success = new ValidationResult(string.Empty);

        public IEnumerable<string> MemberNames
        {
            get;
            protected set;
        }

        public string ErrorMessage
        {
            get;
            protected set;
        }

        public ValidationResult(string errorMessage)
            : this(errorMessage, Enumerable.Empty<string>())
        { }

        public ValidationResult(string errorMessage, IEnumerable<string> memberNames)
        {
            this.ErrorMessage = errorMessage;
            this.MemberNames = memberNames;
        }

        public override string ToString()
        {
            return this.ErrorMessage;
        }
    }
}
