using XConductor.Infrastructure.CrossCutting.Seedwork.Validator;

namespace XConductor.Infrastructure.CrossCutting.Shared.Validator
{
    public class DataAnnotationsEntityValidatorFactory
        : IEntityValidatorFactory
    {
        public IEntityValidator Create()
        {
            return new DataAnnotationsEntityValidator();
        }
    }
}
