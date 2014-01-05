using System;

using XConductor.Domain.Seedwork.Media.Exceptions;

namespace XConductor.Domain.Seedwork.Media.EventArgs
{
    public class MediaRuntimeExceptionEventArgs : MediaEventArgs
    {
        public MediaRuntimeExceptionEventArgs(Object state)
            : base(state)
        { }

        public MediaRuntimeExceptionEventArgs(Object state, MediaRuntimeException exception)
            : base(state)
        {
            this.Exception = exception;
        }

        public MediaRuntimeException Exception { get; protected set; }
    }
}
