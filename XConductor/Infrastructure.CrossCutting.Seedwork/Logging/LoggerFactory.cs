using System;

namespace XConductor.Infrastructure.CrossCutting.Seedwork.Logging
{
    public static class LoggerFactory
    {
        static ILoggerFactory _currentLogFactory = null;

        public static void SetCurrent(ILoggerFactory logFactory)
        {
            _currentLogFactory = logFactory;
        }

        public static ILogger CreateLog()
        {
            return (_currentLogFactory != null) ? _currentLogFactory.Create() : null;
        }
    }
}
