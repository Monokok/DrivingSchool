using DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTO
{
    public class studentDTO
    {
        
        public studentDTO(student student)
        {
            id = student.id;
            name = student.first_name + " " + student.last_name;
            first_name = student.first_name;
            middle_name = student.middle_name;
            last_name = student.last_name;
            tnumber = student.tnumber;
            paid_hours = student.paid_hours;//???
        }
        public int id { get; set; }
        public string first_name { get; set; }
        public string name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string tnumber { get; set; }
        public int paid_hours { get; set; }

    }
}
