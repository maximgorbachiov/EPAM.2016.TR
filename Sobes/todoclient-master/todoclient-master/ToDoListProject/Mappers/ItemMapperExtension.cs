using ToDoListProject.Models;
using ToDoListProject.ProxyService;

namespace ToDoListProject.Mappers
{
    public static class ItemMapperExtension
    {
        public static ToDoItemData ToItemData(this ToDoItemViewModel model)
        {   
            return new ToDoItemData
            {
                Name = model.Name,
                IsCompleted = model.IsCompleted,
                UserId = model.UserId,
                ToDoId = model.ToDoId
            };
        }

        public static ToDoItemViewModel ToItemViewModel(this ToDoItemData data)
        {
            return new ToDoItemViewModel
            {
                Name = data.Name,
                IsCompleted = data.IsCompleted,
                UserId = data.UserId,
                ToDoId = data.ToDoId
            };
        }
    }
}