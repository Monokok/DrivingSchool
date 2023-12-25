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
    public class lesson_typeRepositorySQL: IRepository<lesson_type>
    {
        private DSModel db;

        public lesson_typeRepositorySQL(DSModel dbcontext)
        {
            this.db = dbcontext;
        }
        public void Create(lesson_type item)
        {
            db.lesson_type.Add(item);
        }

        public void Delete(int id)
        {
            lesson_type st = db.lesson_type.Find(id);
            if (st != null)
                db.lesson_type.Remove(st);
        }

        public lesson_type GetItem(int id)
        {
            return db.lesson_type.Find(id);
        }

        public List<lesson_type> GetList()
        {
            return db.lesson_type.ToList();
        }

        public void Update(lesson_type item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
