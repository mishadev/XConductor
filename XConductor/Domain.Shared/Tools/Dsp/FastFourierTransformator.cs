using System;
using XConductor.Domain.Seedwork.Media;

namespace XConductor.Domain.Shared.Tools.Dsp
{
    public class FastFourierTransformator
    {
		private float m_left;
        private int m_bytesPerFrame;
        private Func<byte[], int, long> m_convertor;
        private short m_chennels;
        private short m_bytePerChannel;

        public FastFourierTransformator(IAudioFormatDescription format, int outPutSize = 1024)
        {
            this.m_bytesPerFrame = format.BytesPerFrame;
            this.m_chennels = format.ChannelsPerFrame;
            this.m_bytePerChannel = (short)(format.BitsPerChannel / 8);

			this.m_convertor = FastFourierTransformator.CreateConvertor(format.BitsPerChannel);

			this.m_left = (float)(1 << 10);
        }

        public float[] Transform(byte[] source)
        {
            float[] buff = this.ReadData(source);

			return buff;
        }

        private float[] ReadData(byte[] source)
        {
            var buffer = new float[source.Length / this.m_bytesPerFrame];

            int outIndex = 0;
            for (int n = 0; n < source.Length; n += this.m_bytesPerFrame)
            {
				buffer[outIndex++] = FastFourierTransformator.Convert( // static to improve performance
					source, n, 
					this.m_bytePerChannel, 
					this.m_chennels, 
					this.m_convertor) / this.m_left;
            }

            return buffer;
        }

		private static long Convert(byte[] source, int offset, int bytePerChannel, int chennels, Func<byte[], int, long> convertor)
        {
            int chennel = 0;
            int chennelOffset = 0;
			long val = 0;

            long result = 0;
			while(chennel < chennels)
            {
				chennelOffset = chennel * bytePerChannel;
				val = convertor(source, offset + chennelOffset);

				result = Math.Abs(val) > Math.Abs(result) ? val : result;
                chennel++;
            }

            return result;
        }

		private static Func<byte[], int, long> CreateConvertor(int bits)
        {
            switch (bits)
            {
                case 16:
                    return (value, index) => BitConverter.ToInt16(value, index);
                case 32:
                    return (value, index) => BitConverter.ToInt32(value, index);
                case 64:
                    return (value, index) => BitConverter.ToInt64(value, index);
                default:
                    return null;
            }
        }
    }
}
