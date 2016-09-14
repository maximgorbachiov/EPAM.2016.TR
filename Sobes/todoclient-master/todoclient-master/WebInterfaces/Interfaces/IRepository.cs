using System.Collections.Generic;

namespace WebInterfaces.Interfaces
{
    public interface IRepository<T>
    {
        int Add(T value);

        void Update(T value);

        void Remove(int id);

        T GetById(int id);

        T GetByName(string name);

        List<T> Load();

        void Save(List<T> data);

        void SaveChanges();
    }
}
