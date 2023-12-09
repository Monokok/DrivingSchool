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
        }
        public int id { get; set; }

        public DateTime date { get; set; }

        public string topic { get; set; }

        public int type_id { get; set; }

        public int teacher_id { get; set; }

        public int student_id { get; set; }

        public int payment_type_id { get; set; }
        public string cost { get; set; }

    }
}
