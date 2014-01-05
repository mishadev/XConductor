using System;
using System.Collections.Generic;
using System.Linq;

using XConductor.Application.Shared.Service.Abstractions;

namespace XConductor.Application.Shared.Service
{
	public class SimpleDelayServiceResults : IDelayServiceResults
	{
		double[] m_delays;
		bool m_isValid;

		public SimpleDelayServiceResults(double[] delays, bool isValid)
		{
			this.m_delays = delays;
			this.m_isValid = isValid;
		}

		public double[] Delays
		{
			get { return this.m_delays; }
		}

		public bool IsValidResults
		{
			get
			{
				return this.m_isValid;
			}
		}
	}

	public class ComputeableDelayServiceResults : IDelayServiceResults
    {
        private double[] m_delays;

        private double[] m_sempleData;
        private double[] m_capturedData;
        private float m_maximunDifference;

		public ComputeableDelayServiceResults(double[] sempleData, double[] capturedData)
        {
            this.m_sempleData = sempleData;
            this.m_capturedData = capturedData;
            this.m_maximunDifference = 0.5f;
        }

        private double[] ComputeData()
        {
            var delays = new double[this.m_sempleData.Length];
            var idx = 0;
            while (this.m_sempleData.Length > idx)
            {
                var current = this.m_sempleData[idx];
                int? minIdx = null;

                var currIdx = 0;
				while (this.m_capturedData.Length > currIdx && this.m_capturedData.Length > idx)
                {
                    var currVal = this.m_capturedData[currIdx] - current;
                    var minVal = this.m_capturedData[minIdx ?? currIdx] - current;
					if (Math.Abs(currVal) <= this.m_maximunDifference && Math.Abs(currVal) <= Math.Abs(minVal))
                    {
						minIdx = currIdx;
                    }
                    currIdx++;
                }

                var delay = -1d;
                if (minIdx.HasValue)
                {
					delay = Math.Abs(this.m_capturedData[minIdx.Value] - current);
                }

                delays[idx++] = delay;
            }

            var max = delays.Any() ? delays.Max() : 0;
            return delays.Select(val => val > 0 ? max - val : -1).ToArray();
        }

		private double GetValueOfDefault(double[] array, int idx, double defaultValue)
		{
			return array.Length > idx ? array[idx] : defaultValue;
		}

        public double[] Delays
        {
            get { return this.m_delays ?? (this.m_delays = this.ComputeData()); }
        }

		public bool IsValidResults
		{
			get
			{
				return !Delays.Any (v => v < 0) && this.m_sempleData.Length == this.m_capturedData.Length;
			}
		}
    }
}
