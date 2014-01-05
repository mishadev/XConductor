using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using XConductor.Domain.Seedwork.Abstractions.Settings;

namespace XConductor.Domain.Shared.Abstractions.Settings
{
    public abstract class SettingsService : ISettingsService
    {
        protected Dictionary<string, object> m_contextValues = new Dictionary<string, object>();

        public void SetContext(params KeyValuePair<string, object>[] values)
        {
			if (values != null)
            {
				foreach (var value in values)
                {
					if (value.Value != null) {
						if (this.m_contextValues.ContainsKey(value.Key))
						{
							this.m_contextValues[value.Key] = value.Value;
						}
						else
						{
							this.m_contextValues.Add(value.Key, value.Value);
						}
					}
                }
            }
        }

        protected void PopulateByContext(ISettings settings)
        {
#if MONOTOUCH
			var props = TypeDescriptor.GetProperties(settings.GetType()).Cast<PropertyDescriptor>();
#else
            var props = settings.GetType().GetRuntimeProperties();
#endif
			foreach (var prop in props) {
				if(this.m_contextValues.ContainsKey(prop.Name))
				{
					var val = this.m_contextValues[prop.Name];

					if(prop.PropertyType.IsAssignableFrom(val.GetType()))
						prop.SetValue(settings, val);
				}
			}
        }

        public virtual async Task<ISettings> GetSettings(params object[] parameters)
        {
			var settings = this.DefaultSettings(parameters);

			this.PopulateByContext(settings);

			return settings;
        }

        protected abstract ISettings DefaultSettings(params object[] parameters);
    }
}
