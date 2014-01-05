namespace XConductor.Domain.Seedwork.Media.EventArgs
{
    using System;

    public class MediaEventArgs : EventArgs
    {
        public static readonly new MediaEventArgs Empty = new MediaEventArgs(null);

        public MediaEventArgs(Object state)
        { 
            this.State = state;
        }

        public Object State { get; protected set; }
    }
}
