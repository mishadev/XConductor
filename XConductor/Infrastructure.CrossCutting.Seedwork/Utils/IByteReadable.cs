using System;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Utils
{
    public interface IDataReadable<TOutput>
    {
        int Read(TOutput buffer, int need);
    }
}
