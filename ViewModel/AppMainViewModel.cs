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

namespace Autoschool.ViewModel
{
    public class AppMainViewModel : BaseViewModel
    {
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
                          studentService.RegisterForTheCourse(selected_student.id, selected_courseDataGrid.id);
                          selected_courseDataGrid = null; //убрали детали выбранног курса
                          student_invitations = studentService.GetAllInvitations().Where(i => i.student_id == _selected_student.id).ToList();
                          student_courses = studentService.GetStudentCourses(selected_student.id);
                          courses = studentService.GetAvailableCourses(selected_student.id);
                          student_lessons = studentService.GetAllMyLessons(selected_student.id);
                      }
                  }));
            }
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

        private courseDTO _selected_student_course;
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

        public AppMainViewModel(IStudentService studentService)
        {
            this.studentService = studentService;
            
            students = studentService.GetAllStudents();
            selected_student = students[0];
            
        }
    }
}
