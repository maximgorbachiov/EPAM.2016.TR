using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ToDoListProject.Mappers;
using ToDoListProject.Models;
using WebInterfaces.Interfaces;
using IProxyServiceContract = ToDoListProject.ProxyService.IProxyServiceContract;

//using WebInterfaces.Interfaces;

namespace ToDoListProject.Services
{
    /// <summary>
    /// Works with ToDo backend.
    /// </summary>
    public class ToDoService
    {
        private readonly IProxyServiceContract service;

        /// <summary>
        /// Creates the service.
        /// </summary>
        public ToDoService(IProxyServiceContract service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets all todos for the user.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>The list of todos.</returns>
        public IList<ToDoItemViewModel> GetItems(int userId)
        {
            var result = service.GetAll(userId)
                .Select(itemData => itemData.ToItemViewModel())
                .ToList();

            return result;
        }

        /// <summary>
        /// Creates a todo. UserId is taken from the model.
        /// </summary>
        /// <param name="item">The todo to create.</param>
        public void CreateItem(ToDoItemViewModel item)
        {
            service.CreateItem(item.ToItemData());
        }

        /// <summary>
        /// Updates a todo.
        /// </summary>
        /// <param name="item">The todo to update.</param>
        public void UpdateItem(ToDoItemViewModel item)
        {
            service.UpdateItem(item.ToItemData());
        }

        /// <summary>
        /// Deletes a todo.
        /// </summary>
        /// <param name="id">The todo Id to delete.</param>
        public void DeleteItem(int id)
        {
            service.DeleteItem(id);
        }
    }
}