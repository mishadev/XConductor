using System;
using System.Threading.Tasks;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Common;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.Shared.Abstractions
{
    public abstract class ObservableDomainService<TOutput> : DomainService, IObservableDomainService<TOutput>
    {
        public event EventHandler<DataEventArgs<TOutput>> OnDataAvalable;

        public Type Output
        {
            get
            {
                return typeof(TOutput);
            }
        }

        public ObservableDomainService(ISettingsService settingsService)
            : base(settingsService)
        { }

        public virtual async Task Subscribe(Action<TOutput> onNext, Action<Exception> onError = null, Action onCompleted = null)
        {
            if(onNext == null)
                throw new ArgumentNullException("onNext");

			this.OnDataAvalable += (s, e) => onNext(e.Data);
			onCompleted += async () => await this.Stop();
			onError += ex => this.Error(ex, ex.Message);

            try
            {
                await this.Start();
            }
            catch (Exception ex)
            {
                onError(ex);
            }

			onCompleted();
        }

        protected void DataAvalable(TOutput date)
        {
            if (this.OnDataAvalable != null)
            {
                this.OnDataAvalable(this, new DataEventArgs<TOutput>(date));
            }
        }

        protected override void ClearEvents()
        {
            base.ClearEvents();

            this.OnDataAvalable = null;
        }

        public abstract IAudioFormatDescription AudioFormat { get; }

        private EventHandler<DataEventArgs<TOutput>> m_onDataAvalable;
        event EventHandler<MediaEventArgs> IObservableDomainService.OnDataAvalable
        {
            add 
            { 
                OnDataAvalable -= this.m_onDataAvalable;
                this.m_onDataAvalable = (s, e) => value(s, new MediaEventArgs(e.Data));
                OnDataAvalable += this.m_onDataAvalable;
            }
            remove 
            {
                OnDataAvalable -= this.m_onDataAvalable;
            }
        }
    }
}
