using System.Collections.Generic;
using System.Threading.Tasks;

namespace XConductor.Domain.Seedwork.Abstractions.Settings
{
    public interface ISettingsService
    {
        Task<ISettings> GetSettings(params object[] parameters);

        void SetContext(params KeyValuePair<string, object>[] values);
    }
}