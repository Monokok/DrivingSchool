using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTO
{
    public class courseDTO
    {
        public courseDTO(course course) 
        { 
            id = course.id;
            category_id = course.category_id;
            start_date= course.start_date;
            end_time = course.end_time;
            cost = course.cost;
            lecture_hours= course.lecture_hours;
            driving_hours = course.driving_hours;
            teacher_id = course.teacher_id;
            student_count = course.student_count;
        }

        public int id { get; set; }

        public int category_id { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_time { get; set; }

        public int cost { get; set; }

        public int lecture_hours { get; set; }

        public int driving_hours { get; set; }

        public int teacher_id { get; set; }

        public int student_count { get; set; }

    }
}
