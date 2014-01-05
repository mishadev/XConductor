using System;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Utils
{
    public interface IReadable<out TOutput> : IDisposable
    {
        TOutput Read(out int bytesRead);
    }
}
