using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Infrastructure.CrossCutting.Shared.Validator.DataAnnotations;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;
using XConductor.Domain.Shared.Tools.Generators;
using XConductor.Domain.Seedwork.Generating.Settings;

namespace XConductor.Domain.Shared.Generating.Settings
{
    internal class ToneGeneratorSettings : ValidatableObject, ISettings
    {
		public int[] FrequenciesChennels { get; set; }

		public bool UsePhaseShifting { get; set; }

		public int BassFrequency { get; set; }

        [GenericValidation(GenericValidationComparation.NotEquals, 0, "PeakTime")]
        public int PeakTime { get; set; }

        [GenericValidation(GenericValidationComparation.NotEquals, 0, "BufferSize")]
        public int BufferSize { get; set; }

        [GenericValidation(GenericValidationComparation.NotEquals, 0, "Amplitude")]
        public float Amplitude { get; set; }

        [Requared("Frequencies")]
		public WaveConfiguration[] Configurations { get; set; }

        [GenericValidation(GenericValidationComparation.NotEquals, 0, "PeakCount")]
        public int PeakCount { get; set; }

        [GenericValidation(GenericValidationComparation.NotEquals, 0, "PeakGap")]
        public int PeakGap { get; set; }

        [GenericValidation(GenericValidationComparation.NotEquals, 0, "PeakChannelGap")]
        public int PeakChannelGap { get; set; }

        [Requared("AudioFormat")]
        public IAudioFormatDescription AudioFormat { get; set; }
    }
}
