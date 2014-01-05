using System;

namespace XConductor.Domain.Seedwork.Common
{
    public class DataEventArgs<TData> : EventArgs
    {
        public static readonly new DataEventArgs<TData> Empty = new DataEventArgs<TData>(default(TData));

        public DataEventArgs(TData data)
        {
            this.Data = data;
        }

        public TData Data { get; protected set; }
    }
}
