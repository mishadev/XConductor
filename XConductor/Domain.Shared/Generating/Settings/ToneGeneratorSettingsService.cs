using System.Threading.Tasks;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Shared.Abstractions.Settings;
using XConductor.Domain.Shared.Media;
using XConductor.Domain.Shared.Tools.Generators;
using XConductor.Domain.Seedwork.Generating.Settings;

namespace XConductor.Domain.Shared.Generating.Settings
{
    public class ToneGeneratorSettingsService : SettingsService
    {
        protected override ISettings DefaultSettings(params object[] parameters)
        {
            return new ToneGeneratorSettings
            {
				BassFrequency = 50,
                PeakTime = 1000, //ms
                Amplitude = 50,
                BufferSize = 1024,
				Configurations = new[] { new WaveConfiguration { Frequency = 500 } },
                PeakCount = 1,
                PeakGap = 1000,
                PeakChannelGap = 1000 / 2,
                AudioFormat = new AudioFormatDescription
                {
                    SampleRate = 44100,
                    BitsPerChannel = 16,
                    ChannelsPerFrame = 2,

                    FramesPerPacket = 1,
                    FormatTag = AudioFormats.LinearPCM
                }
            };
        }
    }
}
