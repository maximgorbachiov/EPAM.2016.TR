using System.Collections.Generic;
using System.Web.Http;
using ToDoListProject.Models;
using ToDoListProject.ProxyService;
using ToDoListProject.Services;

namespace ToDoListProject.Controllers
{
    public class ToDosController : ApiController
    {
        private static readonly IProxyServiceContract proxy = new ProxyServiceContractClient();
        private static readonly IUserProxyService userProxyService = new UserProxyServiceClient();

        private readonly ToDoService todoService = new ToDoService(proxy);
        private readonly UserService userService = new UserService(userProxyService);

        /// <summary>
        /// Returns all todo-items for the current user.
        /// </summary>
        /// <returns>The list of todo-items.</returns>
        public IList<ToDoItemViewModel> Get()
        {
            var userId = userService.GetOrCreateUser();
            return todoService.GetItems(userId);
        }

        /// <summary>
        /// Updates the existing todo-item.
        /// </summary>
        /// <param name="todo">The todo-item to update.</param>
        public void Put(ToDoItemViewModel todo)
        {
            todo.UserId = userService.GetOrCreateUser();
            todoService.UpdateItem(todo);
        }

        /// <summary>
        /// Deletes the specified todo-item.
        /// </summary>
        /// <param name="id">The todo item identifier.</param>
        public void Delete(int id)
        {
            todoService.DeleteItem(id);
        }

        /// <summary>
        /// Creates a new todo-item.
        /// </summary>
        /// <param name="todo">The todo-item to create.</param>
        public void Post(ToDoItemViewModel todo)
        {
            todo.UserId = userService.GetOrCreateUser();
            todoService.CreateItem(todo);
        }
    }
}