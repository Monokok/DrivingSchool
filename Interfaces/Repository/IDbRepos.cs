using DomainModel;

namespace Interfaces.Repository
{
    public interface IDbRepos //интерфейс для взаимодействия с репозиториями UnitOfWork
    {
        IRepository<student> Students { get; }
        IRepository<teacher> Teachers { get; }
        IRepository<lesson> Lessons { get; }
        IRepository<course> Courses { get; }
        IRepository<car> Cars { get; }
        IRepository<category> Categories { get; }
        IRepository<invite_course> Invitations { get; }
        IRepository<lesson_type> LessonTypes { get; }

        IReportsRepository Reports { get; }
        int Save();//?
    }
}
