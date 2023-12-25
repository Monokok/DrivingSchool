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
            start_date = course.start_date;
            start_date_string = "Дата начала: " + course.start_date.ToString("f");
            end_time_string = "Дата завершения: " + course.end_time.ToString("f");
            end_time = course.end_time;
            cost = course.cost;
            lecture_hours= course.lecture_hours;
            driving_hours = course.driving_hours;
            teacher_id = course.teacher_id;
            student_count = course.student_count;//максимальное число зарегестрированных людей на курс - потенциальная функция на будущее
            driving_hours_string = driving_hours + " часов практики";
            lecture_hours_string = lecture_hours + " часов теории";
            cost_string = "Цена:" + " " + cost.ToString() + " " + "₽";

            registered_people = 0;//счётчик зарегестрированных на курс людей

        }
        public string category_teacher {  get; set; }
        public int id { get; set; }
        public int registered_people { get; set; }

        public int category_id { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_time { get; set; }

        public string start_date_string {  get; set; }
        public string end_time_string {  get; set; }

        public int cost { get; set; }
        public string cost_string { get; set; }

        public int lecture_hours { get; set; }

        public int driving_hours { get; set; }

        public int teacher_id { get; set; }

        public int student_count { get; set; }

        public string teacher_name { get; set;}
        public string category_name { get; set; }
        public string category_fullname { get; set; }


        public string driving_hours_string { get; set; }
        public string lecture_hours_string { get; set; }
    }
}
