using System;
using System.Collections.Generic;
using System.Linq;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Seedwork.Generating.Settings;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Domain.Shared.Tools.Generators
{
    public class AudioGenerator
    {
        private List<PeakTiming> m_peaksTimings = new List<PeakTiming>();
        private List<int> m_frequencies = new List<int>();

		private Dictionary<int, int[]> m_frequenciesToPhaseShift = new Dictionary<int, int[]>();

        private float m_amplitude;
        private int m_sampleRate;
        private int m_peakCount;
        private int m_peakGap;
        private int m_channels;
        private int m_bitPerChannel;
        private int m_bytesPerSecond;
        private int m_peakTime;
        private int m_peakChannelGap;
		private float m_endingtime;
		private int m_bytePerChannel;
		private int m_bytePerFrame;
		private WaveConfiguration[] m_configurations;
		private bool m_usePhaseShifting;

		private int[] m_lastGeneratedIndexByChennels;

		public AudioGenerator(
			float amplitude, 
			WaveConfiguration[] configurations, 
			bool usePhaseShifting, 
			int peakCount, 
			int peakGap, 
			int peakChannelGap, 
			int peakTime, 
			IAudioFormatDescription audioFormat)
        {
			this.m_usePhaseShifting = usePhaseShifting;
			this.m_amplitude = Math.Max (0, Math.Min(100, amplitude) / 100) * short.MaxValue;
            this.m_sampleRate = audioFormat.SampleRate;
            this.m_peakCount = peakCount;
            this.m_peakGap = peakGap;
            this.m_peakChannelGap = peakChannelGap;
            this.m_channels = audioFormat.ChannelsPerFrame;
            this.m_bitPerChannel = audioFormat.BitsPerChannel;
            this.m_peakTime = peakTime;
			this.m_configurations = configurations;

			this.m_bytePerChannel = this.m_bitPerChannel / 8;
			this.m_bytePerFrame = this.m_bytePerChannel * this.m_channels;
			this.m_bytesPerSecond = this.m_bytePerFrame * this.m_sampleRate;

            var idx = 0;
			float timeOffset = 0;
            while (idx < this.m_peakCount)
            {
				WaveConfiguration configuration = configurations[Math.Min(configurations.Length - 1, idx)];
				int gapChennel = this.m_peakChannelGap;

				if (configuration.AllChannelsSimultaneously) 
					gapChennel = 0;

				this.m_frequencies.Add(configuration.Frequency);

				PeakTiming timing = new PeakTiming (timeOffset, this.m_channels, this.m_peakTime, this.m_peakGap, gapChennel);

				timeOffset = timing.End;

				this.m_peaksTimings.Add(timing);

				idx++;
            }

			this.m_lastGeneratedIndexByChennels = new int[this.m_channels];
			int chennel = 0;
			while (chennel < this.m_channels) { this.m_lastGeneratedIndexByChennels[chennel] = -1; chennel++; } //init by -1

			this.m_endingtime = this.m_peaksTimings.Max (t => t.End) + this.m_peakGap; //ms
        }

        private double CurrentTime(long bytesProcessed)
        {
            return bytesProcessed / (double)this.m_bytesPerSecond;
        }

        public int Generate(byte[] buffer, long offset)
        {
			float time = (float)this.CurrentTime(offset) * 1000; //ms

			if (time > this.m_endingtime) return 0;
           	
			WaveDescriprion description = new WaveDescriprion(this.m_channels, this.m_usePhaseShifting) { 
				Amplitude = this.m_amplitude,
				BytePerChannel = this.m_bytePerChannel,
				Offset = offset,
				BytePerFrame = this.m_bytePerFrame,
				SampleRate = this.m_sampleRate
			};

			int generated = 0;
			int channel = 0;
            while (channel < this.m_channels)
	        {
				var idx = this.m_peaksTimings.FindIndex(t => t.IsPeakTime(channel, time));

				int lastGeneratedIndex = this.m_lastGeneratedIndexByChennels [channel];
				if (idx >= 0) {
					
					bool isChennelSupported = this.IsChennelSupported(channel, idx);
					if (isChennelSupported) {

						description.IsFirstWave = lastGeneratedIndex != idx;
						description.Channel = channel;
						description.Frequency = this.m_frequencies [idx];
						description.PhaseShift [channel] = this.GetPhaseShifting (channel, idx, description);

						generated = AudioGenerator.GenerateWave (description, buffer);

						if (!description.IsFirstWave) {
							this.m_lastGeneratedIndexByChennels [channel] = idx;
						}
					}
				} else if (lastGeneratedIndex != -1) {
					description.IsFirstWave = false;
					description.Channel = channel;
					description.Frequency = this.m_frequencies [lastGeneratedIndex];
					description.IsLastWave = true;
					description.PhaseShift[channel] = this.GetPhaseShifting(channel, lastGeneratedIndex, description);

					generated = AudioGenerator.GenerateWave (description, buffer);

					if (!description.IsLastWave) {
						this.m_lastGeneratedIndexByChennels[channel] = -1;
					}
				}

				channel++;
	        }

			if (generated == 0) {
				return AudioGenerator.GenerateSilence(buffer);
			} else {
				return generated;
			}
        }

		private bool IsChennelSupported(int channel, int frequencyIdx)
		{
			OrdinalNumbers supportedChennels = this.m_configurations [frequencyIdx].Chennels;
			bool supportAll = supportedChennels == OrdinalNumbers.Zero;

			return supportAll || (((channel + 1) & (int)supportedChennels) != 0);
		}

		private int GetPhaseShifting(int channel, int idx, WaveDescriprion description)
		{
			int[] phaseshift;
			if (!this.m_frequenciesToPhaseShift.TryGetValue (idx, out phaseshift)) {
				phaseshift = new int[this.m_channels];
				phaseshift[channel] = description.GetPhaseShiftMax ();
				this.m_frequenciesToPhaseShift.Add (idx, phaseshift);
			}
			if (phaseshift[channel] == -1) {
				phaseshift[channel] = description.GetPhaseShiftMax ();
			}

			return phaseshift [channel];
		}

        private static int GenerateSilence(byte[] buffer, long offset = 0)
        {
			long length = buffer.Length - offset;
			Array.Copy(new byte[length], 0, buffer, offset, length);
            return buffer.Length;
        }

		private static int GenerateWave(WaveDescriprion descriprion, byte[] buffer)
        {
			float amplitude = descriprion.Amplitude;
			int bytePerChannel = descriprion.BytePerChannel;
			int bytePerFrame = descriprion.BytePerFrame;
			int channel = descriprion.Channel;

			double thetaIncrement = descriprion.GetThetaIncrement();
			double theta = descriprion.GetTheta();

			int bufferFrames = buffer.Length / bytePerFrame;
            int frame = 0;

			bool isFirstWave = descriprion.IsFirstWave;
			bool isLastWave = descriprion.IsLastWave;
			float amplitudestep = 0;

            while (frame < bufferFrames)
            {
				if(isFirstWave) {
					amplitudestep = amplitude / (bufferFrames - frame);
					amplitude = 0;

					isFirstWave = false;
					descriprion.IsFirstWave = isFirstWave;
				}

				amplitude += amplitudestep;
				Int16 phase = (Int16)(Math.Sin(theta) * amplitude);

				byte[] bytes = BitConverter.GetBytes(phase);

                Array.Copy(bytes, 0, buffer, (frame * bytePerFrame) + (bytePerChannel * channel), bytes.Length);

				theta += thetaIncrement;

                frame++;

				if (isLastWave) {
					amplitudestep = -(amplitude / (bufferFrames - frame));

					isLastWave = false;
					descriprion.IsLastWave = isLastWave;
				}
            }

            return buffer.Length;
        }

		class WaveDescriprion
		{
			private static readonly double pi2 = 2.0 * Math.PI;

			public bool IsFirstWave;
			public bool IsLastWave;

			public int Frequency;
			public float Amplitude;
			public int BytePerChannel;
			public int Channel;
			public long Offset;
			public int BytePerFrame;
			public int SampleRate;
			public int[] PhaseShift;
			public bool UsePhaseShifting;

			public WaveDescriprion(int channelsPerFrame, bool usePhaseShifting)
			{
				this.UsePhaseShifting = usePhaseShifting;
				this.PhaseShift = new int[channelsPerFrame];
				int chennel = 0;
				while (chennel < channelsPerFrame) { this.PhaseShift[chennel] = -1; chennel++; } //init by -1
			}

			public int GetPhaseShiftMax()
			{
				int shift = 0;

				if (this.UsePhaseShifting) {
					var theta = this.GetTheta();
					var thetaIncrement = this.GetThetaIncrement ();

					var pidev2 = pi2 / 4;
					while (!(Math.Abs(Math.Cos(theta)) < 0.1 && (theta < pidev2 || (theta > pidev2 && theta < pidev2 * 3)))) {
						theta = (theta + thetaIncrement) % pi2;

						shift++;
					}
				}

				return shift;
			}

			public long GetFrameOffset()
			{
				return this.Offset / this.BytePerFrame;
			}

			public double GetThetaIncrement()
			{
				return pi2 * this.Frequency / this.SampleRate;
			}

			public double GetTheta()
			{
				double thetaIncrement = this.GetThetaIncrement();
				long frameOffset = this.GetFrameOffset();

				return ((frameOffset + this.PhaseShift[this.Channel]) * thetaIncrement) % pi2;
			}
		}
    }
}
