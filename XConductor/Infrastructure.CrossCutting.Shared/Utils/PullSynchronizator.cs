using System;
using System.Collections;

using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;
using XConductor.Infrastructure.CrossCutting.Shared.Utils.Abstractions;

namespace XConductor.Infrastructure.CrossCutting.Shared.Utils
{
    public class PullSynchronizator<TInput> : BaseSynchronizator<TInput>, IDataReadable<TInput>
        where TInput : ICollection
    {
        private IReadable<TInput> m_source;

        public PullSynchronizator(IReadable<TInput> source)
            : base()
        {
            this.m_source = source;
        }

        public int Read(TInput buffer, int need)
        {
            int readSource;
            int readPackage = 0;
            bool sourceEnd = false;
            bool cacheEnd = false;

            while (readPackage == 0 && !cacheEnd)
	        {
                cacheEnd = sourceEnd && readPackage == 0;
                readPackage = this.ReadCache(buffer as Array, need, sourceEnd);

                if (readPackage == 0)
                {
                    TInput data = this.m_source.Read(out readSource);
                    this.WriteCache(data as Array);
                    sourceEnd = readSource == 0;
                }
	        }

            return readPackage;
        }

        public override void Dispose()
        {
            this.m_source.Dispose();
        }
    }
}
