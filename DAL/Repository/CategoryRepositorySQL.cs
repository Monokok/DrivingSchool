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
    public class CategoryRepositorySQL: IRepository<category>
    {

        private DSModel db;

        public CategoryRepositorySQL(DSModel dbcontext)
        {
            this.db = dbcontext;
        }

        public void Create(category item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(category item)
        {
            throw new NotImplementedException();
        }

        category IRepository<category>.GetItem(int id)
        {
            return db.category.Find(id);
        }

        List<category> IRepository<category>.GetList()
        {
            return db.category.ToList();
        }
    }
}
