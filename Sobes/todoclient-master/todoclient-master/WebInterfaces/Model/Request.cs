namespace WebInterfaces.Model
{
    public class Request
    {
        public int RequestId { get; set; }

        public Commands Command { get; set; }

        public Item Item { get; set; }
    }
}
