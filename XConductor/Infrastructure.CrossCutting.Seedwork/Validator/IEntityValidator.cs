using System;
using System.Collections.Generic;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Validator
{
    public interface IEntityValidator
    {
        bool IsValid<TEntity>(TEntity item)
            where TEntity : class;

        IEnumerable<String> GetInvalidMessages<TEntity>(TEntity item)
            where TEntity : class;
    }
}
