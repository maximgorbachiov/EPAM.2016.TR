using WebInterfaces.Model;

namespace WcfProxy.Mappers
{
    public static class ItemDataToRequestMapper
    {
        public static RequestItem ToRequestItem(this Request request)
        {
            return new RequestItem
            {
                Command = request.Command,
                Data = request.Item.ToItemData()
            };
        }

        public static Request ToRequest(this RequestItem request)
        {
            return new Request
            {
                Command = request.Command,
                Item = request.Data.ToItem()
            };
        }
    }
}
