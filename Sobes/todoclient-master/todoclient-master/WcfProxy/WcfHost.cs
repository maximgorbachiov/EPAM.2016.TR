using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using WcfProxy.Repositories;
using WcfProxy.WcfService;
using WebInterfaces.Interfaces;

namespace WcfProxy
{
    public class WcfHost : IDisposable
    {
        private readonly ServiceHost host;

        public WcfHost(string wcfAddress, string serviceApiUrl)
        {
            var uri = new Uri(wcfAddress);
            var wcfService = new ProxyService(new Repository(), serviceApiUrl);

            host = new ServiceHost(wcfService, uri);

            var smb = new ServiceMetadataBehavior
            {
                HttpGetEnabled = true,
                MetadataExporter = { PolicyVersion = PolicyVersion.Policy15 }
            };

            host.Description.Behaviors.Add(smb);
            host.Open();
        }

        public void OpenWcfService()
        {
            ((IServiceStateLoader)host.SingletonInstance).LoadServiceState();
        }

        public void CloseWcfService()
        {
            ((IServiceStateLoader)host.SingletonInstance).SaveServiceState();
        }

        public void Dispose()
        {
            host.Close();
        }
    }
}
