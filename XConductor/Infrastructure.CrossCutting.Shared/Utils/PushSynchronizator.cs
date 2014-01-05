using System;
using System.Collections;

using System.Threading.Tasks;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;
using XConductor.Infrastructure.CrossCutting.Shared.Utils.Abstractions;

namespace XConductor.Infrastructure.CrossCutting.Shared.Utils
{
    public class PushSynchronizator<TInput> : BaseSynchronizator<TInput>, IDataObservable<TInput>
        where TInput : class, ICollection
    {
        private Action<TInput> m_next;
        private Action m_completed;
        private Array m_package;

        private IDataObservable<TInput> m_source;

        public PushSynchronizator(IDataObservable<TInput> source, int packageSize)
            : base()
        {
            this.m_source = source;
            this.m_package = Array.CreateInstance(this.ElementType, packageSize);
        }

        public async Task Subscribe(Action<TInput> onNext, Action<Exception> onError = null, Action onCompleted = null)
        {
            this.m_next += onNext;
            this.m_completed += onCompleted;

            await this.m_source.Subscribe(
                onNext: data => this.Synchronize(data),
                onError: onError,
                onCompleted: () => this.ReadCache(readToEnd: true));
        }

        private void Synchronize(TInput data, bool sourceEnd = false)
        {
            this.WriteCache(data as Array);
            this.ReadCache();
        }

        private void ReadCache(bool readToEnd = false)
        {
            int readPackage = this.m_package.Length;

            while (readPackage != 0)
            {
                readPackage = this.ReadCache(this.m_package, this.m_package.Length, readToEnd);

                if (readPackage != 0) this.OnNext(this.m_package as TInput);
            }
            if (readToEnd) OnCompleted();
        }

        private void OnNext(TInput data)
        {
            if (this.m_next != null)
            {
                this.m_next(data);
            }
        }

        private void OnCompleted()
        {
            if (this.m_completed != null)
            {
                this.m_completed();
            }
        }

        public override void Dispose()
        {
            this.m_source.Dispose();
        }
    }
}
