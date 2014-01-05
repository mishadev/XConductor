using System;
using System.Threading.Tasks;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Utils
{
    public interface IDataObservable<T> : IDisposable
    {
        Task Subscribe(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null);
    }
}
