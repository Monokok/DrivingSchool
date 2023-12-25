using DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DTO
{
    public class lessonDTO
    {
        public lessonDTO(lesson Lesson)
        {
            id = Lesson.id;
            date = Lesson.date;
            topic = Lesson.topic;
            type_id = Lesson.type_id;
            teacher_id = Lesson.teacher_id;
            student_id = Lesson.student_id;
            payment_type_id = Lesson.payment_type_id;
            cost = Lesson.cost;

            if (Lesson.status == "Назначено ") status = "Назначено";
            else if (Lesson.status == "Отменено") status = "Отменено";
            else if (Lesson.status == "Назначено") status = "Назначено";
            else if (Lesson.status == "Отменено ") status = "Отменено";
            else if (Lesson.status == "Отменено  ") status = "Отменено";
            else status = "Отменено";



            car_id = Lesson.car_id;
            if (Lesson.payment_type_id == 0)
            {
                payment_status = "Оплачено";
            }
            else if (Lesson.payment_type_id == 1)
            {
                payment_status = "Неоплачено";
            }

        }
        public int id { get; set; }
        public int car_id { get; set; }
        public string car_name { get; set; }
        public string payment_status { get; set; }


        public DateTime date { get; set; }

        public string topic { get; set; }
        public string status { get; set; }
        public string lesson_type { get; set; }
        public string teacher_name { get; set; }
        public string student_name { get; set; }
        public string tnumber { get; set; }
        public int type_id { get; set; }

        public int teacher_id { get; set; }

        public int student_id { get; set; }

        public int payment_type_id { get; set; }
        public string cost { get; set; }

    }
}
