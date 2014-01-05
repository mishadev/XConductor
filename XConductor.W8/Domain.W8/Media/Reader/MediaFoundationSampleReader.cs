using System;
using System.Runtime.InteropServices;

using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Domain.W8.Media.Reader
{
    public class MediaFoundationSampleReader : IReadable<byte[]>
    {
        long m_readed = 0;
        private IMFSourceReader m_reader;

        public MediaFoundationSampleReader(IMFSourceReader reader)
        {
            this.m_reader = reader;
        }

        public byte[] Read(out int bytesRead)
        {
            bytesRead = 0;

            IMFSample pSample;
            int dwFlags;
            ulong timestamp;
            int actualStreamIndex;
            this.m_reader.ReadSample(MediaFoundationInterop.MF_SOURCE_READER_FIRST_AUDIO_STREAM, 0, out actualStreamIndex, out dwFlags, out timestamp, out pSample);
            if (dwFlags != 0)
            {
                // reached the end of the stream or media type changed
                return new byte[0];
            }

            IMFMediaBuffer pBuffer;
            pSample.ConvertToContiguousBuffer(out pBuffer);
            IntPtr pAudioData = IntPtr.Zero;
            int cbBuffer;
            int pcbMaxLength;
            pBuffer.Lock(out pAudioData, out pcbMaxLength, out cbBuffer);

            byte[] readBuffer = new byte[cbBuffer];

            Marshal.Copy(pAudioData, readBuffer, 0, readBuffer.Length);
            bytesRead += readBuffer.Length;

            pBuffer.Unlock();
            Marshal.ReleaseComObject(pBuffer);
            Marshal.ReleaseComObject(pSample);

            m_readed += bytesRead;

            return readBuffer;
        }

        public virtual void Dispose()
        { }
    }
}
