using System;
using System.Threading.Tasks;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.Shared.Abstractions.Settings
{
    public abstract class ChainedDomainSettingsService<TInput, TOutput> : SettingsService
    {
        protected override ISettings DefaultSettings(params object[] parameters)
        {
            if (parameters != null && parameters.Length > 1)
            {
                var service = parameters.GetValue(0) as IObservableDomainService;
                var settings = parameters.GetValue(1);
                var observable = service as IDataObservable<TInput>;

                if (observable != null)
                {
                    return this.DefaultSettings(observable, service.AudioFormat, settings);
                }
            }
            return null;
        }

        protected abstract IChainedDomainSettings<TInput, TOutput> DefaultSettings(IDataObservable<TInput> source, IAudioFormatDescription format, object settings);
    }
}
