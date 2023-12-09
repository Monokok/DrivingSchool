﻿using DomainModel;

namespace Interfaces.Repository
{
    public interface IDbRepos //интерфейс для взаимодействия с репозиториями UnitOfWork
    {
        IRepository<student> Students { get; }
        IRepository<teacher> Teachers { get; }
        IRepository<lesson> Lessons { get; }
        IRepository<course> Courses { get; }
        IReportsRepository Reports { get; }
        int Save();//?
    }
}
