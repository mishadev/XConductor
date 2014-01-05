using XConductor.Infrastructure.CrossCutting.Shared.Utils;
using XConductor.Domain.Seedwork.Generating.Settings;

namespace XConductor.Application.Shared.Service.Abstractions
{
    public interface IDelayServiceSettings : IApplicationServiceSettings
    {
        int? PeakTime { get; set; }
		float? Amplitude { get; set; }
        int? BufferSize { get; set; }
		WaveConfiguration[] Configurations { get; set; }
        int? PeakCount { get; set; }
        int? PeakGap { get; set; }
        int? PeakChannelGap { get; set; }
        int? SampleRate { get; set; }
        int? BitsPerChannel { get; set; }
        int? ChannelsPerFrame { get; set; }
		bool? UsePhaseShifting { get; set; }
		bool? UseStepsAlgorithm { get; set; }
    }
}
