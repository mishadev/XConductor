using System;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Utils
{
    public interface ISubscribtion<T> : IDisposable
    {
        bool Disposed { get; }

        void OnNext(T value);

        void OnError(Exception exception);

        void OnCompleted();
    }
}
