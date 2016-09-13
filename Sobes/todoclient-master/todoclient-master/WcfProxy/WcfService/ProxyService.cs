using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using Newtonsoft.Json;
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

        private const string DeleteUrl = "ToDos/{0}";

        private readonly HttpClient httpClient;

        private readonly IRepository repository;

        private readonly string serviceApiUrl;

        private List<RequestItem> currentData = new List<RequestItem>(); 

        public ProxyService(IRepository repository, string serviceApiUrl)
        {
            this.repository = repository;
            this.serviceApiUrl = serviceApiUrl;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<ToDoItemData> GetAll(int userId)
        {
            var dataAsString = httpClient.GetStringAsync(string.Format(serviceApiUrl + GetAllUrl, userId)).Result;

            var result = JsonConvert.DeserializeObject<List<ToDoItemData>>(dataAsString);

            return result;
        }

        public void CreateItem(ToDoItemData item)
        {
            httpClient.PostAsJsonAsync(serviceApiUrl + CreateUrl, item)
                .Result.EnsureSuccessStatusCode();
        }

        public void UpdateItem(ToDoItemData item)
        {
            httpClient.PutAsJsonAsync(serviceApiUrl + UpdateUrl, item)
                .Result.EnsureSuccessStatusCode();
        }

        public void DeleteItem(int id)
        {
            httpClient.DeleteAsync(string.Format(serviceApiUrl + DeleteUrl, id))
                .Result.EnsureSuccessStatusCode();
        }

        public int CreateUser(string userName)
        {
            var response = httpClient.PostAsJsonAsync(serviceApiUrl + CreateUrl, userName).Result;
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsAsync<int>().Result;
        }

        void IServiceStateLoader.LoadServiceState()
        {
            currentData = repository.Load();

            foreach (var item in currentData)
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
            repository.Save(currentData);
        }
    }
}
