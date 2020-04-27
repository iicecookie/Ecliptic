using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic.Models
{
    public class Worker
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }

        public string Status { get; set; } //ученик-декан....


        public string Details { get; set; } //

        public string Email { get; set; }
        public string Phone { get; set; }   // телефон
        public string Site { get; set; }    // сайт

        public virtual Room Room { get; set; }
        public virtual int? RoomId { get; set; }



        public Worker()
        {
        }

        public Worker(int id, string firstName, string secondName, string lastName, string status, string details, string email, string phone, string site)
        {
            Id = id;
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            Status = status;
            Details = details;
            Email = email;
            Phone = phone;
            Site = site;
        }

        public override string ToString()
        {
            return FirstName + " " + SecondName + " " + LastName;
        }
    }
}
