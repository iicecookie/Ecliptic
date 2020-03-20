using System;
using System.Collections.Generic;

namespace Ecliptic.Models
{
    public class Room : ICloneable
    {
        public string Name { get; set; } // Имя аудитории +
        public int Floor { get; set; } // этаж
        public string Details { get; set; } //
        public string Description { get; set; } // описание
        public List<Worker> Staff { get; set; } // работники - мб микроструктуру для работника // добавить как  отдельный класс дл ябазы данных со своими полями телефн 
        public string Timetable { get; set; } // расписание
        public string Phone { get; set; } // телефон
        public string Site { get; set; } // сайт
        public bool Favorite { get; set; } // избранность

        public List<Note> Notes = new List<Note>();  //заметки

        private List<IVertex> Door; // выходы из аудиторий 
        private List<IVertex> Walls;// cтеныа удитории

        public object Clone()
        {
            return new Room
            {
                Name = this.Name,
                Details = this.Details,
                Description = this.Description,

                //    AnyInform   = this.AnyInform,
                //    Location    = this.Location,
                Floor = this.Floor,
                Staff = this.Staff,
                Timetable = this.Timetable,
                Phone = this.Phone,
                Site = this.Site,
                Favorite = this.Favorite,
            };
            //return this.MemberwiseClone();
        }
    }

    internal interface IVertex
    {
    }
}