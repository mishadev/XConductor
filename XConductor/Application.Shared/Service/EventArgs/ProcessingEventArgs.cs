using System;
using XConductor.Domain.Seedwork.Media.EventArgs;

namespace XConductor.Application.Shared.Service.EventArgs
{
    public class ProcessingEventArgs : MediaEventArgs
    {
        public object ChainKey { get; private set; }

        public ProcessingEventArgs(object key, object state)
            : base(state)
        {
            this.ChainKey = key;
        }
    }
}
