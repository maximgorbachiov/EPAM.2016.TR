using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WcfProxy.Services;
using WebInterfaces.Interfaces;
using WebInterfaces.Model;

namespace WcfProxy.WcfService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProxyService : IProxyServiceContract, IUserProxyService, IServiceStateLoader
    {
        private const string GetAllUrl = "ToDos?userId={0}";

        private const string UpdateUrl = "ToDos";

        private const string CreateUrl = "ToDos";

        private const string CreateUserUrl = "Users";

        private const string DeleteUrl = "ToDos/{0}";

        private readonly HttpClient httpClient;

        private readonly ItemService itemService;
        private readonly RequestService requestService;

        private readonly string serviceApiUrl;

        private readonly Queue<RequestItem> toDoQueue = new Queue<RequestItem>();
        private List<ToDoItemData> toDoItemData = new List<ToDoItemData>();
        private Thread queueTrackThread;

        public ProxyService(ItemService itemService, RequestService requestService, string serviceApiUrl)
        {
            this.itemService = itemService;
            this.requestService = requestService;
            this.serviceApiUrl = serviceApiUrl;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            queueTrackThread = new Thread(new ThreadStart(QueueTrack));
            queueTrackThread.Start();
        }

        public List<ToDoItemData> GetAll(int userId)
        {
            var res = itemService.GetByUserId(userId);

            return itemService.GetByUserId(userId);
        }

        // private List<ToDoItemData> GetAllAsync(int userId)
        // {
        //     /*var dataAsString = httpClient.GetStringAsync(string.Format(serviceApiUrl + GetAllUrl, userId)).Result;
        //
        //     var result = JsonConvert.DeserializeObject<List<ToDoItemData>>(dataAsString);
        //
        //     itemService.SetToDoIdForUserItems(result, userId);*/
        //
        //     var res = itemService.GetByUserId(userId);
        //     return itemService.GetByUserId(userId);
        // }

        public void CreateItem(ToDoItemData item)
        {
            var items = itemService.GetByUserId(item.UserId);
            var existTask = items.Find(i => i.Name == item.Name);

            if (existTask == null)
            {
                itemService.AddItem(item);

                RequestItem task = new RequestItem
                {
                    Command = Commands.Create,
                    Data = item
                };

                toDoQueue.Enqueue(task);
                GetAllByUserId(task.Data.UserId);
            }

            item.ServiceId = itemService.AddItem(item);
            httpClient.PostAsJsonAsync(serviceApiUrl + CreateUrl, item)
                .Result.EnsureSuccessStatusCode();
        }

        public void UpdateItem(ToDoItemData item)
        {
            var localItem = itemService.GetByGlobalId(item.ServiceId);
            localItem.IsCompleted = item.IsCompleted;
            localItem.Name = item.Name;
            localItem.ServiceId = item.ServiceId;
            localItem.ToDoId = itemService.GetByLocalId(item.ServiceId).ToDoId;
            localItem.UserId = item.UserId;

            RequestItem task = new RequestItem
            {
                Command = Commands.Update,
                Data = localItem
            };

            toDoQueue.Enqueue(task);
        }

        public void DeleteItem(int id)
        {
            var item = itemService.GetByGlobalId(id);
            itemService.RemoveItem(id);

            var task = new RequestItem
            {
                Command = Commands.Delete,
                Data = item
            };

            toDoQueue.Enqueue(task);
        }

        public int CreateUser(string userName)
        {
            var response = httpClient.PostAsJsonAsync(serviceApiUrl + CreateUserUrl, userName).Result;
            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsAsync<int>().Result;

            return result;
        }

        private void QueueTrack()
        {
            while (true)
            {
                if (toDoQueue.Count > 0)
                {
                    var task = toDoQueue.Dequeue();
                    TranslateTask(task);
                    //toDoQueue.Dequeue();
                }
                else
                    Thread.Sleep(20);
            }
        }

        private async void TranslateTask(RequestItem task)
        {
            HttpResponseMessage result = null;

            switch (task.Command)
            {
                case Commands.Create:
                    result = (await httpClient.PostAsJsonAsync(serviceApiUrl + CreateUrl, task.Data)).EnsureSuccessStatusCode();
                    break;
                case Commands.Delete:
                    var globalId = task.Data.ToDoId;
                    if (globalId != 0)
                    {
                        result = httpClient.DeleteAsync(string.Format(serviceApiUrl + DeleteUrl, task.Data.ToDoId)).Result;
                        result.EnsureSuccessStatusCode();
                    }
                    else
                        toDoQueue.Enqueue(task);
                    break;
                case Commands.Update:
                    if (task.Data.ToDoId != 0)
                    {
                        var model = new ToDoItemData
                        {
                            Name = task.Data.Name,
                            IsCompleted = task.Data.IsCompleted,
                            ToDoId = task.Data.ToDoId,
                            UserId = task.Data.UserId
                        };
                        result = httpClient.PutAsJsonAsync(serviceApiUrl + UpdateUrl, model).Result;
                        result.EnsureSuccessStatusCode();
                    }
                    else
                    {
                        toDoQueue.Enqueue(task);
                    }
                    break;
                case Commands.GetAll:
                    int userId = task.Data.UserId;
                    var dataAsString = await httpClient.GetStringAsync(string.Format(serviceApiUrl + GetAllUrl, userId));
                    var serverToDoList = JsonConvert.DeserializeObject<List<ToDoItemData>>(dataAsString);
                    MapGlobalId(userId, serverToDoList);
                    break;
                default:
                    break;
            }
        }

        private void MapGlobalId(int userId, List<ToDoItemData> items)
        {
            foreach (var toDo in itemService.GetByUserId(userId))
            {
                if (toDo.ToDoId == 0)
                {
                    var item = items.Find(u => StringEquals(u.Name, toDo.Name));
                    if (item != null)
                    {
                        toDo.ToDoId = item.ToDoId;
                    }
                }
            }
        }

        private bool StringEquals(string strMain, string strSub)
        {
            var res = strMain.Split(new string[] { strSub }, 2, System.StringSplitOptions.None);
            if ((res != null) && (res.Length == 2) && string.IsNullOrWhiteSpace(res[1]))
            {
                return true;
            }
            return false;
        }

        void IServiceStateLoader.LoadServiceState()
        {
            toDoItemData = itemService.LoadItems();

            foreach (var item in toDoQueue)
            {
                switch (item.Command)
                {
                    case Commands.Create:
                        CreateItem(item.Data);
                        break;
                    case Commands.Update:
                        UpdateItem(item.Data);
                        break;
                    case Commands.Delete:
                        DeleteItem(item.Data.ToDoId);
                        break;
                }
            }
        }

        void IServiceStateLoader.SaveServiceState()
        {
            itemService.SaveItems(toDoItemData);
        }

        private void GetAllByUserId(int userId)
        {
            ToDoItemData item = new ToDoItemData { UserId = userId };

            RequestItem task = new RequestItem
            {
                Command = Commands.GetAll,
                Data = item,
            };

            toDoQueue.Enqueue(task);
        }
    }
}
