using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;
using Interfaces.Repository;


namespace DAL.Repository
{
    public class ReportRepositorySQL: IReportsRepository
    {
        private DSModel db;
        public ReportRepositorySQL(DSModel dbcontext)
        {
            this.db = dbcontext;
        }

        ////выполнить ХП
        //public List<DTO.BookInELibrary> ExecuteSP(int library_id)
        //{
        //    System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@lib_name", library_id);
        //    //System.Data.SqlClient.SqlParameter param2 = new System.Data.SqlClient.SqlParameter("@year", year);
        //    LibModel db = new LibModel();
        //    var result = db.Database.SqlQuery<DTO.BookInELibrary>("hmboel @lib_name", new object[] { param1 }).ToList();
        //    var data = result
        //        .Select(i => new DTO.BookInELibrary
        //        {
        //            Author = i.Author,
        //            Genre = i.Genre,
        //            Name = i.Name
        //        }).ToList();
        //    return data;
        //}

        //public List<DTO.ReportData> Report(string name)
        //{
        //    LibModel db = new LibModel();

        //    //var per = dbContext.Book.Join(dbContext.Genre, b => b.Genre_ID, g => g.ID, (b, v) => new { bn = b.Name, gn = v.Name });
        //    var result = from b in db.Book // передаем каждый элемент из people в переменную p
        //                 join s in db.Subscription on b.ID equals s.Book_ID
        //                 join g in db.Genre on b.Genre_ID equals g.ID
        //                 join r in db.Reader on s.Reader_ID equals r.ID
        //                 where (r.Name == name)
        //                 select new DTO.ReportData
        //                 {
        //                     BookName = b.Name,
        //                     SubscriptionData = s.Data,
        //                     GenreName = g.Name,
        //                     ReaderName = r.Name
        //                 };
        //    //var request = db.Phones
        //    // .Join(db.Manufacturers, ph => ph.ManufacturerId, m => m.Id, (ph, m) => ph)
        //    // .Where(i => i.ManufacturerId == manufId)
        //    // .Select(i => new ReportData(){ PhoneName = i.Name, Cost = i.Cost })
        //    // .ToList();
        //    return result.ToList();
        //}
    }
}
