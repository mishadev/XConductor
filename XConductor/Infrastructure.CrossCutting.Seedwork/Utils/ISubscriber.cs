using System;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Utils
{
    public interface ISubscriber<T>
    {
        ISubscribtion<T> Add(ISubscribtion<T> subscribtion);

        void OnNext(T value);

        void OnError(Exception exception);

        void OnCompleted();
    }
}
