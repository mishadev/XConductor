using System;

namespace XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidationAttribute : Attribute
    {
        public string ErrorMessage
        {
            get;
            protected set;
        }

        protected ValidationAttribute()
        {
            this.ErrorMessage = string.Empty;
        }

        protected ValidationAttribute(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public string FormatErrorMessage(string format)
        {
            return !string.IsNullOrWhiteSpace(format) ? string.Format(format, this.ErrorMessage) : this.ErrorMessage;
        }

        public abstract bool IsValid(object value);
        public abstract ValidationResult GetValidationResult(object value);
    }
}
