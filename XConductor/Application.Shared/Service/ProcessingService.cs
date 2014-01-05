using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XConductor.Application.Shared.Service.Abstractions;
using XConductor.Application.Shared.Service.EventArgs;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Extensions;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Application.Shared.Service
{
    public class ProcessingService : IProcessingService 
    {
        public event EventHandler<ProcessingEventArgs> OnProcessingStart;
        public event EventHandler<ProcessingEventArgs> OnProcessingStop; 
        public event EventHandler<ProcessingEventArgs> OnProcessingDataAvailable;
        public event EventHandler<ProcessingEventArgs> OnProcessingResultsAvailable;
        
        private IDictionary<object, List<IChainedDomainService>> _chain = new Dictionary<object, List<IChainedDomainService>>();
        private IDictionary<object, IObservableDomainService> _chainBases = new Dictionary<object, IObservableDomainService>();

        public void Create(object key, IObservableDomainService service)
        {
            if (!this._chain.ContainsKey(key) && service != null)
            {
                this._chain.Add(key, new List<IChainedDomainService>());
                this._chainBases.Add(key, service);
            }
        }

        public void Add(object key, IChainedDomainService chain)
        {
            if (this._chain.ContainsKey(key))
            {
                if (chain != null)
                {
                    var head = this._chain[key].LastOrDefault() ?? (IObservableDomainService)this._chainBases[key];
                    if (head != null)
                    {
#if MONOTOUCH
                        if (head.Output.IsAssignableFrom(chain.Input))
                        {
                            this._chain[key].Add(chain);
                        }
#else
                        if (head.Output.GetTypeInfo().IsAssignableFrom(chain.Input.GetTypeInfo()))
                        {
                            this._chain[key].Add(chain);
                        }
#endif
                    }
                }
            }
        }

        public void AddRange(object key, params IChainedDomainService[] chains)
        {
            if (chains != null)
            {
                foreach (var chain in chains)
                {
                    this.Add(key, chain);
                }
            }
        }

        public void Clear(object key)
        {
            if (this._chain.ContainsKey(key))
            {
                this._chain.Remove(key);
                this._chainBases.Remove(key);
            }
        }

        public void Clear()
        {
            foreach (var key in this._chain.Keys)
            {
                this.Clear(key);
            }
        }

        public async Task Start(object key, object settings)
        {
            if (this._chain.ContainsKey(key))
            {
				await this._chainBases[key].Initialize(settings);

                IObservableDomainService head = this._chainBases[key];
                foreach (IChainedDomainService chain in this._chain[key])
                {
					await chain.Initialize (head, settings);

                    head = chain;
                }

                this.SubscribeChain(key, head, this.ProcessingStart, this.ProcessingStop, this.ProcessingDataAvailable, this.ProcessingResultsAvailable);

				await head.Start();
            }
        }

        public async Task Stop(object key)
        {
			if (this._chain.ContainsKey(key))
            {
                var service = this._chain[key].LastOrDefault() ?? (IDomainService)this._chainBases[key];

				await service.Stop();
            }
        }

        private void ProcessingStart(object sender, ProcessingEventArgs e)
        {
            if (this.OnProcessingStart != null)
            {
                this.OnProcessingStart(this, e);
            }
        }

        private void ProcessingStop(object sender, ProcessingEventArgs e)
        {
            if (this.OnProcessingStop != null)
            {
                this.OnProcessingStop(this, e);
            }
        }

        private void ProcessingDataAvailable(object sender, ProcessingEventArgs e)
        {
            if (this.OnProcessingDataAvailable != null)
            {
                this.OnProcessingDataAvailable(this, e);
            }
        }

        private void ProcessingResultsAvailable(object sender, ProcessingEventArgs e)
        {
            if (this.OnProcessingResultsAvailable != null)
            {
                this.OnProcessingResultsAvailable(this, e);
            }
        }

        private void SubscribeChain(
            object key, 
            IObservableDomainService service, 
            EventHandler<ProcessingEventArgs> start, 
            EventHandler<ProcessingEventArgs> stop, 
            EventHandler<ProcessingEventArgs> data,
            EventHandler<ProcessingEventArgs> results)
        {
            EventHandler<MediaEventArgs> processingStart = (s, e) => start(s, new ProcessingEventArgs(key, e.State));
            EventHandler<MediaEventArgs> processingStop = (s, e) => stop(s, new ProcessingEventArgs(key, e.State));
            EventHandler<MediaEventArgs> processingData = (s, e) => data(s, new ProcessingEventArgs(key, e.State));
            EventHandler<MediaEventArgs> processingResults = (s, e) => results(s, new ProcessingEventArgs(key, e.State));

            if (service is IChainedDomainService)
            {
                ((IChainedDomainService)service).SubscribeService(processingStart, processingStop, processingData, processingResults);
            }
            else
            {
                service.SubscribeService(processingStart, processingStop, processingData);
            }
        }

        public void Dispose()
        {
            var services = this._chain.SelectMany(c => c.Value);

            foreach (var service in services)
	        {
                if (service != null) service.Dispose();
	        } 
        }

        public bool IsInitialized
        {
            get { return this._chain.SelectMany(c => c.Value).Any(); }
        }
    }
}
