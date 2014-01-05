using System.Collections.Generic;
using XConductor.Application.Shared.Service.Abstractions;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;
using XConductor.Domain.Seedwork.Generating.Settings;

namespace XConductor.Application.Shared.Service
{
    internal class DelayServiceSettings : ApplicationServiceSettings, IDelayServiceSettings
    {
        public int? PeakTime { get; set; }
		public float? Amplitude { get; set; }
        public int? BufferSize { get; set; }
		public WaveConfiguration[] Configurations { get; set; }
        public int? PeakCount { get; set; }
        public int? PeakGap { get; set; }
        public int? PeakChannelGap { get; set; }
        public int? SampleRate { get; set; }
        public int? BitsPerChannel { get; set; }
        public int? ChannelsPerFrame { get; set; }
		public bool? UsePhaseShifting { get; set; }
		public bool? UseStepsAlgorithm { get; set; }
    }
}
