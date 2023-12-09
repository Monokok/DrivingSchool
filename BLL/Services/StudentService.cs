
using DomainModel;
using Interfaces.DTO;
using Interfaces.Repository;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class StudentService: IStudentService //сервис студента - все связанные с ним ЕГО возможности как пользователя
    {
        private IDbRepos db;
        public StudentService(IDbRepos repos)
        {
            db = repos;//new DSModel();
        }

        public List<courseDTO> GetAllCourses()
        {
            return db.Courses.GetList().Select(i => new courseDTO(i)).ToList();
        }

        public List<lessonDTO> GetAllLessons()
        {
            //преобразование из объекта "lesson" в "lessonDTO"
            return db.Lessons.GetList().Select(i => new lessonDTO(i)).ToList();
        }

        public List<lessonDTO> GetAllMyLessons(int id)
        {
            List<lesson> lessons = db.Lessons.GetList().Where(l => l.student_id == id).ToList();
            return lessons.Select(l => new lessonDTO(l)).ToList();
        }

        public List<studentDTO> GetAllStudents()
        {
            return db.Students.GetList().Select(i => new studentDTO(i)).ToList();
        }

        //public List<BookDTO> GetAllBooks()
        //{
        //    return db.Books.GetList().Select(i => new BookDTO(i)).ToList();
        //}
        ////public class SubReaderBookGenre
        ////{
        ////    public SubReaderBookGenre() { }
        ////    public string Name { get; set; }
        ////    public string BookName { get; set; }
        ////    public string BookGenre { get; set; }
        ////    public DateTime? Data { get; set; }
        ////}
        //public List<SubReaderBookGenre> GetAllSubscriptions()
        //{
        //    var res = db.Subscriptions.GetList()
        //        .Join(db.Readers.GetList(), s => s.Reader_ID, r => r.ID, (s, r) => new { r, s })
        //        .Join(db.Books.GetList(), sub => sub.s.Book_ID, b => b.ID, (sub, b) => new { sub, b })
        //        .Join(db.Genres.GetList(), bbk => bbk.b.Genre_ID, g => g.ID, (bbk, g) => new { bbk, g })
        //        .Select(n => new SubReaderBookGenre
        //        {
        //            Name = n.bbk.sub.r.Name,
        //            Data = n.bbk.sub.s.Data,
        //            BookName = n.bbk.b.Name,
        //            BookGenre = n.g.Name
        //        });
        //    return res.ToList();
        //    // .Join(db.Book, bk => bk.s.Book_ID, b => b.ID, (bk, b) => new { bk, b })
        //}

        ////public class BookGenreNoID
        ////{
        ////    public BookGenreNoID() { }
        ////    public string Name { get; set; }
        ////    public string Genre { get; set; }
        ////    public int Amount { get; set; }
        ////}

        //public List<BookGenreNoID> GetAllAvailableBooksForGrid()
        //{
        //    return db.Books.GetList().Join(db.Genres.GetList(), b => b.Genre_ID, g => g.ID, (b, g) => new { b, g })
        //        .Where(bk => bk.b.Amount > 0)
        //        .Select(bk => new BookGenreNoID
        //        {
        //            Name = bk.b.Name,
        //            Genre = bk.g.Name,
        //            Amount = bk.b.Amount
        //        }).ToList();
        //    //.ToList().Select(i => new BookDTO(i)).Where(b => b.Amount > 0).ToList();
        //}
        //public List<BookDTO> GetAllAvailableBooks()
        //{
        //    return db.Books.GetList().Select(i => new BookDTO(i)).Where(b => b.Amount > 0).ToList();
        //}

        //public ReaderDTO GetReader(int id)
        //{
        //    //Reader r = db.Reader.Where(i => i.ID == id).FirstOrDefault();

        //    //return new ReaderDTO(r);
        //    return new ReaderDTO(db.Readers.GetItem(id));
        //}

        ////public BookDTO GetBook(int id)
        ////{
        ////    Book b = db.Book.Where(i => i.ID == id).FirstOrDefault();

        ////    return new BookDTO(b);
        ////}

        //public BookDTO GetBook(int Id)
        //{
        //    return new BookDTO(db.Books.GetItem(Id));
        //}
        //public void CreateBook(BookDTO b)
        //{
        //    db.Books.Create(new Book() { Author = b.Author, Genre_ID = b.Genre_ID, Name = b.Name, Amount = b.Amount, Library_ID = b.Library_ID });
        //    Save();
        //    //db.Phones.Attach(p);
        //}
        //public void UpdateBook(BookDTO p)
        //{
        //    Book b = db.Books.GetItem(p.ID);
        //    b.Amount = p.Amount;
        //    b.Name = p.Name;
        //    b.Genre_ID = p.Genre_ID;
        //    b.Author = p.Author;
        //    //ph.ManufacturerId = p.ManufacturerId;
        //    Save();
        //}
        //public void DeleteBook(int id)
        //{
        //    Book b = db.Books.GetItem(id);
        //    if (b != null)
        //    {
        //        db.Books.Delete(b.ID);
        //        Save();
        //    }
        //}
        //public bool Save()
        //{
        //    if (db.Save() > 0) return true;
        //    return false;
        //}

        //public List<GenreDTO> GetAllGenres()
        //{
        //    return db.Genres.GetList().Select(i => new GenreDTO(i)).ToList();
        //}

        //public List<LibraryDTO> GetAllLibraries()
        //{
        //    return db.Libraries.GetList().Select(i => new LibraryDTO(i)).ToList();
        //}
        //public List<ReaderDTO> GetAllReaders()
        //{
        //    return db.Readers.GetList().Select(i => new ReaderDTO(i)).ToList();
        //}

    }
}
