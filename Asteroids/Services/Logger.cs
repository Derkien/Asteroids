using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;

namespace Asteroids.Services
{
    public static class ApplicationLogging
    {
        private static ILoggerFactory Factory = null;

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (Factory == null)
                {
                    Factory = new LoggerFactory();
                    Factory.AddProvider(new DebugLoggerProvider());
                    //TODO: Consider what to do with file logging. This solution rise CPU time to time.
                    // Factory.AddFile("asteroids_app.log");
                }

                return Factory;
            }
        }

        public static ILogger CreateLogger<T>() =>
          LoggerFactory.CreateLogger<T>();

        public static ILogger CreateLogger(Type type) =>
          LoggerFactory.CreateLogger(type);
    }
}
