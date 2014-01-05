using System;
using System.Collections;

namespace XConductor.Infrastructure.CrossCutting.Shared.Utils.Abstractions
{
    public abstract class BaseSynchronizator<TInput> : IDisposable
        where TInput : ICollection
    {
        private Array m_buffer;
        private int m_unuseful = 0;
        private int m_useful = 0;

        protected Type ElementType;

        public BaseSynchronizator(int bufferSize = 0)
        {
            var type = typeof(TInput);
            if (type.IsArray)
            {
                this.ElementType = type.GetElementType();
                this.m_buffer = Array.CreateInstance(this.ElementType, bufferSize);
            }
        }

        protected void EnsureBuffer(int length)
        {
            Array temp = null;

            if (this.m_buffer.Length < length + this.m_useful)
            {
                temp = Array.CreateInstance(this.ElementType, length + this.m_useful);
                Array.Copy(this.m_buffer, temp, this.m_buffer.Length);
                this.m_buffer = temp;
            }
        }

        protected void WriteCache(Array data)
        {
            if (data == null || data.Length == 0) return;

            this.EnsureBuffer(data.Length);

            if (this.m_unuseful > 0)
            {
                Array.Copy(this.m_buffer, this.m_unuseful, this.m_buffer, 0, this.m_useful);
                this.m_unuseful = 0;
            }

            Array.Copy(data, 0, this.m_buffer, this.m_useful, data.Length);

            this.m_useful += data.Length;
        }

        protected int ReadCache(Array package, int need, bool forceRead)
        {
            var read = 0;

            int needPackage = Math.Min(package.Length, need);
            if (this.m_useful >= needPackage || forceRead)
            {
                read = Math.Min(needPackage, this.m_useful);
                if (read != 0)
                {
                    Array.Copy(this.m_buffer, this.m_unuseful, package, 0, read);

                    this.m_useful = Math.Max(this.m_useful - package.Length, 0);
                    this.m_unuseful = Math.Min(this.m_unuseful + package.Length, this.m_buffer.Length);
                }
            }

            return read;
        }

        public virtual void Dispose()
        { }
    }
}
