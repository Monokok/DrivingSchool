using Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IStudentService
    {
        List<studentDTO> GetAllStudents();
        List<teacherDTO> GetAllTeachers();
        List<lessonDTO> GetAllLessons();
        List<lessonDTO> GetAllMyLessons(int id);
        List<courseDTO> GetAllCourses();
        List<courseDTO> GetStudentCourses(int student_id);
        List<courseDTO> GetAvailableCourses(int student_id);
        List<carDTO> GetAllCars();
        List<lessonDTO> GetAmountLessonsForPeriod(DateTime _start, DateTime _end);
        List<DateTime> GetAvailableHours(int _teacher_id, DateTime _DayMonthYear);

        List<categoryDTO> GetAllCategories();
        List<invite_courseDTO> GetAllInvitations();
        bool RegisterForTheCourse(int _student_id, int course_id);
        bool RegisterForTheLesson(int _student_id, int _teacher_id, int _car_id, DateTime _time, bool _flag);
        bool CancelTheLesson(int _student_id, int _lesson_id);

        bool CancelTeacherTheLesson(int _Teacher_id, int _lesson_id);


        int GetStudentPaidHours(int student_id);
        int GetStudentSpent(int _student_id);
        int GetHowManyLessonNoPay(int _student_id);

        List<courseDTO> GetAllCoursesForAdmin(DateTime _start, DateTime _end);

        List<lessonDTO> GetAllTeacherLessons(int id);

        List<lessonDTO> GetTeacherReportLessons(int _teacher_id, DateTime _start, DateTime _end);

        //List<SubReaderBookGenre> GetAllSubscriptions();
        //List<BookGenreNoID> GetAllAvailableBooksForGrid();
        //List<BookDTO> GetAllAvailableBooks();
        //ReaderDTO GetReader(int id);
        //BookDTO GetBook(int id);
        //void CreateBook(BookDTO b);
        //void UpdateBook(BookDTO p);
        //void DeleteBook(int id);
        //List<GenreDTO> GetAllGenres();
        //List<LibraryDTO> GetAllLibraries();
        //List<ReaderDTO> GetAllReaders();
    }
}
