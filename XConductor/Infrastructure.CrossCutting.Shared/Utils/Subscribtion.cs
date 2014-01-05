using System;

using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Infrastructure.CrossCutting.Shared.Utils
{
    public class Subscribtion<T> : ISubscribtion<T>
    {
        private Action<T> m_onNext;
        private Action<Exception> m_onError;
        private Action m_onCompleted;
        internal bool m_disposed = false;

        public Subscribtion(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
        {
            this.m_onNext = onNext;
            this.m_onError = onError;
            this.m_onCompleted = onCompleted;
        }

        public void OnNext(T value)
        {
            if (this.m_onNext != null)
            {
                this.m_onNext(value);
            }
        }

        public void OnError(Exception exception)
        {
            if (this.m_onError != null)
            {
                this.m_onError(exception);
            }
        }

        public void OnCompleted()
        {
            if (this.m_onCompleted != null)
            {
                this.m_onCompleted();
            }
        }

        public void Dispose()
        {
            if (!this.m_disposed)
            {
                this.m_disposed = true;

                this.m_onNext = null;
                this.m_onError = null;
                this.m_onCompleted = null;
            }
        }

        public bool Disposed
        {
            get { return this.m_disposed; }
        }
    }
}
