//using DomainModel;
using DomainModel;
using Interfaces.DTO;
using Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class DbReposSQL: IDbRepos
    {
        private DSModel db;
        private StudentRepositorySQL StudentRepository;
        private TeacherRepositorySQL TeacherRepository;
        private LessonRepositorySQL LessonRepository;
        private CourseRepositorySQL CourseRepository;

        //private ReaderRepositorySQL ReaderRepository;
        //private GenreRepositorySQL GenreRepository;
        //private LibraryRepositorySQL LibraryRepository;
        //private SubscriptionRepositorySQL SubscriptionRepository;
        private ReportRepositorySQL reportRepository;

        public DbReposSQL()
        {
            db = new DSModel();
        }
        public IRepository<student> Students
        {
            get
            {
                if (StudentRepository == null)
                    StudentRepository = new StudentRepositorySQL(db);
                return StudentRepository;
            }
        }
        public IReportsRepository Reports
        {
            get
            {
                if (reportRepository == null)
                    reportRepository = new ReportRepositorySQL(db);
                return reportRepository;
            }
        }

        public IRepository<teacher> Teachers
        {
            get
            {
                if (TeacherRepository == null)
                    TeacherRepository = new TeacherRepositorySQL(db);
                return TeacherRepository;
            }
        }

        public IRepository<lesson> Lessons
        {
            get
            {
                if (LessonRepository == null)
                    LessonRepository = new LessonRepositorySQL(db);
                return LessonRepository;
            }
        }

        public IRepository<course> Courses
        {
            get
            {
                if (CourseRepository == null)
                    CourseRepository = new CourseRepositorySQL(db);
                return CourseRepository;
            }
        }

        public int Save()
        {
            return db.SaveChanges();
        }
    }
}
