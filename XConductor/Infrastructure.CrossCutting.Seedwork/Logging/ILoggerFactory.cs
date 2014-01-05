namespace XConductor.Infrastructure.CrossCutting.Seedwork.Logging
{
    public interface ILoggerFactory
    {
        ILogger Create();
    }
}
