using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic.Models
{
    public class Worker
    {
        public int Id { get; set; }

        public string FirstName  { get; set; }
        public string SecondName { get; set; }
        public string LastName   { get; set; }

        public string Status { get; set; } 

        public string Details { get; set; } 

        public string Email { get; set; }
        public string Phone { get; set; }   
        public string Site  { get; set; }    

        public virtual Room Room { get; set; }
        public virtual int? RoomId { get; set; }


        public Worker() { }

        public Worker(Worker worker)
        {
            Id = worker.Id;
            FirstName  = worker.FirstName;
            SecondName = worker.SecondName;
            LastName   = worker.LastName;
            Status  = worker.Status;
            Details = worker.Details;
            Email   = worker.Email;
            Phone   = worker.Phone;
            Site    = worker.Site;
        }

        public Worker(int id,
                      string firstName, string secondName, string lastName,
                      string status = null, string details = null,
                      string email  = null, string phone   = null, string site = null)
        {
            Id = id;
            FirstName  = firstName;
            SecondName = secondName;
            LastName   = lastName;
            Status  = status;
            Details = details;
            Email   = email;
            Phone   = phone;
            Site    = site;
        }

        public override string ToString()
        {
            return FirstName + " " + SecondName + " " + LastName;
        }
    }
}
