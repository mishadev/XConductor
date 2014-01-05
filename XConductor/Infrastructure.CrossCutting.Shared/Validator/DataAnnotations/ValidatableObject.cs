using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using XConductor.Infrastructure.CrossCutting.Seedwork.Validator.DataAnnotations;

namespace XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations
{
    public abstract class ValidatableObject : IValidatableObject
    {
        protected virtual void FillErroeList(List<ValidationResult> errors)
        {
            var type = this.GetType();
            var item = this;
#if MONOTOUCH
            var result = from property in TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>()
                         from attribute in property.Attributes.OfType<ValidationAttribute>()
                         where !attribute.IsValid(property.GetValue(item))
                         select attribute.GetValidationResult(property.GetValue(item));
#else
            var result = from property in type.GetRuntimeProperties()
                         from attribute in property.GetCustomAttributes().OfType<ValidationAttribute>()
                         where !attribute.IsValid(property.GetValue(item))
                         select attribute.GetValidationResult(property.GetValue(item));
#endif
            errors.AddRange(result);
        }

        public IEnumerable<IValidationResult> Validate()
        {
            var errors = new List<ValidationResult>();

            this.FillErroeList(errors);

            return errors.OfType<IValidationResult>();
        }

        public bool IsValid()
        {
            return !this.Validate().Any();
        }
    }
}
