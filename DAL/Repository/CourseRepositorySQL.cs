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
    public class CourseRepositorySQL: IRepository<course>
    {
        private DSModel db;

        public CourseRepositorySQL(DSModel dbcontext)
        {
            this.db = dbcontext;
        }
        public void Create(course item)
        {
            db.course.Add(item);
        }

        public void Delete(int id)
        {
            course st = db.course.Find(id);
            if (st != null)
                db.course.Remove(st);
        }

        public course GetItem(int id)
        {
            return db.course.Find(id);
        }

        public List<course> GetList()
        {
            return db.course.ToList();
        }

        public void Update(course item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
