using Ecliptic.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecliptic.Models
{
    public static class FloorData
    {
        public static Floor CurrentFloor { get; set; }
        public static List<Floor> Floors { get; set; } // список этажей здания

        static FloorData()
        {
            CurrentFloor = null;
            
            Floors = new List<Floor>();
 
            Floors = Floors.OrderBy(f => f.Level).ToList();
        }

        /// <summary>
        /// Получение этажа по его номеру
        /// </summary>
        /// <param name="level">номер этажа</param>
        /// <returns>ссылка на этаж</returns>
        public static Floor GetFloor(int? level)
        {
            foreach (var floor in Floors)
            {
                if (floor.Level == level)
                {
                    return floor;
                }
            }
            return null;
        }
    }
}