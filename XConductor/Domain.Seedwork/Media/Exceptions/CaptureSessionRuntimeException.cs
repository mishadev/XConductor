using System;

namespace XConductor.Domain.Seedwork.Media.Exceptions
{
    public class MediaRuntimeException : Exception
    {
        public MediaRuntimeException()
            : base()
        { }

        public MediaRuntimeException(string message)
            : base(message)
        { }

        public MediaRuntimeException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
