using System;
using System.Collections.Generic;
using System.Text;

namespace Ecliptic.Models
{
    public class Worker
    {
        public string FirstName  { get; set; }
        public string SecondName { get; set; }
        public string LastName   { get; set; }

        public string Status { get; set; } //ученик-декан....


        public string Details { get; set; } //

        public string Email   { get; set; }
        public string Phone   { get; set; }   // телефон
        public string Site    { get; set; }    // сайт


        public List<string> Notes { get; set; }


        public override string ToString()
        {
            return FirstName + " " + SecondName + " " + LastName;
        }
    }
}
