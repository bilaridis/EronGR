using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EronNew.Data
{
    public class WalletManager<TUser> : IDisposable where TUser : class
    {
        private readonly ILogger<WalletManager<TUser>> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public WalletManager(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<WalletManager<TUser>> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        #region IDisposable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~WalletManager()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
