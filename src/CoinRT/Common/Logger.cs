using System;
using System.Diagnostics;
using MetroLog;

namespace CoinRT.Common
{
    public static class Logger
    {
        private const string DefaultPatternLayout = "%date{HH:mm:ss} [%level] %logger{2} (%thread) - %m%n";

        private static bool consoleAppenderAdded;

        public static ILogger GetLoggerForDeclaringType()
        {
            var frame = new StackFrame(1);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            return GetLogger(type);
        }

        public static ILogger GetLogger(Type type)
        {
            return LogManagerFactory.DefaultLogManager.GetLogger(type);
        }
    }    
}
