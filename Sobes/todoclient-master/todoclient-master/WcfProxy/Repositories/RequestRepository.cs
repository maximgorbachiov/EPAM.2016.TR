using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WcfProxy.DataBaseContext;
using WebInterfaces.Interfaces;
using WebInterfaces.Model;

namespace WcfProxy.Repositories
{
    public class RequestRepository : IRepository<Request>
    {
        private readonly TemporaryDB context = new TemporaryDB();

        public int Add(Request request)
        {
            var newItem = context.Requests.Add(request);

            return newItem.RequestId;
        }

        public void Update(Request request)
        {
            var findedRequest = context.Requests.FirstOrDefault(req => req.Item.ServiceId == request.Item.ServiceId);

            if (findedRequest != null)
            {
                findedRequest.Item = request.Item;
                findedRequest.Command = request.Command;
            }

            context.Entry(findedRequest).State = EntityState.Modified;
        }

        public void Remove(int serviceId)
        {
            var request = context.Requests.FirstOrDefault(t => t.Item.ServiceId == serviceId);
            context.Requests.Remove(request);
        }

        public Request GetById(int serviceId)
        {
            var request = context.Requests.FirstOrDefault(t => t.Item.ServiceId == serviceId);

            return request;
        }

        public Request GetByName(string name)
        {
            var request = context.Requests.FirstOrDefault(t => t.Item.Name == name);

            return request;
        }

        public List<Request> Load()
        {
            return context.Requests.ToList();
        }

        public void Save(List<Request> data)
        {
            context.Requests.AddRange(data);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
