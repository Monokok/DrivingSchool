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
    public class invite_courseRepositorySQL : IRepository<invite_course>
    {
        private DSModel db;
       public invite_courseRepositorySQL(DSModel dbcontext)
        {
            this.db = dbcontext;
        }
        public void Create(invite_course item)
        {
            db.invite_course.Add(item);
        }

        public void Delete(int id)
        {
            invite_course st = db.invite_course.Find(id);
            if (st != null)
                db.invite_course.Remove(st);
        }

        public invite_course GetItem(int id)
        {
            return db.invite_course.Find(id);
        }

        public List<invite_course> GetList()
        {
            return db.invite_course.ToList();
        }

        public void Update(invite_course item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
