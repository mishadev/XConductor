using System;
using System.Threading;
using System.Threading.Tasks;
using XConductor.Domain.Seedwork.Abstractions;
using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media.EventArgs;
using XConductor.Domain.Seedwork.Media.Exceptions;
using XConductor.Infrastructure.CrossCutting.Shared.Utils;

namespace XConductor.Domain.Shared.Abstractions
{
    public abstract class DomainService : IDomainService
    {
        public event EventHandler<MediaRuntimeExceptionEventArgs> OnError;
        public event EventHandler<MediaEventArgs> OnStart;
        public event EventHandler<MediaEventArgs> OnStop;

        public bool Initialized { get; protected set; }
        public bool Runing { get; protected set; }

        public DomainService(ISettingsService settingsService)
        {
            this.SettingsService = settingsService;
        }

        public async Task Start()
        {
            if (this.Initialized && !this.Runing)
            {
                this.Runing = true;

                await this.StartHook();

				this.Started();
            }
        }

		public async Task Stop()
        {
            if (this.Initialized && this.Runing)
            {
                this.Runing = false;

				await this.StopHook();

				this.Stoped();
            }
        }

        public virtual async Task Initialize(ISettings settings)
        {
			this.ClearEvents();
            if (settings != null && (!settings.Equals(this.Settings) || !this.Initialized))
            {
                this.InitSettings(settings);
                if (this.Settings != null && this.Settings.IsValid())
                {
                    this.Initialized = true;

                    await this.InitializeHook();
                }
            }
        }

        protected virtual void ClearEvents()
        {
            this.OnError = null;
			this.OnStart = null;
			this.OnStop = null;
        }

        public virtual async Task Initialize(params object[] parameters)
        {
            if (parameters == null && parameters.Length < 0) 
				return;

            if (parameters.GetValue(0) is ISettings)
            {
                await this.Initialize((ISettings)parameters.GetValue(0));
            }
            else if (this.SettingsService != null)
            {
                var settings = await this.SettingsService.GetSettings(parameters);

                await this.Initialize(settings);
            }
        }

        protected virtual void Started()
        {
            if (this.OnStart != null)
            {
                this.OnStart(this, MediaEventArgs.Empty);
            }
        }

		protected virtual void Stoped()
        {
            if (this.OnStop != null)
            {
                this.OnStop(this, MediaEventArgs.Empty);
            }
        }

		protected virtual void Error(object state, string message)
        {
            if (this.OnError != null)
            {
                this.OnError(this, new MediaRuntimeExceptionEventArgs(state, new MediaRuntimeException(message)));
            }
			this.ClearEvents();
        }

        protected abstract Task StopHook();
        protected abstract Task StartHook();
        protected abstract Task InitializeHook();
        protected abstract void InitSettings(ISettings settings);
        public abstract ISettings Settings { get; }

        public virtual ISettingsService SettingsService { get; protected set; }
        public virtual void Dispose() 
        {
            var settings = this.Settings as IDisposable;
            var settingsService = this.SettingsService as IDisposable;

            if (settings != null) settings.Dispose();
            if (settingsService != null) settingsService.Dispose();
        }
    }
}
