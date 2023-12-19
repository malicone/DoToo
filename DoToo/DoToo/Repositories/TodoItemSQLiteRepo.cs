using DoToo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoToo.Repositories
{
    public class TodoItemSQLiteRepo : SQLiteRepo<TodoItem>, ITodoItemRepo
    {
        public async Task<List<TodoItem>> GetByCategory( int categoryId )
        {
            await CreateConnection();
            var query = Connection.Table<TodoItem>().Where( i => i.CategoryId == categoryId );
            return await query.ToListAsync();
        }        
    }
}
