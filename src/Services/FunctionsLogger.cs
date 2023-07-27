namespace MI.Platform.Api.Services
{
    using Microsoft.Azure.WebJobs.Logging;
    using Microsoft.Extensions.Logging;
    using System;

    class FunctionsLogger<T> : ILogger<T>
    {
        readonly ILogger _logger;

        public FunctionsLogger(ILoggerFactory factory)
            => _logger = factory.CreateLogger(LogCategories.CreateFunctionUserCategory(typeof(T).FullName));

        public IDisposable BeginScope<TState>(TState state) =>
            _logger.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) => 
            _logger.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            => _logger.Log(logLevel, eventId, state, exception, formatter);
    }
}
