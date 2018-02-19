using Microsoft.Extensions.Logging;

namespace GOC.Inventory.Infrastructure.Repositories
{
    public class RepositoryBase<T> where T : class
    {
        protected ILogger Logger { get; private set; }

        public RepositoryBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<T>();
        }
    }
}
