using DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTO
{
    public class teacherDTO
    {
        public teacherDTO(teacher teacher)
        {
            id = teacher.id;
            first_name = teacher.first_name;
            middle_name = teacher.middle_name;
            last_name = teacher.last_name;
            tnumber = teacher.tnumber;
        }
        public int id { get; set; }
        public string first_name { get; set; }

        public string middle_name { get; set; }

        public string last_name { get; set; }
        public string tnumber { get; set; }
    }
}
