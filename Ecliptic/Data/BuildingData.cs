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
        public static List<Building> Buildings { get; set; }

        public static Building CurrentBuilding { get; set; }
 
        static BuildingData()
        {
            Buildings = new List<Building>();
        }
    }
}