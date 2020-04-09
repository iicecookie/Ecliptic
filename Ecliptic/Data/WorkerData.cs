﻿using Ecliptic.Models;
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
        public static List<Worker> Workers { get; private set; }

        static WorkerData()
        {
            Workers = new List<Worker>();

            using (var db = new ApplicationContext())
            {
                if (db.Workers.Count() == 0)
                {
                    db.Workers.Add(new Worker
                    {
                        FirstName = "Celivans",
                        SecondName = "irina",
                        LastName = "vasileva",

                        Details = "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                        Status = "Teacher",

                        Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                        Phone = "8(906)6944309",
                        Email = "seliv@mail.ru",
                        RoomId = RoomData.Rooms.ElementAt(0).RoomId,

                    });
                    db.Workers.Add(new Worker
                    {
                        FirstName = "Uraeva",
                        SecondName = "Elena",

                        Details = "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                        Status = "Teacher",

                        Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                        Phone = "8(906)6944309",
                        Email = "seliv@mail.ru",
                        RoomId = RoomData.Rooms.ElementAt(1).RoomId,
                    });
                    db.Workers.Add(new Worker
                    {
                        FirstName = "Makarov",
                        SecondName = "Kirya",

                        Details = "The American black bear is a medium-sized bear native to North America. It is the continent's smallest and most widely distributed bear species. American black bears are omnivores, with their diets varying greatly depending on season and location. They typically live in largely forested areas, but do leave forests in search of food. Sometimes they become attracted to human communities because of the immediate availability of food. The American black bear is the world's most common bear species.",
                        Status = "Teacher",

                        Site = "http://xn--80afqpaigicolm.xn--p1ai/csharp/csharp-otkryt-url-v-browser/",
                        Phone = "8(906)6944309",
                        Email = "seliv@mail.ru",
                        RoomId = RoomData.Rooms.ElementAt(2).RoomId,
                    });

                    db.SaveChanges();
                }

                Workers = db.Workers.Include(u => u.Room)
                                    .ToList();

                RoomData.Rooms = db.Rooms.Include(u => u.Workers)
                                         .ToList();
            }
        }
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
