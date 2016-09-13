using System.ServiceModel;

namespace WebInterfaces.Interfaces
{
    [ServiceContract]
    public interface IUserProxyService
    {
        [OperationContract]
        int CreateUser(string name);
    }
}
