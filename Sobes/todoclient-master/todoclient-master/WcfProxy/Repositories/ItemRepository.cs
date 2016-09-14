using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WcfProxy.DataBaseContext;
using WebInterfaces.Interfaces;
using WebInterfaces.Model;

namespace WcfProxy.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly TemporaryDB context = new TemporaryDB();

        private readonly IIdGenerator generator;

        public ItemRepository(IIdGenerator generator)
        {
            this.generator = generator;
            var sortedList = context.Items.OrderBy(item => item.ServiceId).ToList();

            int position = (sortedList.Count == 0) ? 0 : sortedList[sortedList.Count - 1].ServiceId;
            generator.SetCurentPosition(position + 1);
        }

        public int Add(Item item)
        {
            item.ServiceId = generator.GetId();
            context.Items.Add(item);

            return item.ServiceId;
        }

        public void Update(Item item)
        {
            var findedItem = context.Items.FirstOrDefault(t => t.ServiceId == item.ServiceId);

            if (findedItem != null)
            {
                findedItem.IsCompleted = item.IsCompleted;
                findedItem.Name = item.Name;
                findedItem.ToDoId = item.ToDoId;
                findedItem.UserId = item.UserId;
            }

            context.Entry(findedItem).State = EntityState.Modified;
        }

        public void Remove(int id)
        {
            var item = context.Items.FirstOrDefault(t => t.ServiceId == id);
            context.Items.Remove(item);
        }

        public Item GetById(int id)
        {
            var item = context.Items.FirstOrDefault(t => t.ServiceId == id);

            return item;
        }

        public Item GetByName(string name)
        {
            var item = context.Items.FirstOrDefault(t => t.Name == name);

            return item;
        }

        public Item GetByLocalId(int id)
        {
            return context.Items.Single(item => item.ServiceId == id);
        }

        public Item GetByGlobalId(int id)
        {
            return context.Items.Single(item => item.ServiceId == id);
        }

        public List<Item> GetByUserId(int id)
        {
            return context.Items.Where(i => i.UserId == id).Select(u => u).ToList();
        }

        public void SetToDoIdByUserId(List<Item> items, int userId)
        {
            var usersItems = context.Items.Where(item => item.UserId == userId).ToList();

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < usersItems.Count; j++)
                {
                    if (items[i].ServiceId == usersItems[j].ServiceId)
                    {
                        usersItems[j].ServiceId = items[i].ServiceId;
                        context.Entry(usersItems[j]).State = EntityState.Modified;
                        break;
                    }
                }
            }
        }

        public List<Item> Load()
        {
            return context.Items.Where(item => item.ServiceId >= 0).ToList();
        }

        public void Save(List<Item> data)
        {
            context.Items.AddRange(data);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
