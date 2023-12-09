using DomainModel;
using Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class LessonRepositorySQL: IRepository<lesson>
    {
        private DSModel db;

        public LessonRepositorySQL(DSModel dbcontext)
        {
            this.db = dbcontext;
        }
        public void Create(lesson item)
        {
            db.lesson.Add(item);
        }

        public void Delete(int id)
        {
            lesson st = db.lesson.Find(id);
            if (st != null)
                db.lesson.Remove(st);
        }

        public lesson GetItem(int id)
        {
            return db.lesson.Find(id);
        }

        public List<lesson> GetList()
        {
            return db.lesson.ToList();
        }

        public void Update(lesson item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
