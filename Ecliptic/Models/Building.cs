using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic.Models
{
    [Table("Buildings")]
    public class Building
    {
        [PrimaryKey, AutoIncrement, Column("id")]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
