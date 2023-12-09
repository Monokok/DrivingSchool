using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Repository
{
    public interface IRepository<T> where T : class //обобщенный интерфейс репозитория для сущностей приложения 
    {
        List<T> GetList(); //получение всех объектов
        T GetItem(int id); //Получение объекта по ID
        void Create(T item); //Создание объекта
        void Update(T item); //Обновление объекта
        void Delete(int id); //Удаление объекта по ID
        
    }
}
