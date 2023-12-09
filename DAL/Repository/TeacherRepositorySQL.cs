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
    public class TeacherRepositorySQL : IRepository<teacher>
    {
        private DSModel db;

        public TeacherRepositorySQL(DSModel dbcontext)
        {
            this.db = dbcontext;
        }
        public void Create(teacher item)
        {
            db.teacher.Add(item);
        }

        public void Delete(int id)
        {
            teacher st = db.teacher.Find(id);
            if (st != null)
                db.teacher.Remove(st);
        }

        public teacher GetItem(int id)
        {
            return db.teacher.Find(id);
        }

        public List<teacher> GetList()
        {
            return db.teacher.ToList();
        }

        public void Update(teacher item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
