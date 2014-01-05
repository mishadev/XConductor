using System;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Validator
{
    public static class EntityValidatorFactory
    {
        static IEntityValidatorFactory _factory = null;

        public static void SetCurrent(IEntityValidatorFactory factory)
        {
            _factory = factory;
        }

        public static IEntityValidator CreateValidator()
        {
            return (_factory != null) ? _factory.Create() : null;
        }
    }
}
