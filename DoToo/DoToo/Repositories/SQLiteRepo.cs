using DoToo.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DoToo.Repositories
{
    public class SQLiteRepo<T> : BaseRepo<T> where T : BaseEntity, new()
    {
        public new event EventHandler<T> OnEntityAdded;
        public new event EventHandler<T> OnEntityUpdated;
        public new event EventHandler<T> OnEntityDeleted;

        public async override Task<int> Add( T entity )
        {
            await CreateConnection();
            int id = await Connection.InsertAsync( entity );
            OnEntityAdded?.Invoke( this, entity );
            return id;
        }

        public async override Task<int> AddOrUpdate( T entity )
        {
            if ( entity.Id == 0 )
            {
                return await Add( entity );
            }
            else
            {
                await Update( entity );
                return entity.Id;
            }
        }

        public async override Task<T> Get( int id )
        {
            await CreateConnection();
            T entity = await Connection.GetAsync<T>( id );
            return entity;
        }

        public async override Task<List<T>> GetAll()
        {
            await CreateConnection();
            return await Connection.Table<T>().ToListAsync();
        }

        public async override Task Update( T entity )
        {
            await CreateConnection();
            await Connection.UpdateAsync( entity );
            OnEntityUpdated?.Invoke( this, entity );
        }

        public async override Task Delete( int id )
        {
            T entity = await Get( id );
            await Connection.DeleteAsync( entity );
            OnEntityDeleted?.Invoke( this, entity );
        }

        protected override void FreeManagedResources() { }                    

        protected override void FreeUnmanagedResources()
        {
            Connection.CloseAsync();
        }

        protected async Task CreateConnection()
        {
            if ( Connection != null )
            {
                return;
            }

            var documentPath = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
            var databasePath = Path.Combine( documentPath, _DB_FILE_NAME );
            Connection = new SQLiteAsyncConnection( databasePath, FLAGS );
            await Connection.CreateTableAsync<T>();
        }

        private const SQLiteOpenFlags FLAGS =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache;

        protected SQLiteAsyncConnection Connection { get; private set; }

        private const string _DB_FILE_NAME = "TodoItems.db";
    }
}