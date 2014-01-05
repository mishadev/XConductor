using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace XConductor.Application.Shared.Service
{
    public class ApplicationServiceSettings
    {
        public IDictionary<string, object> AsPropertiesDictionary()
        {
#if MONOTOUCH
            var results = TypeDescriptor.GetProperties(this.GetType()).Cast<PropertyDescriptor>()
                .ToDictionary(p => p.Name, p => p.GetValue(this), StringComparer.OrdinalIgnoreCase);

#else
            var results = this.GetType().GetRuntimeProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(this), StringComparer.OrdinalIgnoreCase);
               
#endif

            return results;
        }
    }
}
