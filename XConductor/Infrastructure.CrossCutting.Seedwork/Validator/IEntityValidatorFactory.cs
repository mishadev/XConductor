using System;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Validator
{
    public interface IEntityValidatorFactory
    {
        IEntityValidator Create();
    }
}
