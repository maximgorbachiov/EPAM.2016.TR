using System.Collections.Generic;
using WebInterfaces.Model;

namespace WebInterfaces.Interfaces
{
    public interface IRepository
    {
        int Add(ToDoItemData item);

        void Update(ToDoItemData item);

        void Remove(int id);

        ToDoItemData GetById(int id);

        ToDoItemData GetByName(string name);

        List<ToDoItemData> Load();

        void Save(List<ToDoItemData> data);
    }
}
