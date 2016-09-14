using System.Collections.Generic;
using System.Linq;
using WcfProxy.Mappers;
using WebInterfaces.Interfaces;
using WebInterfaces.Model;

namespace WcfProxy.Services
{
    public class ItemService
    {
        private readonly IItemRepository repository;

        public ItemService(IItemRepository repository)
        {
            this.repository = repository;
        }

        public int AddItem(ToDoItemData data)
        {
            var result = repository.Add(data.ToItem());
            repository.SaveChanges();

            return result;
        }

        public void UpdateItem(ToDoItemData data)
        {
            repository.Update(data.ToItem());
            repository.SaveChanges();
        }

        public void RemoveItem(int serviceId)
        {
            repository.Remove(serviceId);
            repository.SaveChanges();
        }

        public ToDoItemData FindByServiceId(int serviceId)
        {
            return repository.GetById(serviceId).ToItemData();
        }

        public void SetToDoIdForUserItems(List<ToDoItemData> items, int userId)
        {
            repository.SetToDoIdByUserId(items.Select(item => item.ToItem()).ToList(), userId);
            repository.SaveChanges();
        }

        public List<ToDoItemData> LoadItems()
        {
            var items = repository.Load()
                .Select(item => item.ToItemData())
                .ToList();

            return items;
        }

        public void SaveItems(List<ToDoItemData> data)
        {
            repository.Save(data.Select(item => item.ToItem()).ToList());
        }

        public ToDoItemData GetByGlobalId(int id)
        {
            var result = repository.GetByGlobalId(id).ToItemData();
            return result;
        }

        public ToDoItemData GetByLocalId(int id)
        {
            var result = repository.GetByLocalId(id).ToItemData();
            return result;
        }

        public List<ToDoItemData> GetByUserId(int userId) 
        {
            var result = repository
                .GetByUserId(userId)
                .Select(item => item.ToItemData())
                .ToList();

            return result;
        }
    }
}
