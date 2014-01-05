using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XConductor.Domain.W8.Media.Reader.Settings
{
    public class MediaFoundationReaderSettings
    {
        public MediaFoundationReaderSettings()
        {
            this.MajorType = MediaTypes.MFMediaType_Audio;
            this.SubType = AudioSubtypes.MFAudioFormat_PCM;
        }

        public Guid MajorType { get; set; }
        public Guid SubType { get; set; }
    }
}
