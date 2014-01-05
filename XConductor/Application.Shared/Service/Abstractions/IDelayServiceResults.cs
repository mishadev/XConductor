using System.Collections.Generic;

namespace XConductor.Application.Shared.Service.Abstractions
{
    public interface IDelayServiceResults
    {
        double[] Delays { get; }

		bool IsValidResults { get; }
    }
}
