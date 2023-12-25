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
        private CategoryRepositorySQL CategoryRepository;
        private invite_courseRepositorySQL InviteRepository;
        private lesson_typeRepositorySQL LessonTypesRepository;
        private carRepositorySQL CarRepository;



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

        public IRepository<category> Categories
        {
            get
            {
                if (CategoryRepository == null)
                    CategoryRepository = new CategoryRepositorySQL(db);
                return CategoryRepository;
            }
        }

        public IRepository<invite_course> Invitations
        {
            get
            {
                if (InviteRepository == null)
                    InviteRepository = new invite_courseRepositorySQL(db);
                return InviteRepository;
            }
        }

        public IRepository<lesson_type> LessonTypes
        {
            get
            {
                if (LessonTypesRepository == null)
                    LessonTypesRepository = new lesson_typeRepositorySQL(db);
                return LessonTypesRepository;
            }
        }
        public IRepository<car> Cars
        {
            get
            {
                if (CarRepository == null)
                    CarRepository = new carRepositorySQL(db);
                return CarRepository;
            }
        }
        public int Save()
        {
            return db.SaveChanges();
        }
    }
}
