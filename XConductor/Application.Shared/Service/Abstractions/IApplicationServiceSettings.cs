using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XConductor.Application.Shared.Service.Abstractions
{
    public interface IApplicationServiceSettings
    {
        IDictionary<string, object> AsPropertiesDictionary();
    }
}
