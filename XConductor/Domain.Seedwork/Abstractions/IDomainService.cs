using System;
using System.Threading.Tasks;

using XConductor.Domain.Seedwork.Abstractions.Settings;
using XConductor.Domain.Seedwork.Media.EventArgs;

namespace XConductor.Domain.Seedwork.Abstractions
{
    public interface IDomainService : IDisposable
    {
        event EventHandler<MediaRuntimeExceptionEventArgs> OnError;
        event EventHandler<MediaEventArgs> OnStart;
        event EventHandler<MediaEventArgs> OnStop;

        bool Initialized { get; }

        bool Runing { get; }

        ISettings Settings { get; }

        ISettingsService SettingsService { get; }

        Task Initialize(ISettings settings);

        Task Initialize(params object[] parameters);

        Task Start();

        Task Stop();
    }
}
