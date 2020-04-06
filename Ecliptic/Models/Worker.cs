using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic.Models
{
    [Table("Workers")]
    public class Worker
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }


        public string FirstName  { get; set; }
        public string SecondName { get; set; }
        public string LastName   { get; set; }

        public string Status { get; set; } //ученик-декан....


        public string Details { get; set; } //

        public string Email   { get; set; }
        public string Phone   { get; set; }   // телефон
        public string Site    { get; set; }    // сайт

        public override string ToString()
        {
            return FirstName + " " + SecondName + " " + LastName;
        }
    }
}
