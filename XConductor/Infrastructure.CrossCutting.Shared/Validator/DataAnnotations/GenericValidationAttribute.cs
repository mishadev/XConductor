using System;

namespace XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations
{
    public class GenericValidationAttribute : ValidationAttribute
    {
        private GenericValidationComparation m_comparation;
        private object m_value;

        public GenericValidationAttribute(GenericValidationComparation comparation, object value)
            : base()
        {
            this.m_value = value;
            this.m_comparation = comparation;
        }

        public GenericValidationAttribute(GenericValidationComparation comparation, object value, string errorMessage)
            : base(errorMessage)
        {
            this.m_value = value;
            this.m_comparation = comparation;
        }

        public override bool IsValid(object value)
        {
            switch (this.m_comparation)
            {
                case GenericValidationComparation.NotEquals:
                    return value != this.m_value;
                case GenericValidationComparation.Equals:
                    return value == this.m_value;
                default:
                    return true;
            }
        }

        public override ValidationResult GetValidationResult(object value)
        {
            if (!this.IsValid(value))
            { 
                return new ValidationResult(this.ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
