using Ecliptic.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Ecliptic.Models
{
    public static class BuildingData
    {
        public static List<Building> Buildings { get; set; } // список доступных в системе зданий 

        public static Building CurrentBuilding { get; set; } // текущее загруженое
 
        static BuildingData()
        {
            Buildings = new List<Building>();
        }
    }
}