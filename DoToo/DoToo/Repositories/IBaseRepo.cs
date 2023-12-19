using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoToo.Repositories
{
    public interface IBaseRepo<T> : IDisposable
    {
        event EventHandler<T> OnEntityAdded;
        event EventHandler<T> OnEntityUpdated;
        event EventHandler<T> OnEntityDeleted;
        Task<T> Get( int id );
        Task<List<T>> GetAll();
        Task<int> Add( T entity );
        Task Update( T entity );
        Task<int> AddOrUpdate( T entity );
        Task Delete( int id );
    }
}
