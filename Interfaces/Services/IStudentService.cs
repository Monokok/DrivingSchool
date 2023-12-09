using Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Services
{
    public interface IStudentService
    {
        List<studentDTO> GetAllStudents();
        List<lessonDTO> GetAllLessons();
        List<lessonDTO> GetAllMyLessons(int id);
        List<courseDTO> GetAllCourses();
        
        //List<SubReaderBookGenre> GetAllSubscriptions();
        //List<BookGenreNoID> GetAllAvailableBooksForGrid();
        //List<BookDTO> GetAllAvailableBooks();
        //ReaderDTO GetReader(int id);
        //BookDTO GetBook(int id);
        //void CreateBook(BookDTO b);
        //void UpdateBook(BookDTO p);
        //void DeleteBook(int id);
        //List<GenreDTO> GetAllGenres();
        //List<LibraryDTO> GetAllLibraries();
        //List<ReaderDTO> GetAllReaders();
    }
}
