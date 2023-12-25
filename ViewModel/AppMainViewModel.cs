using BLL.Services;
using Interfaces.DTO;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Autoschool.ViewModel
{
    public class AppMainViewModel : BaseViewModel
    {
        #region Администратор

        private DateTime _AdminFirstTime {  get; set; }
        public DateTime AdminFirstTime
        {
            get
            {
                return _AdminFirstTime;
            }
            set
            {
                _AdminFirstTime = value; OnPropertyChanged();
            }
        }

        private DateTime _AdminSecondTime { get; set; }
        public DateTime AdminSecondTime
        {
            get
            {
                return _AdminSecondTime;
            }
            set
            {
                _AdminSecondTime = value; OnPropertyChanged();
            }
        }


        private RelayCommand admin_reportCommand;
        public RelayCommand Admin_reportCommand
        {
            get
            {
                return admin_reportCommand ??
                  (admin_reportCommand = new RelayCommand(obj =>
                  {
                      if (AdminFirstTime  != null && AdminSecondTime != null) 
                      {
                          admin_report_courses = studentService.GetAllCoursesForAdmin(AdminFirstTime, AdminSecondTime);
                          admin_sold_courses = 0;
                          admin_total_lessons_for_period = studentService.GetAmountLessonsForPeriod(AdminFirstTime, AdminSecondTime);//получили список всех занятий за период
                          admin_lessons_counter = admin_total_lessons_for_period.Count();//Всего занятий проведено
                          admin_lessons_cost = 0;//Общая выручка автошколы
                          admin_lecture_counter = 0;//Количество лекций
                          money_to_teachers = 0;//Выручка преподавателей:
                          admin_drive_counter = 0;//Количество занятий вождения
                          admin_students_counter = 0;//всего студентов прошедших хотя бы одно занятие в автошколе

                          List<int> ListOfIdStudents = new List<int>();//лист с ID студентов, записанных на занятия
                          foreach (var item in admin_total_lessons_for_period)
                          {
                              if (ListOfIdStudents.Contains(item.student_id)== false)//если такой ID ещё не встречался в листе
                              {
                                  ListOfIdStudents.Add(item.student_id);//добавляем его
                              }
                          }

                          foreach (var i in admin_report_courses)
                          {
                              if (i.registered_people > 0) admin_sold_courses += i.registered_people;//если есть зарегестрированные люди => плюсуем кол-во проданных курсов
                          }
                          foreach (var les in admin_total_lessons_for_period)
                          {
                              if (les.type_id == 1)//лекция: половина от неё идёт автошколе
                              {
                                  admin_lessons_cost += (int.Parse(les.cost) / 2);
                                  money_to_teachers += (int.Parse(les.cost) / 2);
                                  admin_lecture_counter += 1;//Количество лекций += 1
                              }
                              else if (les.type_id == 0)//вождение - вся выручка преподавателю
                              {
                                  money_to_teachers += int.Parse(les.cost) - 52 * 15; //-52 * 15 === 52 рубля Аи-95 на 15 км.//средний расход бензина по статистике
                                  admin_drive_counter += 1;
                              }
                          }
                          admin_students_counter = ListOfIdStudents.Count();

                          ////if (admin_report_courses.Count == 0) MessageBox.Show("За выбранный период нет статистики по курсам!");
                          ////else MessageBox.Show("Загрузка данных успешна!");
                      }
                      else { MessageBox.Show("Некорректно выбран период для статистики!"); }
                  }));
            }
        }

        private List<courseDTO> _admin_report_courses {  get; set; }
        public List<courseDTO> admin_report_courses
        {
            get { return _admin_report_courses; }
            set { _admin_report_courses = value; OnPropertyChanged(); }
        }
        private List<lessonDTO> _admin_total_lessons_for_period {  get; set; }
        public List<lessonDTO> admin_total_lessons_for_period
        {
            get { return _admin_total_lessons_for_period; }
            set { _admin_total_lessons_for_period = value; OnPropertyChanged(); }
        }

        private int _admin_lessons_counter {  get; set; }
        public int admin_lessons_counter
        {
            get { return _admin_lessons_counter; }
            set { _admin_lessons_counter = value; OnPropertyChanged(); }
        }
        private int _admin_sold_courses {  get; set; }
        public int admin_sold_courses
        {
            get { return _admin_sold_courses; }
            set { _admin_sold_courses = value; OnPropertyChanged(); }
        }
        private int _admin_students_counter {  get; set; }
        public int admin_students_counter
        {
            get { return _admin_students_counter; }
            set { _admin_students_counter = value; OnPropertyChanged(); }
        }
        private double _admin_lessons_cost {  get; set; }
        public double admin_lessons_cost
        {
            get { return _admin_lessons_cost; }
            set { _admin_lessons_cost = value; OnPropertyChanged(); }
        }
        private double _money_to_teachers {  get; set; }
        public double money_to_teachers
        {
            get { return _money_to_teachers; }
            set { _money_to_teachers = value; OnPropertyChanged(); }
        }
        private double _admin_lecture_counter {  get; set; }
        public double admin_lecture_counter
        {
            get { return _admin_lecture_counter; }
            set { _admin_lecture_counter = value; OnPropertyChanged(); }
        }
        private double _admin_drive_counter {  get; set; }
        public double admin_drive_counter
        {
            get { return _admin_drive_counter; }
            set { _admin_drive_counter = value; OnPropertyChanged(); }
        }




        #endregion



        #region Студент
        //CultureInfo ci = new CultureInfo("ru-RU");

        private RelayCommand registerForTheCourseCommand;
        public RelayCommand RegisterForTheCourseCommand
        {
            get
            {
                return registerForTheCourseCommand ??
                  (registerForTheCourseCommand = new RelayCommand(obj =>
                  {
                      if (selected_courseDataGrid != null && selected_student != null)
                      {
                          if (studentService.RegisterForTheCourse(selected_student.id, selected_courseDataGrid.id))
                          {
                              selected_courseDataGrid = null; //убрали детали выбранног курса
                              student_invitations = studentService.GetAllInvitations().Where(i => i.student_id == _selected_student.id).ToList();
                              student_courses = studentService.GetStudentCourses(selected_student.id);
                              courses = studentService.GetAvailableCourses(selected_student.id);
                              student_lessons = studentService.GetAllMyLessons(selected_student.id);
                              student_paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                              selected_student.paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                              HowManyMoneyStudentSpent = studentService.GetStudentSpent(selected_student.id);
                              authorized_teacher = AllTeachers[0];
                              MessageBox.Show("Запись на курс успешна!");
                          }
                          else MessageBox.Show("Ошибка записи на курс");
                      }
                  }));
            }
        }
        private RelayCommand cancelTheCourseCommand;
        public RelayCommand CancelTheLessonCommand
        {
            get
            {
                return cancelTheCourseCommand ??
                  (cancelTheCourseCommand = new RelayCommand(obj =>
                  {
                      if (selected_student_lessons.status == "Назначено")
                      {
                          if (studentService.CancelTheLesson(selected_student.id, selected_student_lessons.id))
                          {
                              MessageBox.Show("Отмена успешна");
                              student_paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                              student_lessons = studentService.GetAllMyLessons(selected_student.id);
                              times = studentService.GetAvailableHours(selected_student.id, selected_monthday);
                              //times = studentService.GetAvailableHours(selected_teacher.id, selected_monthday);
                              if (times.Count == 0) selected_hour = DateTime.Now;
                              else if (times.Count > 0) selected_hour = times[0];


                              student_paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                              selected_student.paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                              HowManyLessonNoPay = studentService.GetHowManyLessonNoPay(selected_student.id);//обновляем Label с числом денег на неоплаченные занятия
                              lessonSignUp = false;
                              authorized_teacher = AllTeachers[0];

                          }
                          else MessageBox.Show("Отмена занятия не удалась. Отмена доступна минимум за два дня до назначенного занятия вождения!");
                      }
                      else MessageBox.Show("Занятие уже отменено!");
                  }));
            }
        }

        private RelayCommand registerForTheLessonCommand;
        public RelayCommand RegisterForTheLessonCommand
        {
            get
            {
                return registerForTheLessonCommand ??
                  (registerForTheLessonCommand = new RelayCommand(obj =>
                  {
                      if (new DateTime(selected_monthday.Year, selected_monthday.Month, selected_monthday.Day, selected_hour.Hour, selected_hour.Minute, 00) < DateTime.Now)
                      {
                          MessageBox.Show("Выберите корректную дату для записи. Вы хотите записаться на занятие в прошлом?");
                      }

                      if (selected_teacher != null && selected_monthday != null && selected_hour != null && selected_car != null)
                      {
                          if (selected_student.paid_hours <= 0 && selected_paid_hours_flag == true)
                          {
                              MessageBox.Show("Оплаченные занятия кончились! Оплата возможна лично инструктору в день занятия.");
                          }
                          else
                          {
                              DateTime selected_time = new DateTime(selected_monthday.Year, selected_monthday.Month, selected_monthday.Day, selected_hour.Hour, selected_hour.Minute, 00);

                              if (selected_time.Hour >= 9 && selected_time.Hour <= 20)
                              {
                                  if (studentService.RegisterForTheLesson(selected_student.id, selected_teacher.id, selected_car.id, selected_time, selected_paid_hours_flag) == true)
                                  {

                                      MessageBox.Show("Запись успешна!");
                                      student_lessons = studentService.GetAllMyLessons(selected_student.id);
                                      selected_student_lessons = null;
                                      
                                      times = studentService.GetAvailableHours(selected_teacher.id, selected_monthday);
                                      if (times.Count == 0) selected_hour = DateTime.Now;
                                      else if (times.Count > 0) selected_hour = times[0];
                                      student_paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                                      selected_student.paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                                      HowManyLessonNoPay = studentService.GetHowManyLessonNoPay(selected_student.id);//обновляем Label с числом денег на неоплаченные занятия
                                      authorized_teacher = AllTeachers[0];

                                  }

                                  else MessageBox.Show("Произошла ошибка");
                              }
                              else MessageBox.Show("Укажите время в период с 9:00 до 19:00!");
                          }
                          //studentService.RegisterForTheCourse(selected_student.id, selected_courseDataGrid.id);
                          //selected_courseDataGrid = null; //убрали детали выбранног курса
                          //student_invitations = studentService.GetAllInvitations().Where(i => i.student_id == _selected_student.id).ToList();
                          //student_courses = studentService.GetStudentCourses(selected_student.id);
                          //courses = studentService.GetAvailableCourses(selected_student.id);
                          //student_lessons = studentService.GetAllMyLessons(selected_student.id);
                          //student_paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                      }
                      else if (selected_teacher == null)
                      {
                          MessageBox.Show("Пожалуйста, укажите преподавателя, к которому хотите записаться на занятие");
                      }
                      else if (selected_monthday == null)
                      {
                          MessageBox.Show("Пожалуйста, укажите дату занятия");
                      }
                      else if (selected_hour == null)
                      {
                          MessageBox.Show("Пожалуйста, укажите время занятия");
                      }
                      else if (selected_car == null)
                      {
                          MessageBox.Show("Пожалуйста, укажите ТС для занятия");
                      }
                  }));
            }
        }

        private bool _lessonSignUp {  get; set; }//флаг недоступности кнопки
        public bool lessonSignUp
        {
            get { return _lessonSignUp; } set { _lessonSignUp = value; OnPropertyChanged(); }
        }
        private int _howManyLessonNoPay {  get; set; }//флаг недоступности кнопки
        public int HowManyLessonNoPay
        {
            get { return _howManyLessonNoPay; } set { _howManyLessonNoPay = value; OnPropertyChanged(); }
        }
        private List<carDTO> _selected_teacher_cars {  get; set; }//список машин конкретного преподавателя из комбобокса
        public List<carDTO> selected_teacher_cars
        {
            get { return _selected_teacher_cars; } set { _selected_teacher_cars = value; OnPropertyChanged(); }
        }
        
        private carDTO _selected_car{  get; set; }//конкретный автомобиль, выбранный студентом для записи на занятие
        public carDTO selected_car
        {
            get { return _selected_car; } set { _selected_car = value; OnPropertyChanged(); }
        }
        


        private List<teacherDTO> _teachers {  get; set; }

        public List<teacherDTO> teachers //список всех преподавателей для combobox записи на занятие
        {
            get
            {
                return _teachers;
            }
            set {  _teachers = value; OnPropertyChanged();}
        }
        private teacherDTO _selected_teacher {  get; set; }

        public teacherDTO selected_teacher //выбранный преподаватель в комбобоксе записи на занятие
        {
            get
            {
                return _selected_teacher;
            }
            set { _selected_teacher = value;
                OnPropertyChanged();
                selected_teacher_cars = AllCars.Where(cr => cr.teacher_id == value.id).ToList();
                if (selected_monthday != null)
                {
                    times = studentService.GetAvailableHours(selected_teacher.id, selected_monthday);
                }
            }
        }

        private lessonDTO _selected_student_lessons {  get; set; }

        public lessonDTO selected_student_lessons
        {
            get { return _selected_student_lessons; }
            set
            {
                _selected_student_lessons = value;
                if (value != null)
                {
                    if (value.type_id == 0)
                    {
                        lessonSignUp = true;
                    }
                    else
                    {
                        lessonSignUp = false;
                    }
                }
                OnPropertyChanged();
            }
        }
        
        private bool _selected_paid_hours_flag {  get; set; } //флаг в селекторе "за счёт оплаченных занятий"

        public bool selected_paid_hours_flag
        {
            get
            {
                return _selected_paid_hours_flag;
            }
            set
            {
                _selected_paid_hours_flag = value;
                OnPropertyChanged();
            }
        }

        private int _student_paid_hours {  get; set; }

        public int student_paid_hours
        {
            get { return _student_paid_hours; }
            set { _student_paid_hours = value; OnPropertyChanged(); }
        }


        private List<lessonDTO> _student_lessons;
        public List<lessonDTO> student_lessons
        {
            get { return _student_lessons; }
            set
            {
                _student_lessons = value; OnPropertyChanged();
            }
        }

        IStudentService studentService;
        public List<studentDTO> students { get; set; }

        private studentDTO _selected_student { get; set; }
        private List<invite_courseDTO> _student_invitations { get; set; }
        private List<courseDTO> _student_courses {  get; set; }
        public List<courseDTO> student_courses //курсы в разделе профиля о каждом студенте
        { 
            get
            {
                return _student_courses;
            }
            set
            {
                _student_courses = value;
                OnPropertyChanged();
            }
        }
    public List<invite_courseDTO> student_invitations 
        { 
            get
            {
                return _student_invitations;
            }
            set
            { 
                _student_invitations = value;
                OnPropertyChanged();
            }
        } //список ID'шников курсов, на кот записан студент
        public studentDTO selected_student {
            get
            {
                return _selected_student;
            }
            set
            {
                _selected_student = value; 
                OnPropertyChanged();
                student_invitations = studentService.GetAllInvitations().Where(i => i.student_id == _selected_student.id).ToList();
                student_courses = studentService.GetStudentCourses(selected_student.id);
                courses = studentService.GetAvailableCourses(selected_student.id);
                student_lessons = studentService.GetAllMyLessons(selected_student.id);
                student_paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                selected_paid_hours_flag = false;
                HowManyMoneyStudentSpent = studentService.GetStudentSpent(selected_student.id);
                HowManyLessonNoPay = studentService.GetHowManyLessonNoPay(selected_student.id);//обновляем Label с числом денег на неоплаченные занятия
            }
        }


        //        private List<categoryDTO> _categories { get; set; }
        //        public List<categoryDTO> categories {
        //            get
        //            {
        //                return _categories;
        //            }
        //            set
        //            {
        //                _categories = value;
        //                OnPropertyChanged();
        //            }                
        //}
        //private categoryDTO _selected_category;
        //public categoryDTO selected_category //Комбобох 1
        //{
        //    get
        //    {
        //        return _selected_category;
        //    }
        //    set
        //    {
        //        _selected_category = value;


        //        // var registers = studentService.GetAllInvitations()
        //        //     .Where(r=> r.student_id == selected_student.id)
        //        //     .ToList(); //регистрации на курсы
        //        // List<courseDTO> CoursesList = new List<courseDTO>(); 
        //        // foreach (var item in registers) //заполняем список курсов за вычетом тех, на которые уже записаны
        //        // {
        //        //     CoursesList.Add(studentService.GetAllCourses().Find(c => c.id != item.group_id));
        //        // }
        //        ////  = studentService.GetAllCourses().Where(c => c.id != ).ToList(); //все курсы
        //        // times = new List<DateTime>();

        //        // times = CoursesList.Where(c => c.category_id == selected_category.id)
        //        //     .Select(o => o.start_date)
        //        //     .ToList();



        //        // OnPropertyChanged("times");
        //        // selected_time = new DateTime();
        //        // OnPropertyChanged("selected_time");
        //        //courses = studentService.GetAllCourses().
        //        //        Where(c => c.category_id == selected_category.id). //сделать вывод курсов, на которые ещё не записаны!
        //        //        ToList();
        //        courses = studentService.GetAllCourses();
        //        //courses.RemoveAll(x => student_courses.Where(st => st.id == x.id));

        //        foreach (var item in student_courses)
        //        {
        //            courses.RemoveAll(el => el.id == item.id);
        //        }
        //        courses.RemoveAll(el => el.category_id != selected_category.id);
        //        //foreach (var item in student_invitations)
        //        //{
        //        //    var crs = (studentService.GetAllCourses().Find(c => c.id != item.group_id && c.category_id == selected_category.id));
        //        //    if  (crs != null)
        //        //    {
        //        //        courses.Add(crs);
        //        //    }
        //        //}
        //        OnPropertyChanged("courses");
        //        //selected_time = times[0];
        //        //OnPropertyChanged("selected_time");
        //    }
        //}

        private courseDTO _selected_student_course { get; set; }
        public courseDTO selected_student_course
        {
            get { return _selected_student_course; }
            set { _selected_student_course = value;OnPropertyChanged(); }
        }
        private List<courseDTO> _courses { get; set; } //Для грида в разделе записи на курсы

        public List<courseDTO> courses {
            get
            {
                return _courses;
            }
            set
            {
                _courses = value;
                OnPropertyChanged();
            }
        } //Для грида в разделе записи на курсы


        //public List<DateTime> times { get; set; } //список доступных дат для записи
        //private DateTime _selected_time {  get; set; }
        //public DateTime selected_time 
        //{ 
        //    get
        //    {
        //        return _selected_time;
        //    }
        //    set
        //    {
        //        _selected_time = value;
        //        //var registers = studentService.GetAllInvitations().ToList();
        //        courses = new List<courseDTO>();
        //        foreach (var item in student_invitations)
        //        {//добавление в список курсов для отображения в DataGrid 1-го раздела без учёта уже купленных
        //            var crs = (studentService.GetAllCourses().Find(c => c.category_id == selected_category.id && c.start_date == _selected_time
        //        && c.id != item.group_id));
        //            if (crs != null)
        //            {
        //                courses.Add(crs);
        //            }
        //        }
        //        OnPropertyChanged("courses");
        //    }

        //} //выбранное время в комбобохе

        private courseDTO _selected_courseDataGrid { get; set; } //для грида

        public courseDTO selected_courseDataGrid //свойство для DataGrid!!!
        {
            //Надо уведомлять об изменении свойства а не поля
            get
            {
                return _selected_course;
            }
            set
            {
                _selected_course = value;
                OnPropertyChanged();
            }
        }
        private courseDTO _selected_course { get; set; } //для грида

        public courseDTO selected_course//свойство для ComboBox!!!!!!!!!!
        {
            //Надо уведомлять об изменении свойства а не поля
            get
            {
                return _selected_course;
            }
            set
            {
                _selected_course = value;
                OnPropertyChanged();
            }
        }


        private DateTime _NowDate { get; set; }
        public DateTime NowDate
        {
            get
            {
                return _NowDate;
            }
            set { _NowDate = value; OnPropertyChanged(); }
        }

        private DateTime _selected_hour {  get; set; }//время часа выбранного в комбобохе
        public DateTime selected_hour
        {
            get
            {
                return _selected_hour; }
            set
            {
                _selected_hour = value; OnPropertyChanged();
            }

        }

        private List<DateTime> _times {  get; set; }
        public List<DateTime> times
        {
            get { return _times; }
            set { _times = value; OnPropertyChanged(); }
        }

        private DateTime _selected_monthday {  get; set; }//время даты месяца с годом выбранного в ДатаПикере
        public DateTime selected_monthday
        {
            get
            {
                return _selected_monthday; }
            set
            {
                _selected_monthday = value; OnPropertyChanged();
                if (_selected_teacher != null)
                {
                    times = studentService.GetAvailableHours(_selected_teacher.id, new DateTime(value.Year, value.Month, value.Day));
                }
            }

        }

        private List<carDTO> _AllCars {  get; set; }
        public List<carDTO> AllCars
        {
            get { return _AllCars; }
            set { _AllCars = value; OnPropertyChanged(); }
        }

        private int _HowManyMoneyStudentSpent {  get; set; }
        public int HowManyMoneyStudentSpent
        {
            get { return _HowManyMoneyStudentSpent;   }
            set { _HowManyMoneyStudentSpent = value; OnPropertyChanged(); }
        }
        #endregion
        #region Преподаватель
        #region Комманды
        private RelayCommand cancelTeacherTheCourseCommand;
        public RelayCommand CancelTeacherTheLessonCommand
        {
            get
            {
                return cancelTeacherTheCourseCommand ??
                  (cancelTeacherTheCourseCommand = new RelayCommand(obj =>
                  {
                      if (studentService.CancelTeacherTheLesson(authorized_teacher.id, _selected_teacher_lessons.id))
                      {
                          MessageBox.Show("Отмена успешна");
                          teachers_lessons = studentService.GetAllTeacherLessons(authorized_teacher.id);
                          TeacherlessonSignUp = false;
                          selected_student = students[0];
                          //student_paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                          //student_lessons = studentService.GetAllMyLessons(selected_student.id);
                          //times = studentService.GetAvailableHours(selected_student.id, selected_monthday);
                          //student_paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                          //selected_student.paid_hours = studentService.GetStudentPaidHours(selected_student.id);
                          //HowManyLessonNoPay = studentService.GetHowManyLessonNoPay(selected_student.id);//обновляем Label с числом денег на неоплаченные занятия

                      }
                      else MessageBox.Show("Отмена занятия не удалась.");
                  }));
            }
        }
        
        private RelayCommand teacher_reportCommand;
        public RelayCommand Teacher_reportCommand
        {
            get
            {
                return teacher_reportCommand ??
                  (teacher_reportCommand = new RelayCommand(obj =>
                  {
                      if (teacher_first_selected_date != null && teacher_second_selected_date != null)
                      {
                          teacher_report_lessons = studentService.GetTeacherReportLessons(authorized_teacher.id, teacher_first_selected_date, teacher_second_selected_date);
                          if (teacher_report_lessons != null) {
                              MoneyToSchool = 0;//выручка автошколы от занятий (преимущественное лекционных)
                              lessonsCost = 0;//общая выручка
                              lessonsCostBenz = 0;
                              lessonsLectureCounter = 0;
                              List<DateTime> TeachLectures = new List<DateTime>();
                              lessonsDrivingCounter = 0;
                              lessonsStudentsCounter = 0;
                              List<int> StudentsID = new List<int>();
                              foreach (var item in teacher_report_lessons)
                              {
                                  
                                  if (item.type_id == 1) //если лекция
                                  {
                                      lessonsCost += (int.Parse(item.cost) /2);//половина от занятия преподавателю.
                                      lessonsCostBenz += (int.Parse(item.cost) / 2);//половина автошколе
                                      MoneyToSchool += (int.Parse(item.cost) - int.Parse(item.cost) / 2);
                                      if (TeachLectures.Contains(item.date) == false)//если лекции с такой датой ещё нету в листе - добавляем её и +1 к лекциям
                                      {
                                          TeachLectures.Add(item.date);
                                          lessonsLectureCounter += 1;//лекции
                                      }
                                      
                                  }
                                  else if (item.type_id == 0)
                                  {
                                      lessonsCost += int.Parse(item.cost);
                                      lessonsDrivingCounter += 1;//вождение
                                      //20-25 км рекомендованный километраж за занятие. 51 = АИ-95
                                      lessonsCostBenz += int.Parse(item.cost) - 52 * 15;
                                  }
                                  if (StudentsID.Contains(item.student_id) == false)//если ID ещё не было - его добавляем в лист IDшников
                                  {
                                      StudentsID.Add(item.student_id);
                                  }
                              }

                              //lessonsCost = lessonsCost * 0.8;
                              lessonsStudentsCounter = StudentsID.Count;
                              lessonsCounter = teacher_report_lessons.Where(l => l.type_id == 0).Count() + lessonsLectureCounter;


                          }
                          else lessonsCounter = 0;
                      }    

                  }));
            }
        }

        private int _lessonsCounter { get; set; }
        public int lessonsCounter
        {
            get { return _lessonsCounter; }
            set { _lessonsCounter = value; OnPropertyChanged(); }
        }
        private double _MoneyToSchool { get; set; }
        public double MoneyToSchool
        {
            get { return _MoneyToSchool; }
            set { _MoneyToSchool = value; OnPropertyChanged(); }
        }
        private double _lessonsCostBenz { get; set; } //выручка за вычетом бензина
        public double lessonsCostBenz
        {
            get { return _lessonsCostBenz; }
            set { _lessonsCostBenz = value; OnPropertyChanged(); }
        }
        private double _lessonsCost { get; set; }
        public double lessonsCost
        {
            get { return _lessonsCost; }
            set { _lessonsCost = value; OnPropertyChanged(); }
        }
        private int _lessonsLectureCounter { get; set; }
        public int lessonsLectureCounter
        {
            get { return _lessonsLectureCounter; }
            set { _lessonsLectureCounter = value; OnPropertyChanged(); }
        }
        private int _lessonsDrivingCounter { get; set; }
        public int lessonsDrivingCounter
        {
            get { return _lessonsDrivingCounter; }
            set { _lessonsDrivingCounter = value; OnPropertyChanged(); }
        }
        private int _lessonsStudentsCounter { get; set; }
        public int lessonsStudentsCounter
        {
            get { return _lessonsStudentsCounter; }
            set { _lessonsStudentsCounter = value; OnPropertyChanged(); }
        }

        #endregion


        private List<lessonDTO> _teacher_report_lessons { get; set; }
        public List<lessonDTO> teacher_report_lessons//лист для датаГрида репорта преподавателя
        {
            get { return _teacher_report_lessons; }
            set { _teacher_report_lessons = value; OnPropertyChanged(); }
        }


        private DateTime _teacher_first_selected_date {  get; set; }
        public DateTime teacher_first_selected_date//Дата начала периода для отчёта учителя
        {
            get
            {
                return _teacher_first_selected_date;
            }
            set
            {
                _teacher_first_selected_date =value; OnPropertyChanged();
            }
        }
        private DateTime _teacher_second_selected_date {  get; set; }
        public DateTime teacher_second_selected_date //Дата конца периода для отчёта учителя
        {
            get
            {
                return _teacher_second_selected_date;
            }
            set
            {
                _teacher_second_selected_date = value; OnPropertyChanged();
            }
        }

        public List<teacherDTO> AllTeachers { get; set; }
        private teacherDTO _authorized_teacher { get; set; }

        public teacherDTO authorized_teacher
        {
            get
            { return _authorized_teacher; }

            set
            {
                _authorized_teacher = value; OnPropertyChanged();
                teachers_lessons = studentService.GetAllTeacherLessons(authorized_teacher.id);
                teacher_first_selected_date = DateTime.Now;
                teacher_second_selected_date= DateTime.Now.AddDays(5);
            }
        }

        private List<lessonDTO> _teachers_lessons { get; set; }
        public List<lessonDTO> teachers_lessons
        {
            get { return _teachers_lessons; }
            set { _teachers_lessons = value; OnPropertyChanged(); }
        }

        


        private bool _TeacherlessonSignUp { get; set; }//флаг недоступности кнопки
        public bool TeacherlessonSignUp
        {
            get { return _TeacherlessonSignUp; }
            set { _TeacherlessonSignUp = value; OnPropertyChanged(); }
        }

        private lessonDTO _selected_teacher_lessons { get; set; }
        public lessonDTO selected_teacher_lessons
        {
            get { return _selected_teacher_lessons; }
            set { _selected_teacher_lessons = value; OnPropertyChanged();
                if (value != null)
                {
                    if (value.type_id == 0 && value.status == "Назначено")
                    {
                        TeacherlessonSignUp = true;
                    }
                    else
                    {
                        TeacherlessonSignUp = false;
                    }
                }
            }
        }

        #endregion


        public AppMainViewModel(IStudentService studentService)
        {
            this.studentService = studentService;
            NowDate = DateTime.Now;
            selected_monthday = DateTime.Now;
            selected_monthday = NowDate;
            students = studentService.GetAllStudents();
            teachers = studentService.GetAllTeachers();
            AllTeachers = studentService.GetAllTeachers();
            AllCars = studentService.GetAllCars();
            selected_student = students[0];
            authorized_teacher = AllTeachers[0];
            AdminFirstTime = DateTime.Now;
            AdminSecondTime = DateTime.Now.AddMonths(1);

        }
    }
}
