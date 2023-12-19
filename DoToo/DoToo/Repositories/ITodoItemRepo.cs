using DoToo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoToo.Repositories
{
    public interface ITodoItemRepo : IBaseRepo<TodoItem> 
    {
        Task<List<TodoItem>> GetByCategory( int categoryId );
    }
}
