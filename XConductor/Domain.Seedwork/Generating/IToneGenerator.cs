using System;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;

namespace XConductor.Domain.Seedwork.Generating
{
    public interface IToneGenerator : IObservableDomainService<byte[]>
    { }
}

namespace XConductor.Domain.Seedwork.Generating.Settings
{
	public class WaveConfiguration
	{
		public int Frequency;
		public OrdinalNumbers Chennels;
		public bool AllChannelsSimultaneously;
	}
}
