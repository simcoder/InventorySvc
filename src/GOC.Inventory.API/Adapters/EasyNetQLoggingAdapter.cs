using System;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace GOC.Inventory.API.Adapters
{
    public class EasyNetQLoggingAdapter : IEasyNetQLogger
    {
        readonly ILogger _logger;
        public EasyNetQLoggingAdapter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EasyNetQLoggingAdapter>();
        }
        public void DebugWrite(string format, params object[] args)
        {
            _logger.LogDebug(format);
        }

        public void ErrorWrite(string format, params object[] args)
        {
            _logger.LogError(format);
        }

        public void ErrorWrite(Exception exception)
        {
            _logger.LogError(exception.Message);
        }

        public void InfoWrite(string format, params object[] args)
        {
            _logger.LogInformation(format);
        }
    }
}
