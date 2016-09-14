using System.Collections.Generic;
using System.Linq;
using WcfProxy.Mappers;
using WebInterfaces.Interfaces;
using WebInterfaces.Model;

namespace WcfProxy.Services
{
    public class RequestService
    {
        private readonly IRepository<Request> repository;

        public RequestService(IRepository<Request> repository)
        {
            this.repository = repository;
        }

        public int AddRequestItem(RequestItem data)
        {
            var result = repository.Add(data.ToRequest());
            repository.SaveChanges();

            return result;
        }

        public void UpdateItem(RequestItem data)
        {
            repository.Update(data.ToRequest());
            repository.SaveChanges();
        }

        public void RemoveItem(int serviceId)
        {
            repository.Remove(serviceId);
            repository.SaveChanges();
        }

        public RequestItem FindByServiceId(int serviceId)
        {
            return repository.GetById(serviceId).ToRequestItem();
        }

        public List<RequestItem> LoadItems()
        {
            return repository.Load()
                .Select(item => item.ToRequestItem())
                .ToList();
        }

        public void SaveItems(List<RequestItem> data)
        {
            repository.Save(data.Select(item => item.ToRequest()).ToList());
        }
    }
}
