using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using XConductor.Infrastructure.CrossCutting.Seedwork.Validator;
using XConductor.Infrastructure.CrossCutting.Seedwork.Validator.DataAnnotations;
using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;

namespace XConductor.Infrastructure.CrossCutting.Shared.Validator
{
    public class DataAnnotationsEntityValidator
        : IEntityValidator
    {
        void FillValidatableObjectErrors<TEntity>(TEntity item, List<string> errors)
            where TEntity : class
        {
            if (item is IValidatableObject)
            {
                var validationResults = ((IValidatableObject)item).Validate();

                errors.AddRange(validationResults.Select(vr => vr.ErrorMessage));
            }
        }

        void FillValidationAttributeErrors<TEntity>(TEntity item, List<string> errors)
            where TEntity : class
        {
#if MONOTOUCH
            var result = from property in TypeDescriptor.GetProperties(typeof(TEntity)).Cast<PropertyDescriptor>()
                         from attribute in property.Attributes.OfType<ValidationAttribute>()
                         where !attribute.IsValid(property.GetValue(item))
                         select attribute.FormatErrorMessage(string.Empty);
#else
            var result = from property in typeof(TEntity).GetRuntimeProperties()
                         from attribute in property.GetCustomAttributes().OfType<ValidationAttribute>()
                         where !attribute.IsValid(property.GetValue(item))
                         select attribute.FormatErrorMessage(string.Empty);
#endif

            if (result.Any())
            {
                errors.AddRange(result);
            }
        }

        public bool IsValid<TEntity>(TEntity item)
            where TEntity : class
        {
            var errors = this.GetInvalidMessages<TEntity>(item);

            return errors != null && !errors.Any(); //valid only if errors empty
        }

        public IEnumerable<string> GetInvalidMessages<TEntity>(TEntity item)
            where TEntity : class
        {
            if (item == null)
                return null;

            var validationErrors = new List<string>();

            this.FillValidatableObjectErrors(item, validationErrors);
            this.FillValidationAttributeErrors(item, validationErrors);

            return validationErrors;
        }
    }
}
