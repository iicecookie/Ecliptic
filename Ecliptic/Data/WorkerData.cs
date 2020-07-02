using Ecliptic.Models;
using Ecliptic.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecliptic.Data
{
    public static class WorkerData
    {
        public static List<Worker> Workers { get; set; } // работники помещений здания

        static WorkerData()
        {
            Workers = new List<Worker>();
        }

        /// <summary>
        /// Получение работника по его инициалам
        /// </summary>
        /// <param name="first">Имя</param>
        /// <param name="second">Фамилия</param>
        /// <param name="last">Отчество</param>
        /// <returns>Работник</returns>
        public static Worker GetWorker(string first, string second = null, string last = null)
        {
            foreach (var i in Workers)
            {
                if (i.FirstName == first &&
                    i.SecondName == second &&
                    i.LastName == last)
                    return i;
            }
            return null;
        }
    }
}