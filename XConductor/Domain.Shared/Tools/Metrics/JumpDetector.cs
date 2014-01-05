using System.Linq;
using System;

namespace XConductor.Domain.Shared.Tools.Metrics
{
    public class JumpDetector
    {
        int m_max = 0;
        float[] m_squeresum;
        int m_idx = 0;
        int m_length = 0;
        int m_minMeaningfulValue;

        public JumpDetector(int maxDeviation, int rangeLength, int minMeaningfulValue)
        {
            this.m_max = maxDeviation;
            this.m_length = rangeLength;
			this.m_squeresum = new float[this.m_length];
            this.m_minMeaningfulValue = minMeaningfulValue;
        }

        public bool IsJumpDetect(float[] source)
        {
            var kff = 0f;
			 
			var sourceVal = source.Select(e => Math.Pow(e, 2)).Average();
			this.m_squeresum[this.m_idx++ % this.m_length] = (float)sourceVal;

            if (sourceVal != 0)
            {
				int idx = 0;
				float sum = 0;
				int lenght = Math.Min (this.m_idx, this.m_length);
				while (idx < lenght) {
					sum += this.m_squeresum [idx++];
				}

				float avg = (sum / lenght);

				if (avg != 0)
                {
					kff = (float)sourceVal / avg;
                }
            }

            var result = false;
			if (result = (kff > this.m_max && sourceVal > this.m_minMeaningfulValue))
            {
				this.m_squeresum = new float[this.m_length];
				this.m_idx = 0;
            }

            return result;
        }
    }
}
