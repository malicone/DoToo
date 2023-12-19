using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoToo.Models
{
    public class BaseEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
