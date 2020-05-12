using Ecliptic.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Android.Graphics;

namespace Ecliptic.Models
{
    public static class PointData
    {
        public static List<PointM> Points { get; set; }

        static PointData()
        {
            Points = new List<PointM>();
        }
    }
}