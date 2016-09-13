using System.Collections.Generic;
using System.Linq;
using WebInterfaces.Interfaces;
using WebInterfaces.Model;

namespace WcfProxy.Repositories
{
    public class Repository : IRepository
    {
        private List<ToDoItemData> data = new List<ToDoItemData>();

        public int Add(ToDoItemData item)
        {
            int id = 0;

            data.Add(item);
            item.ServiceId = id;

            return id;
        }

        public void Update(ToDoItemData item)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].ServiceId == item.ServiceId)
                {
                    data[i] = item;
                    break;
                }
            }
        }

        public void Remove(int id)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].ServiceId == id)
                {
                    data.RemoveAt(i);
                    break;
                }
            }
        }

        public ToDoItemData GetById(int id)
        {
            return data.Single(item => item.ServiceId == id);
        }

        public ToDoItemData GetByName(string name)
        {
            return data.FirstOrDefault(item => item.Name == name);
        }

        public List<ToDoItemData> Load()
        {
            return data;
        }

        public void Save(List<ToDoItemData> data)
        {
            this.data = data;
        }
    }
}
