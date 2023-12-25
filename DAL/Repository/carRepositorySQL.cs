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
    public class carRepositorySQL : IRepository<car>
    {
        private DSModel db;
       public carRepositorySQL(DSModel dbcontext)
        {
            this.db = dbcontext;
        }
        public void Create(car item)
        {
            db.cars.Add(item);
        }

        public void Delete(int id)
        {
            car st = db.cars.Find(id);
            if (st != null)
                db.cars.Remove(st);
        }

        public car GetItem(int id)
        {
            return db.cars.Find(id);
        }

        public List<car> GetList()
        {
            return db.cars.ToList();
        }

        public void Update(car item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
