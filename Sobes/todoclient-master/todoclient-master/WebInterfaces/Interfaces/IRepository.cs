using System.Collections.Generic;
using WebInterfaces.Model;

namespace WebInterfaces.Interfaces
{
    public interface IRepository
    {
        List<RequestItem> Load();

        void Save(List<RequestItem> data);
    }
}
