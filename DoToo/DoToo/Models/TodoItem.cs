using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace DoToo.Models
{
    public class TodoItem : BaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public bool Completed { get; set; }
        public DateTime Due { get; set; }
        public int CategoryId { get; set; }
    }
}
