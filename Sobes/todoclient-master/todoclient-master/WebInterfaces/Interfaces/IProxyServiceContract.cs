using System.Collections.Generic;
using System.ServiceModel;
using WebInterfaces.Model;

namespace WebInterfaces.Interfaces
{
    [ServiceContract]
    public interface IProxyServiceContract
    {
        [OperationContract]
        List<ToDoItemData> GetAll(int userId);

        [OperationContract]
        void CreateItem(ToDoItemData item);

        [OperationContract]
        void UpdateItem(ToDoItemData item);

        [OperationContract]
        void DeleteItem(int id);
    }
}
