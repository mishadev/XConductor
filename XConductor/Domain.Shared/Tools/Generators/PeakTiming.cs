using System;
using System.Linq;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.Shared.Tools.Generators
{
    internal class PeakTiming
    {
        private float[] m_peacksBegins;
        private float[] m_peacksEnds;

        private float m_timeOffset;

		public float Begin
		{
			get
			{
				return this.m_timeOffset + this.m_peacksBegins.FirstOrDefault();
			}
		}

		public float End
		{
			get
			{
				return this.m_timeOffset + this.m_peacksEnds.LastOrDefault();
			}
		}

        public PeakTiming(float offset, int chennels, int peakTime, int gapTime, int gapChennelTime)
        {
            this.m_peacksBegins = new float[chennels];
            this.m_peacksEnds = new float[chennels];
            
            int delay = gapChennelTime;
            int chennel = 0;
            while (chennel < chennels)
            {
				var begin = gapTime + (chennel * delay);
                var end = begin + peakTime;

                this.m_peacksBegins[chennel] = begin;
                this.m_peacksEnds[chennel] = end;

                chennel++;
            }

            this.m_timeOffset = offset;
        }

		public float FromBeginTime(int channel, float time)
		{
			return time - (this.m_timeOffset + this.m_peacksBegins [channel]);
		}

		public float ToEndTime(int channel, float time)
		{
			return (this.m_timeOffset + this.m_peacksEnds[channel]) - time;
		}

        public bool IsPeakTime(int channel, float time)
        {
            if (channel >= this.m_peacksBegins.Length) throw new InvalidOperationException();

			float from = this.FromBeginTime(channel, time);
			float to = this.ToEndTime(channel, time);

			return from >= 0 && to >= 0;
        }
    }
}
