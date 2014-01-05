using System;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Adapter
{
    public interface  ITypeAdapterFactory
    {
        ITypeAdapter Create();
    }
}
