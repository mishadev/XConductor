using System;

namespace XConductor.Domain.Seedwork.Media
{
    public interface IAudioFormatDescription : IDisposable
    {
        ///<summary>
        ///     An identifier specifying the general audio data format
        ///</summary>
        AudioFormats FormatTag { get; }

        ///<summary>
        ///     The number of frames per second of the data in the stream, when the stream is played at normal speed. 
        ///     For compressed formats, this field indicates the number of frames per second of equivalent decompressed data.
        ///</summary>
        int SampleRate { get; }

        ///<summary>
        ///     The number of channels in each frame of audio data
        ///</summary>
        short ChannelsPerFrame { get; }
        
        ///<summary>
        ///     The number of bytes from the start of one frame to the start of the next frame in an audio buffer
        ///</summary>
        int BytesPerFrame { get; }
        
        ///<summary>
        ///     The number of frames in a packet of audio data. For uncompressed audio, the value is 1. 
        ///     For variable bit-rate formats, the value is a larger fixed number, such as 1024 for AAC. 
        ///     For formats with a variable number of frames per packet, such as Ogg Vorbis, set this field to 0
        ///</summary>
        short FramesPerPacket { get; }

        ///<summary>
        ///     The number of bytes in a packet of audio data. To indicate variable packet size, set this field to 0. 
        ///     For a format that uses variable packet size, specify the size of each packet using an special data structure.
        ///</summary>
        short BytesPerPacket { get; }

        ///<summary>
        ///     The number of bits for one audio sample.
        ///</summary>
        short BitsPerChannel { get; }
        
        ///<summary>
        ///     Pads the structure out to force an even 8-byte alignment. Must be set to 0
        ///</summary>
        int Reserved { get; }
    }
}