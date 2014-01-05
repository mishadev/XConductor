using System;
using System.Collections;
using System.Threading.Tasks;

using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Infrastructure.CrossCutting.Seedwork.Utils;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.Shared.Abstractions
{
    public abstract class ChainedDomainService<TSettings, TInput, TOutput> : ObservableDomainService<TOutput>, IChainedDomainService<TSettings, TInput, TOutput>
        where TSettings : IChainedDomainSettings<TInput, TOutput>
        where TInput : class, ICollection
        where TOutput : class, ICollection
    {
        public event EventHandler<MediaEventArgs> OnResultsAvalable;

        protected TSettings m_settings;
        private IDataObservable<TInput> m_source;
        private IDisposable m_subscription;
        private IAudioFormatDescription m_audioFormat;

        public Type Input
        {
            get { return typeof(TInput); }
        }

        public override IAudioFormatDescription AudioFormat { get { return this.m_audioFormat; } }

        public override ISettings Settings
        {
            get { return this.m_settings; }
        }

        public ChainedDomainService(ISettingsService settingsService)
            : base(settingsService)
        { }

        private IDataObservable<TInput> CreateBytePushSynchronizator(IDataObservable<TInput> source)
        {
            return new PushSynchronizator<TInput>(source, this.m_settings.PackageSize);
        }

        public override void Dispose()
        {
            if (this.m_source != null) this.m_source.Dispose();

            base.Dispose();
        }

        protected override async Task StopHook()
        { }

		protected override async Task StartHook()
        {
			if (this.m_subscription != null) this.m_subscription.Dispose();

			this.m_subscription = this.m_source.Subscribe(
				onNext: data => this.DataAvalable(this.Processing(data)),
				onCompleted: async () => await this.Stop());
        }

		protected override async Task InitializeHook()
        {
			this.ChainedInitializeHook();

			this.m_source = this.CreateBytePushSynchronizator(this.m_settings.Source);
			this.m_audioFormat = this.m_settings.FormatDescription;
        }

        protected virtual void ResultsAvalable(object result)
        {
            if (this.OnResultsAvalable != null)
            {
                this.OnResultsAvalable(this, new MediaEventArgs(result));
            }
        }

        protected override void ClearEvents()
        {
            base.ClearEvents();

            this.OnResultsAvalable = null;
        }

        protected abstract void ChainedInitializeHook();
        protected abstract TOutput Processing(TInput input);
    }
}