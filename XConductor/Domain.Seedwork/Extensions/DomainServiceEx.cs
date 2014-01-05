using System;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Media.EventArgs;

namespace XConductor.Domain.Seedwork.Extensions
{
    public static class DomainServiceEx
    {
        public static void SubscribeService(this IChainedDomainService service, EventHandler<MediaEventArgs> start, EventHandler<MediaEventArgs> stop, EventHandler<MediaEventArgs> data, EventHandler<MediaEventArgs> results)
        {
            service.OnResultsAvalable -= results;

            service.SubscribeService(start, stop, data);

            service.OnResultsAvalable += results;
        }

        public static void SubscribeService(this IObservableDomainService service, EventHandler<MediaEventArgs> start, EventHandler<MediaEventArgs> stop, EventHandler<MediaEventArgs> data)
        {
            service.OnDataAvalable -= data;

            service.SubscribeService(start, stop);

            service.OnDataAvalable += data;
        }

        public static void SubscribeService(this IDomainService service, EventHandler<MediaEventArgs> start, EventHandler<MediaEventArgs> stop)
        {
            service.OnStart -= start;
            service.OnStop -= stop;

            service.OnStart += start;
            service.OnStop += stop;
        }
    }
}