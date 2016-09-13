using System.Collections.Generic;
using WebInterfaces.Interfaces;
using WebInterfaces.Model;

namespace WcfProxy.Repositories
{
    public class Repository : IRepository
    {
        private List<RequestItem> data = new List<RequestItem>();

        public List<RequestItem> Load()
        {
            return data;
        }

        public void Save(List<RequestItem> data)
        {
            this.data = data;
        }
    }
}
