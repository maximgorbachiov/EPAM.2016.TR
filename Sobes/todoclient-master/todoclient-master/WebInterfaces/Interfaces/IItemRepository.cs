using System.Collections.Generic;
using WebInterfaces.Model;

namespace WebInterfaces.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        void SetToDoIdByUserId(List<Item> items, int userId);

        Item GetByGlobalId(int id);

        Item GetByLocalId(int id);

        List<Item> GetByUserId(int id);
    }
}
