using DoToo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoToo.Repositories
{
    public abstract class BaseRepo<T> : IBaseRepo<T> where T : BaseEntity, new()
    {
        public event EventHandler<T> OnEntityAdded;
        public event EventHandler<T> OnEntityUpdated;
        public event EventHandler<T> OnEntityDeleted;

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~BaseRepo()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose( disposing: false );
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose( disposing: true );
            GC.SuppressFinalize( this );
        }

        public abstract Task<T> Get( int id );
        public abstract Task<List<T>> GetAll();
        public abstract Task<int> Add( T entity );
        public abstract Task Update( T entity );
        public abstract Task<int> AddOrUpdate( T entity );
        public abstract Task Delete( int id );

        protected virtual void Dispose( bool disposing )
        {
            if ( !_disposedValue )
            {
                if ( disposing )
                {
                    // TODO: dispose managed state (managed objects)
                    FreeManagedResources();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                FreeUnmanagedResources();
                _disposedValue = true;
            }
        }

        protected abstract void FreeManagedResources();
        protected abstract void FreeUnmanagedResources();

        private bool _disposedValue;
    }
}