
using DomainModel;
using Interfaces.DTO;
using Interfaces.Repository;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class StudentService : IStudentService //сервис студента - все связанные с ним ЕГО возможности как пользователя
    {
        #region Администратор

        public List<lessonDTO> GetAmountLessonsForPeriod(DateTime _start, DateTime _end)
        {
            DateTime start, end;
            if (_start > _end)
            {
                start = _end;
                end = _start;
            }
            else
            {
                start = _start;
                end = _end;
            }
            List<carDTO> cars = db.Cars.GetList().Select(c => new carDTO(c)).ToList();//получили все машины

            List <studentDTO> students = db.Students.GetList().Select(st => new studentDTO(st)).ToList();
            var lessons = db.Lessons.GetList().Where(l => l.date >= start && l.date <= end).Select(l => new lessonDTO(l));//получаем список занятий за период
            var res = lessons.Where(l => l.status != "Отменено").ToList();//убираем занятия которые отменены

            foreach (var les in res)//подгружаем данные о ТС
            {
                foreach (var crs in cars)
                {
                    if (les.car_id == crs.id)
                    {
                        les.car_name = crs.full_car_name;
                    }
                }
            }
            foreach (var item in students)
            {
                foreach(var les in res)
                {
                    if (item.id == les.student_id)
                    {
                        les.student_name = item.name;
                    }
                }
            }
            return res;
        }

        public List<courseDTO> GetAllCoursesForAdmin(DateTime _start, DateTime _end)
        {
            DateTime start, end;
            if (_start > _end)
            {
                start = _end;
                end = _start;
            }
            else
            {
                start = _start;
                end = _end;
            }
            List<courseDTO> list = db.Courses.GetList().Where(c => c.start_date >= start && c.end_time <= end).Select(i => new courseDTO(i)).ToList();//добыли все курсы
            List<invite_courseDTO> invitations = db.Invitations.GetList().Select(i => new invite_courseDTO(i)).ToList();
            List<teacherDTO> teachers = db.Teachers.GetList().Select(t => new teacherDTO(t)).ToList();
            foreach(var inv in invitations) //проходимся по всем регистрациям
            {  
                if (list.Find(c => c.id == inv.group_id) != null)//находим курс из регистрации
                {
                    list.Find(c => c.id == inv.group_id).registered_people += 1;//у него в поле += 1 к числу людей === число людей что записаны на него
                }
            }
            foreach(var crs in list)
            {
                if (teachers.Find(t => t.id == crs.teacher_id) != null)
                {
                    crs.teacher_name = teachers.Find(t => t.id == crs.teacher_id).name;
                }
            }
            return list;
        }
        #endregion

        #region Преподаватель
        //преподавательское:
        public List<lessonDTO> GetTeacherReportLessons(int _teacher_id, DateTime _start, DateTime _end)
        {
            List<carDTO> cars = db.Cars.GetList().Where(c => c.teacher_id == _teacher_id).Select(c => new carDTO(c)).ToList();//все машинки у конкретного учителя

            DateTime start, end;
            if (_start > _end)
            {
                start = _end;
                end = _start;
            }
            else
            {
                start = _start;
                end = _end;
            }
            List<lessonDTO> result = db.Lessons.GetList()
                .Where(l => l.teacher_id == _teacher_id && l.date >= start && l.date <= end).Select(i => new lessonDTO(i)).ToList();
            var res = result.Where(l => l.status != "Отменено").ToList();
            foreach (var item in res)
            {
                if (cars.FirstOrDefault(c => c.id == item.car_id) != null) item.car_name = cars.FirstOrDefault(c => c.id == item.car_id).full_car_name;
                else item.car_name = "Лекция";
            }
            return res;
        }
        public List<lessonDTO> GetAllTeacherLessons(int id)
        {
            List<lessonDTO> TeacherLesson = db.Lessons.GetList().Where(l => l.teacher_id == id && l.date >= DateTime.Now).Select(l => new lessonDTO(l)).ToList();
            TeacherLesson = TeacherLesson.Where(l => l.status != "Отменено").ToList();
            List<carDTO> TeacherCars = db.Cars.GetList().Where(c => c.teacher_id == id).Select(c =>  new carDTO(c)).ToList();
            List<studentDTO> Students = db.Students.GetList().Select(st => new studentDTO(st)).ToList();
            foreach (var les in TeacherLesson)
            {
                if (les.type_id == 0) { les.lesson_type = "Вождение"; }
                else if (les.type_id == 1) { les.lesson_type = "Лекция"; }
                else if (les.type_id == 2) { les.lesson_type = "Экзамен"; }
                foreach (var car in TeacherCars)
                {
                    if (les.car_id == car.id)
                    {
                        les.car_name = car.full_car_name;
                    }
                }
                foreach (var st in Students)
                {
                    if (st.id == les.student_id)
                    {
                        les.student_name = st.name;
                    }
                }
            }
            return TeacherLesson;

        }

        public bool CancelTeacherTheLesson(int _teacher_id, int _lesson_id)
        {
            //если преподаватель рил есть
            if (db.Teachers.GetItem(_teacher_id) != null)
            {
               
                //если урок рил есть
                if (db.Lessons.GetItem(_lesson_id) != null)
                {
                    //если урок ведёт именно этот преподаватель
                    if (db.Lessons.GetItem(_lesson_id).teacher_id == _teacher_id)
                    {
                        db.Lessons.GetItem(_lesson_id).status = "Отменено";//статус меняем
                        if (db.Lessons.GetItem(_lesson_id).payment_type_id == 0)//0 - оплачено//для оплаченного студенту возвращается академ час
                        {
                            db.Students.GetItem(db.Lessons.GetItem(_lesson_id).student_id).paid_hours += 1;
                        }
                        db.Save();
                        return true;
                    }
                }
            }
            return false;
        }


        #endregion

        //студентское:
        public int GetStudentSpent(int _student_id)
        {
            if (db.Students.GetItem(_student_id) != null)//если студент есть в бд
            {
                int cost = 0;
                List<invite_course> StudentRegistrations = db.Invitations.GetList().Where(i => i.student_id == _student_id).ToList();//нашли его регистрации

                List<course> StudentCourses = new List<course> { };//Список курсов, на которых есть регистрация у студента

                foreach (var item in StudentRegistrations)
                {
                    if (db.Courses.GetList().FirstOrDefault(c => c.id == item.group_id) != null)
                    {
                        StudentCourses.Add(db.Courses.GetList().FirstOrDefault(c => c.id == item.group_id));
                        cost += db.Courses.GetList().FirstOrDefault(c => c.id == item.group_id).cost;
                    }
                }
                return cost;
            }
            return 0;
        }


        public bool CancelTheLesson(int _student_id, int _lesson_id)
        {
            //student? st =//null-ссылочные типы только в c# 8.0+
            //если студент рил существует
            if (db.Students.GetItem(_student_id) != null)
            {
                //если урок рил есть 
                if (db.Lessons.GetItem(_lesson_id) != null)
                {
                    //если это именно вождение type_id = 0 и статус != отменено
                    if (db.Lessons.GetItem(_lesson_id).type_id == 0 && db.Lessons.GetItem(_lesson_id).status != "Отменено")
                    {
                        //отмена записи доступна минимум за два дня до занятия: н-р сегодня 24. Занятие 26. 26 - 24 = 2. можно. 25 - 24 = 1. нельзя
                        if (db.Lessons.GetItem(_lesson_id).date > DateTime.Now)//если это занятие есть в будущем, а не является проведённым
                        {
                            if ((db.Lessons.GetItem(_lesson_id).date - DateTime.Now).TotalDays >= 2)
                            {
                                db.Lessons.GetItem(_lesson_id).status = "Отменено";
                                if (db.Lessons.GetItem(_lesson_id).payment_type_id == 0)//0 - оплачено
                                {
                                    db.Students.GetItem(_student_id).paid_hours += 1;
                                }
                                db.Save();
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        //время работы с 9 до 17:00
        public List<DateTime> GetAvailableHours(int _teacher_id, DateTime _DayMonthYear)
        {
            
            List<DateTime> hours = new List<DateTime>();
            for ( int i = 0; i <= 20; i++)
            {
                if (i == 0 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 9, 0, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 9, 0, 0));
                else if (i == 1
                    && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 9, 30, 0), _teacher_id)
                    ) hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 9, 30, 0));
                else if (i == 2 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 10, 00, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 10, 00, 0));
                else if (i == 3 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 10, 30, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 10, 30, 0));
                else if (i == 4 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 11, 00, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 11, 00, 0));
                else if (i == 5 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 11, 30, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 11, 30, 0));
                else if (i == 6 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 12, 00, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 12, 00, 0));
                else if (i == 7 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 12, 30, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 12, 30, 0));
                else if (i == 8 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 13, 00, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 13, 00, 0));
                else if (i == 9 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 13, 30, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 13, 30, 0));
                else if (i == 10 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 14, 00, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 14, 00, 0));
                else if (i == 11 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 14, 30, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 14, 30, 0));
                else if (i == 12 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 15, 00, 0), _teacher_id)) 
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 15, 00, 0));
                else if (i == 13 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 15, 30, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 15, 30, 0));
                else if (i == 14 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 16, 00, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 16, 00, 0));
                else if (i == 15 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 16, 30, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 16, 30, 0));
                else if (i == 16 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 17, 00, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 17, 00, 0));
                else if (i == 17 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 17, 30, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 17, 30, 0));
                else if (i == 18 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 18, 00, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 18, 00, 0));
                else if (i == 19 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 18, 30, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 18, 30, 0));
                else if (i == 20 && CheckAvailableDate(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 19, 00, 0), _teacher_id))
                    hours.Add(new DateTime(_DayMonthYear.Year, _DayMonthYear.Month, _DayMonthYear.Day, 19, 00, 0));
                //TimeSpan timeSpan = TimeSpan.FromHours(i);
                //hours.Add());
            }
            return hours;
        }

        private bool CheckAvailableDate(DateTime _time, int _teacher_id)
        {
            
            var lessons = db.Lessons.GetList()
                           .Where(l => l.date.Month == _time.Month && l.date.Day == _time.Day && l.date.Year == _time.Year && l.teacher_id == _teacher_id && (l.status == "Назначено" || l.status == "Назначено  " || l.status == "Назначено "))//получаем все занятия в тот же день что и _time
                           .OrderBy(i => i.date);//сортируем список по времени всех занятий по возрастанию

            var LessonsOnTheSameDay = db.Lessons.GetList()
                .Where(l => l.date.Year == _time.Year && l.date.Month == _time.Month && l.date.Day == _time.Day && l.teacher_id == _teacher_id && (l.status == "Назначено" || l.status == "Назначено  " || l.status == "Назначено "))//список занятий в тот же день
                .OrderBy(l => l.date);//сортированнй по возрастанию
            lesson LessonBefore = null, LessonAfter = null;
            if (LessonsOnTheSameDay.LastOrDefault(l => l.date <= _time) != null)
            {
                LessonBefore = LessonsOnTheSameDay.LastOrDefault(l => l.date <= _time);

            };//получаем последний урок, что меньше (по дате) текущей переданной даты для записи
            if (LessonBefore == null)//уроков до нету - сделаем урок с очень "старой" датой
                LessonBefore = new lesson
                {
                    date = new DateTime(1966, 1, 1),
                    car_id = 0,
                    cost = "0",
                    payment_type_id = 0,
                    status = "",
                    student_id = 0,
                    teacher_id = 0,
                    topic = " ",
                    type_id = 0
                };
            if (LessonsOnTheSameDay.FirstOrDefault(l => l.date >= _time) != null)
            {
                LessonAfter = LessonsOnTheSameDay.FirstOrDefault(l => l.date >= _time);
            }
            if (LessonAfter == null)//уроков после нету - сделаем урок с очень "будущей" датой
                LessonAfter = new lesson
                {
                    date = new DateTime(3995, 1, 1),
                    car_id = 0,
                    cost = "0",
                    payment_type_id = 0,
                    status = "",
                    student_id = 0,
                    teacher_id = 0,
                    topic = " ",
                    type_id = 0
                };
            if ((_time - LessonBefore.date).TotalHours > 1 && (_time - LessonBefore.date).TotalMinutes >= 15) //смотрим разницу по времени с занятиемДо
            {
                if ((LessonAfter.date - _time).TotalHours > 1 && (LessonAfter.date - _time).TotalMinutes >= 15)//смотрим разницу по времени с занятиемПосле
                {
                    return true;
                }
            }
            return false;
        }

        public bool Save()
        {
            if (db.Save() > 0) return true;
            return false;
        }
        private IDbRepos db;
        public StudentService(IDbRepos repos)
        {
            db = repos;//new DSModel();
        }

        public List<categoryDTO> GetAllCategories()
        {
            return db.Categories.GetList().Select(c => new categoryDTO(c)).ToList();
        }

        public List<courseDTO> GetAllCourses()
        {
            var list = db.Courses.GetList().Select(i => new courseDTO(i)).ToList();
            foreach (var item in list)
            {
                var teacher =  db.Teachers.GetList().Find(t => t.id == item.teacher_id); //нашли учителя по id
                string cat_name = db.Categories.GetList().Find(c => c.id == item.category_id).name;
                item.teacher_name = teacher.last_name + " " + teacher.first_name + " " + teacher.middle_name;
                item.category_name = cat_name;
                item.category_teacher = "Категория " + cat_name + " " + teacher.last_name + " " + teacher.first_name + " " + teacher.middle_name;
                item.category_fullname = "Категория: " + cat_name;
            }
            return list;
        }

        public List<invite_courseDTO> GetAllInvitations()
        {
            return db.Invitations.GetList().Select(i => new invite_courseDTO(i)).ToList();
        }
        public int GetHowManyLessonNoPay(int _student_id)
        {
            int count = 0;
            List < lesson > StudentLessons = db.Lessons.GetList().Where(l => l.student_id == _student_id).ToList();
            foreach (var item in StudentLessons)
            {
                if (item.payment_type_id == 1 && item.status == "Назначено")
                {
                    count += int.Parse(item.cost);
                }
            }
            return count;
        }
        public List<lessonDTO> GetAllLessons()
        {
            //преобразование из объекта "lesson" в "lessonDTO"
            List<lessonDTO> res = db.Lessons.GetList().Select(i => new lessonDTO(i)).ToList();
            return res;
        }
        public List<carDTO> GetAllCars()
        {
            return db.Cars.GetList().Select(i => new carDTO(i)).ToList();
        }
        public List<lessonDTO> GetAllMyLessons(int id)
        {
            List<lesson_type> types = db.LessonTypes.GetList();//список с категориями занятий: лекция, экзамен, вождение.
            List<teacher> teachers = db.Teachers.GetList();
            List<carDTO> cars = GetAllCars();


            List<lessonDTO> lessons = db.Lessons.GetList().Where(l => l.student_id == id).Select(l => new lessonDTO(l)).ToList();
            foreach (var les in lessons)//по всем урокам
            {
                if (les.type_id == 1) les.car_id = 0;//для лекции car_id = 0 => лекция.
                var cr = db.Cars.GetItem(les.car_id);
                if (cr != null) { les.car_name = (cr.car_type + " " + cr.brand + " " + cr.model); }
                
                foreach (var typ in types) //по всем видам занятий
                {
                    foreach (var item in teachers)//по всем преподавателям
                    {
                        if (les.type_id == typ.id) { les.lesson_type = typ.name; }
                        if (item.id == les.teacher_id) 
                        { 
                            les.teacher_name = item.last_name + " " + item.first_name + " " + item.middle_name; 
                            les.tnumber = item.tnumber;
                        }
                        
                    }
                }
            }
            return lessons;
        }

        public List<studentDTO> GetAllStudents()
        {
            return db.Students.GetList().Select(i => new studentDTO(i)).ToList();
        }

        public List<teacherDTO> GetAllTeachers()
        {
            return db.Teachers.GetList().Select(i => new teacherDTO(i)).ToList();
        }

        public bool RegisterForTheCourse(int _student_id, int course_id)
        {
            //если студент существует с таким id
            if (db.Students.GetList().Find(st => st.id == _student_id) != null)
            {
                //если действительно курс с таким id существует:
                if (db.Courses.GetList().Find(cours => cours.id == course_id) != null)
                {
                    Random r = new Random();
                    //находим все регистрации на курсы у студента
                    var invitations = db.Invitations.GetList().Where(inv => inv.student_id == _student_id).ToList();
                    foreach (var item in invitations)
                    {
                        if (item.group_id == course_id) return false;  //проверяем: если студент уже записан на курс - выход
                    }
                    //не записан -> создать новый объект в invitations
                    db.Invitations.Create(new invite_course() { student_id = _student_id, group_id = course_id });

                    var crs = db.Courses.GetList().Find(c => c.id == course_id);
                    DateTime startDate = crs.start_date;
                    int counter = 1;
                    while (startDate <= crs.end_time)//цикл до тех пор, пока не кончится время курса
                    {
                        if (startDate.DayOfWeek == DayOfWeek.Monday ||
                            startDate.DayOfWeek == DayOfWeek.Wednesday ||
                            startDate.DayOfWeek == DayOfWeek.Friday
                            )
                        {//проверяем понедельник\среду\пятницу
                            
                            db.Lessons.Create(new lesson
                            {
                                date = startDate,
                                topic = "ПДД тема: " + counter.ToString(),
                                type_id = 1, //1 = лекция
                                teacher_id = crs.teacher_id,
                                student_id = _student_id,
                                payment_type_id = 0, //0 = оплачено
                                cost = (crs.cost / Math.Abs(crs.lecture_hours + crs.driving_hours)).ToString(),
                                status = "Назначено",
                            });
                            counter++;
                            //break;
                        }
                        startDate = startDate.AddDays(1);//идём по циклу
                        
                    }

                    db.Students.GetList().Find(st => st.id == _student_id).paid_hours += crs.driving_hours;//добавляем студенты оплаченные часы.
                    Save();
                    return true;
                }
            }
            return false;
        }

        public List<courseDTO> GetStudentCourses(int student_id)
        {
            //если студент существует с таким id
            if (db.Students.GetList().Find(st => st.id == student_id) != null)
            {
                List<course> courses = new List<course>();
                var invitations = db.Invitations.GetList().Where(inv => inv.student_id == student_id);
               
                foreach (var item in invitations)
                {
                    courses.Add(db.Courses.GetList().Find(cours => cours.id == item.group_id)
                        );
                }

                List<courseDTO> res = courses.Select(c => new courseDTO(c)).ToList();

                foreach (var item in res)
                {
                    var teacher = db.Teachers.GetList().Find(t => t.id == item.teacher_id); //нашли учителя по id
                    string cat_name = db.Categories.GetList().Find(c => c.id == item.category_id).name;
                    item.teacher_name = teacher.last_name + " " + teacher.first_name + " " + teacher.middle_name;
                    item.category_name = cat_name;
                    item.category_teacher = "Категория " + cat_name + " " + teacher.last_name + " " + teacher.first_name + " " + teacher.middle_name;
                    item.category_fullname = "Категория: " + cat_name;

                }
                return res;
            }
            else return new List<courseDTO>();
        }

        public List<courseDTO> GetAvailableCourses(int student_id)
        {
            //если студент существует с таким id
            if (db.Students.GetList().Find(st => st.id == student_id) != null)
            {

                List<course> courses = new List<course>();
                var invitations = db.Invitations.GetList().Where(inv => inv.student_id == student_id);

                foreach (var item in invitations)
                {
                    courses.Add(db.Courses.GetList().Find(cours => cours.id == item.group_id)
                        );
                }
                var list = db.Courses.GetList().Except(courses).ToList();
                List<courseDTO> res = list.Select(c => new courseDTO(c)).ToList();
                foreach (var item in res)
                {
                    var teacher = db.Teachers.GetList().Find(t => t.id == item.teacher_id); //нашли учителя по id
                    string cat_name = db.Categories.GetList().Find(c => c.id == item.category_id).name;
                    item.teacher_name = teacher.last_name + " " + teacher.first_name + " " + teacher.middle_name;
                    item.category_name = cat_name;
                    item.category_teacher = "Категория " + cat_name + " " + teacher.last_name + " " + teacher.first_name + " " + teacher.middle_name;
                    item.category_fullname = "Категория: " + cat_name;

                }
                return res;

                //List<courseDTO> stcrs = courses.Select(c => new courseDTO(c)).ToList();


                //List<courseDTO> allcrs = db.Courses.GetList().Select(i => new courseDTO(i)).ToList(); //нашли все курсы
                //allcrs.RemoveAll(stcrs.Contains);//удалили все курсы студента изи всех курсов = все ещё не записанные курсы
                //foreach (var item in allcrs)
                //{
                //    var teacher = db.Teachers.GetList().Find(t => t.id == item.teacher_id); //нашли учителя по id
                //    string cat_name = db.Categories.GetList().Find(c => c.id == item.category_id).name;
                //    item.teacher_name = teacher.last_name + " " + teacher.first_name + " " + teacher.middle_name;
                //    item.category_name = cat_name;
                //}
                //return allcrs;
            }
            else return new List<courseDTO>();
        }

        public int GetStudentPaidHours(int student_id)
        {
            if (db.Students.GetItem(student_id) != null)
                return db.Students.GetItem(student_id).paid_hours;
            else return 0;
        }

        public bool RegisterForTheLesson(int _student_id, int _teacher_id, int _car_id, DateTime _time, bool _flag)//flag - флаг оплаты занятия на основе уже оплаченных часов при записи на курс.
        {
            if (_time < DateTime.Now) return false;
            //если студент действительно есть в автошколе
            if (db.Students.GetItem(_student_id) != null)
            {
                var Student = db.Students.GetItem(_student_id);
                //если преподаватель действительно существует в автошколе
                if (db.Teachers.GetItem(_teacher_id) != null)
                {
                    //если авто действительно существует в автошколе
                    if (db.Cars.GetItem(_car_id) != null)
                    {
                        //проверяем нет ли у учителя занятости в это время: последний урок перед переданным временем должен иметь + 1ч (сам академ час занятия) и + 15 мин отдыха
                        var lessons = db.Lessons.GetList()
                            .Where(l => l.date.Month == _time.Month && l.date.Day == _time.Day && l.date.Year == _time.Year && l.teacher_id == _teacher_id && l.status == "Назначено")//получаем все занятия в тот же день что и _time
                            .OrderBy(i => i.date);//сортируем список по времени всех занятий по возрастанию

                        var LessonsOnTheSameDay = db.Lessons.GetList() 
                            .Where(l => l.date.Year == _time.Year && l.date.Month == _time.Month && l.date.Day == _time.Day && l.teacher_id == _teacher_id && l.status == "Назначено")//список занятий в тот же день
                            .OrderBy(l => l.date);//сортированнй по возрастанию
                        lesson LessonBefore = null, LessonAfter = null;
                        //получаем первый урок, что больше (по дате) текущей переданной даты для записи
                        //если нулл => уроков на тот же день нет - свободно записываемся.
                        //не нул - смотрим есть ли разница в 1 ч 15 мин

                        if (LessonsOnTheSameDay.LastOrDefault(l => l.date <= _time) != null)
                        {
                            LessonBefore = LessonsOnTheSameDay.LastOrDefault(l => l.date <= _time);

                        };//получаем последний урок, что меньше (по дате) текущей переданной даты для записи
                        if (LessonBefore == null)//уроков до нету - сделаем урок с очень "старой" датой
                            LessonBefore = new lesson
                            {
                                date = new DateTime(1966, 1, 1),
                                car_id = 0,
                                cost = "0",
                                payment_type_id = 0,
                                status = "",
                                student_id = 0,
                                teacher_id = 0,
                                topic = " ",
                                type_id = 0
                            };
                       if(LessonsOnTheSameDay.FirstOrDefault(l => l.date >= _time) != null)
                        {
                            LessonAfter = LessonsOnTheSameDay.FirstOrDefault(l => l.date >= _time);
                        }
                        if (LessonAfter == null)//уроков после нету - сделаем урок с очень "будущей" датой
                            LessonAfter = new lesson { date = new DateTime(3995, 1, 1), car_id = 0, cost = "0", payment_type_id = 0, status = "", student_id = 0,
                                teacher_id = 0, topic = " ", type_id = 0
                            };

                        //if (LessonBefore != null)//если есть занятие до
                        //{
                        //    if (LessonAfter != null)//если есть занятие после
                        //    {
                        if ((_time - LessonBefore.date).TotalHours > 1 && (_time - LessonBefore.date).TotalMinutes > 15) //смотрим разницу по времени с занятиемДо
                        {
                            if ((LessonAfter.date - _time).TotalHours > 1 && (LessonAfter.date - _time).TotalMinutes > 15)//смотрим разницу по времени с занятиемПосле
                            {
                                if (_flag)//если юзер хочет за счёт оплаченных занятий записаться
                                {
                                    if (db.Students.GetItem(_student_id).paid_hours > 1)
                                    {
                                        db.Lessons.Create(new lesson
                                        {
                                            car_id = _car_id,
                                            cost = 1500.ToString(),
                                            date = _time,
                                            status = "Назначено",
                                            student_id = _student_id,
                                            payment_type_id = 0,//0 - оплачено
                                            teacher_id = _teacher_id,
                                            topic = "Вождение",
                                            type_id = 0
                                        });
                                        db.Students.GetItem(_student_id).paid_hours -= 1;
                                    }
                                    else
                                    {
                                        db.Lessons.Create(new lesson
                                        {
                                            car_id = _car_id,
                                            cost = 1500.ToString(),
                                            date = _time,
                                            status = "Назначено",
                                            student_id = _student_id,
                                            payment_type_id = 0,//1 - Неоплачено
                                            teacher_id = _teacher_id,
                                            topic = "Вождение",
                                            type_id = 0
                                        });
                                    }
                                    
                                    Save();
                                    return true;
                                }
                                else//если юзер не выбрал оплату за счёт оплаченных
                                {
                                    db.Lessons.Create(new lesson
                                    {
                                        car_id = _car_id,
                                        cost = 1500.ToString(),
                                        date = _time,
                                        status = "Назначено",
                                        student_id = _student_id,
                                        payment_type_id = 1,//1 - Неоплачено
                                        teacher_id = _teacher_id,
                                        topic = "Вождение",
                                        type_id = 0
                                    });
                                    Save();
                                    return true;
                                }
                            }
                        }
                        return false;
                        //    }
                        //}
                        

                        ////DateTime min_time = lessons.FirstOrDefault().date;//минимальаня дата
                        //var lessonBefore = lessons.Where(ls => ls.date.AddHours(-1).AddMinutes(-15) < _time).OrderBy(i => i.date).Last(); //найдём самый близкий урок До
                        //var lessonAfter = lessons.Where(ls => ls.date > _time).First();
                        //if (lessonBefore != null) //если урок есть (время занято)
                        //{
                        //    //создаём 
                        //    //if (lessonBefore.date.AddHours(1).AddMinutes(15) < _time)//если там есть 1ч15мин, то урок создадим. Иначе - return false
                        //    //{

                            //    //}
                            //}
                            //DateTime timeBefore = lessons.Where(t => t.date < _time).FirstOrDefault().date;//

                            //var lessonsAfter = lessons.Where(ls => ls.date.AddHours(1).AddMinutes(15) )
                       
                        ////узнать нет ли урока в это время: найти урок, который меньше переданной даты на 1 ча
                        //for (global::System.Int32 i = 0; i < lessons.Count(); i++)
                        //{
                        //    if (lessons[i])
                        //}

                        //foreach (var item in lessons)
                        //{
                        //    if (item.date.AddMinutes(15) < min_time)
                        //    {

                        //    }
                        //}
                    }
                }
            }
            return false;
        }

        //bool RegisterForTheLesson(int _student_id, int _teacher_id, int car_id, DateTime _time)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<BookDTO> GetAllBooks()
        //{
        //    return db.Books.GetList().Select(i => new BookDTO(i)).ToList();
        //}
        ////public class SubReaderBookGenre
        ////{
        ////    public SubReaderBookGenre() { }
        ////    public string Name { get; set; }
        ////    public string BookName { get; set; }
        ////    public string BookGenre { get; set; }
        ////    public DateTime? Data { get; set; }
        ////}
        //public List<SubReaderBookGenre> GetAllSubscriptions()
        //{
        //    var res = db.Subscriptions.GetList()
        //        .Join(db.Readers.GetList(), s => s.Reader_ID, r => r.ID, (s, r) => new { r, s })
        //        .Join(db.Books.GetList(), sub => sub.s.Book_ID, b => b.ID, (sub, b) => new { sub, b })
        //        .Join(db.Genres.GetList(), bbk => bbk.b.Genre_ID, g => g.ID, (bbk, g) => new { bbk, g })
        //        .Select(n => new SubReaderBookGenre
        //        {
        //            Name = n.bbk.sub.r.Name,
        //            Data = n.bbk.sub.s.Data,
        //            BookName = n.bbk.b.Name,
        //            BookGenre = n.g.Name
        //        });
        //    return res.ToList();
        //    // .Join(db.Book, bk => bk.s.Book_ID, b => b.ID, (bk, b) => new { bk, b })
        //}

        ////public class BookGenreNoID
        ////{
        ////    public BookGenreNoID() { }
        ////    public string Name { get; set; }
        ////    public string Genre { get; set; }
        ////    public int Amount { get; set; }
        ////}

        //public List<BookGenreNoID> GetAllAvailableBooksForGrid()
        //{
        //    return db.Books.GetList().Join(db.Genres.GetList(), b => b.Genre_ID, g => g.ID, (b, g) => new { b, g })
        //        .Where(bk => bk.b.Amount > 0)
        //        .Select(bk => new BookGenreNoID
        //        {
        //            Name = bk.b.Name,
        //            Genre = bk.g.Name,
        //            Amount = bk.b.Amount
        //        }).ToList();
        //    //.ToList().Select(i => new BookDTO(i)).Where(b => b.Amount > 0).ToList();
        //}
        //public List<BookDTO> GetAllAvailableBooks()
        //{
        //    return db.Books.GetList().Select(i => new BookDTO(i)).Where(b => b.Amount > 0).ToList();
        //}

        //public ReaderDTO GetReader(int id)
        //{
        //    //Reader r = db.Reader.Where(i => i.ID == id).FirstOrDefault();

        //    //return new ReaderDTO(r);
        //    return new ReaderDTO(db.Readers.GetItem(id));
        //}

        ////public BookDTO GetBook(int id)
        ////{
        ////    Book b = db.Book.Where(i => i.ID == id).FirstOrDefault();

        ////    return new BookDTO(b);
        ////}

        //public BookDTO GetBook(int Id)
        //{
        //    return new BookDTO(db.Books.GetItem(Id));
        //}
        //public void CreateBook(BookDTO b)
        //{
        //    db.Books.Create(new Book() { Author = b.Author, Genre_ID = b.Genre_ID, Name = b.Name, Amount = b.Amount, Library_ID = b.Library_ID });
        //    Save();
        //    //db.Phones.Attach(p);
        //}
        //public void UpdateBook(BookDTO p)
        //{
        //    Book b = db.Books.GetItem(p.ID);
        //    b.Amount = p.Amount;
        //    b.Name = p.Name;
        //    b.Genre_ID = p.Genre_ID;
        //    b.Author = p.Author;
        //    //ph.ManufacturerId = p.ManufacturerId;
        //    Save();
        //}
        //public void DeleteBook(int id)
        //{
        //    Book b = db.Books.GetItem(id);
        //    if (b != null)
        //    {
        //        db.Books.Delete(b.ID);
        //        Save();
        //    }
        //}
        //public bool Save()
        //{
        //    if (db.Save() > 0) return true;
        //    return false;
        //}

        //public List<GenreDTO> GetAllGenres()
        //{
        //    return db.Genres.GetList().Select(i => new GenreDTO(i)).ToList();
        //}

        //public List<LibraryDTO> GetAllLibraries()
        //{
        //    return db.Libraries.GetList().Select(i => new LibraryDTO(i)).ToList();
        //}
        //public List<ReaderDTO> GetAllReaders()
        //{
        //    return db.Readers.GetList().Select(i => new ReaderDTO(i)).ToList();
        //}

    }
}
