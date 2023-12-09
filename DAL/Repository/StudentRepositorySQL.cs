using DomainModel;
using Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class StudentRepositorySQL : IRepository<student>
    {
        private DSModel db;

        public StudentRepositorySQL(DSModel dbcontext)
        {
            this.db = dbcontext;
        }
        public void Create(student item)
        {
            db.student.Add(item);
        }

        public void Delete(int id)
        {
            student st = db.student.Find(id);
            if (st != null)
                db.student.Remove(st);
        }

        public student GetItem(int id)
        {
            return db.student.Find(id);
        }

        public List<student> GetList()
        {
            return db.student.ToList();
        }

        public void Update(student item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
