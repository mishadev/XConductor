using MonoTouch.AudioToolbox;

using System;
using System.Collections.Generic;
using System.Linq;

using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Shared.Media;

namespace XConductor.Domain.iOS.Media
{
    public static class AudioFormatDescriptionTools
    {
        private static Dictionary<AudioFormatType, AudioFormats> m_platformToStandart;
        private static Dictionary<AudioFormats, AudioFormatType> m_standartToPlatform;

        static AudioFormatDescriptionTools()
        {
            m_platformToStandart = new Dictionary<AudioFormatType, AudioFormats> {
				{ AudioFormatType.AC3, AudioFormats.AC3 },
				{ AudioFormatType.AES3, AudioFormats.AES3 },
				{ AudioFormatType.ALaw, AudioFormats.ALaw },
				{ AudioFormatType.AMR, AudioFormats.AMR },
				{ AudioFormatType.AppleIMA4, AudioFormats.AppleIMA4 },
				{ AudioFormatType.AppleLossless , AudioFormats.AppleLossless },
				{ AudioFormatType.Audible , AudioFormats.Audible },
				//{ AudioFormatType.CAC3 , AudioFormats.CAC3 },
				{ AudioFormatType.DVIIntelIMA , AudioFormats.DVIIntelIMA },
				{ AudioFormatType.iLBC , AudioFormats.iLBC },
				{ AudioFormatType.LinearPCM , AudioFormats.LinearPCM },
				{ AudioFormatType.MACE3 , AudioFormats.MACE3 },
				{ AudioFormatType.MACE6 , AudioFormats.MACE6 },
				{ AudioFormatType.MicrosoftGSM , AudioFormats.MicrosoftGSM },
				{ AudioFormatType.MIDIStream , AudioFormats.MIDIStream },
				{ AudioFormatType.MPEG4AAC , AudioFormats.MPEG4AAC },
				{ AudioFormatType.MPEG4AAC_ELD , AudioFormats.MPEG4AAC_ELD },
				{ AudioFormatType.MPEG4AAC_ELD_SBR , AudioFormats.MPEG4AAC_ELD_SBR },
				//{ AudioFormatType.MPEG4AAC_ELD_V2 , AudioFormats.MPEG4AAC_ELD_V2 },
				{ AudioFormatType.MPEG4AAC_HE , AudioFormats.MPEG4AAC_HE },
				{ AudioFormatType.MPEG4AAC_HE_V2 , AudioFormats.MPEG4AAC_HE_V2 },
				{ AudioFormatType.MPEG4AAC_LD , AudioFormats.MPEG4AAC_LD },
				{ AudioFormatType.MPEG4AAC_Spatial , AudioFormats.MPEG4AAC_Spatial },
				{ AudioFormatType.MPEG4CELP , AudioFormats.MPEG4CELP },
				{ AudioFormatType.MPEG4HVXC , AudioFormats.MPEG4HVXC },
				{ AudioFormatType.MPEG4TwinVQ , AudioFormats.MPEG4TwinVQ },
				{ AudioFormatType.MPEGLayer1 , AudioFormats.MPEGLayer1 },
				{ AudioFormatType.MPEGLayer2 , AudioFormats.MPEGLayer2 },
				{ AudioFormatType.MPEGLayer3 , AudioFormats.MPEGLayer3 },
				{ AudioFormatType.ParameterValueStream , AudioFormats.ParameterValueStream },
				{ AudioFormatType.QDesign , AudioFormats.QDesign },
				{ AudioFormatType.QDesign2 , AudioFormats.QDesign2 },
				{ AudioFormatType.QUALCOMM , AudioFormats.QUALCOMM },
				{ AudioFormatType.TimeCode , AudioFormats.TimeCode },
				{ AudioFormatType.ULaw , AudioFormats.ULaw },
			};

            m_standartToPlatform = new Dictionary<AudioFormats, AudioFormatType>(m_platformToStandart.Count);
            foreach (var item in m_platformToStandart)
	        {
                m_standartToPlatform.Add(item.Value, item.Key);
	        }
            
        }

        public static IAudioFormatDescription ConvertToStandsrtFormatDescription(AudioStreamBasicDescription description)
        {
            return new AudioFormatDescription()
            {
                FormatTag = ConvertToStandsrtFormat(description.Format),
                SampleRate = (int)description.SampleRate,
                ChannelsPerFrame = (short)description.ChannelsPerFrame,
                FramesPerPacket = (short)description.FramesPerPacket,
                BitsPerChannel = (short)description.BitsPerChannel,
            };
        }

        public static AudioStreamBasicDescription ConvertToPlatformFormatDescription(IAudioFormatDescription description)
        {
            var formatType = AudioFormatDescriptionTools.ConvertToPlatformFormat(description.FormatTag);

            var desc = new AudioStreamBasicDescription(formatType);
            desc.SampleRate = description.SampleRate;
            desc.ChannelsPerFrame = description.ChannelsPerFrame;
            desc.BitsPerChannel = description.BitsPerChannel;
            desc.BytesPerPacket = description.BytesPerPacket;
            desc.BytesPerFrame = description.BytesPerFrame;
            desc.FramesPerPacket = description.FramesPerPacket;
			desc.FormatFlags = AudioFormatFlags.IsSignedInteger | AudioFormatFlags.IsBigEndian | AudioFormatFlags.IsPacked;

			return desc;
        }

        public static AudioFormats ConvertToStandsrtFormat(AudioFormatType formatType)
        {
            return m_platformToStandart[formatType];
        }

        public static AudioFormatType ConvertToPlatformFormat(AudioFormats formatType)
        {
            return m_standartToPlatform[formatType];
        }

		public static void PlatformReverseConvertation(byte[] buffer, int bitsPerChannel, int channelsPerFrame)
		{
			int bytePerChannel = bitsPerChannel / 8;
			int channels = channelsPerFrame;
			int bytePerFrame = bytePerChannel * channels;
			int bufferFrames = buffer.Length / bytePerFrame;

			int chennelOffset = 0;
			int frameOffset = 0;

			int frame = 0;
			int chennel = 0;
			while (frame < bufferFrames)
			{
				chennel = 0;
				frameOffset = frame * bytePerFrame;
				while (chennel < channels)
				{
					chennelOffset = bytePerChannel * chennel;

					Array.Reverse(buffer, frameOffset + chennelOffset, bytePerChannel);

					chennel++;
				}

				frame++;
			}
		}
    }
}

