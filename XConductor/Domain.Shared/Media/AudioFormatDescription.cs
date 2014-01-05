using XConductor.Domain.Seedwork.Media;

namespace XConductor.Domain.Shared.Media
{
    public class AudioFormatDescription : IAudioFormatDescription
    {
        public AudioFormatDescription()
        {
            this.FramesPerPacket = 1;
            this.FormatTag = AudioFormats.LinearPCM;
        }

        public AudioFormats FormatTag
        {
            get; set;
        }

        public int SampleRate
        {
            get; set;
        }

        public short ChannelsPerFrame
        {
            get; set;
        }

        public int BytesPerFrame
        {
            get
            {
                return (this.BitsPerChannel * this.ChannelsPerFrame) / 8;
            }
        }

        public short BytesPerPacket
        {
            get
            {
                return (short)(this.FramesPerPacket * this.BytesPerFrame);
            }
        }

        public short FramesPerPacket
        {
            get;
            set;
        }

        public short BitsPerChannel
        {
            get; set;
        }

        public int Reserved
        {
            get; set;
        }

        public void Dispose()
        { }

        public object CustomProp
        {
            get;
            protected set;
        }
    }
}
