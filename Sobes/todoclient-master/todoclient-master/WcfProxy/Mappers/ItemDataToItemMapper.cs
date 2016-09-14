using WebInterfaces.Model;

namespace WcfProxy.Mappers
{
    public static class ItemDataToItemMapper
    {
        public static ToDoItemData ToItemData(this Item item)
        {
            return new ToDoItemData
            {
                ToDoId = item.ToDoId,
                UserId = item.UserId,
                IsCompleted = item.IsCompleted,
                Name = item.Name,
                ServiceId = item.ServiceId
            };
        }

        public static Item ToItem(this ToDoItemData item)
        {
            return new Item
            {
                ToDoId = item.ToDoId,
                UserId = item.UserId,
                IsCompleted = item.IsCompleted,
                Name = item.Name,
                ServiceId = item.ServiceId
            };
        }
    }
}
