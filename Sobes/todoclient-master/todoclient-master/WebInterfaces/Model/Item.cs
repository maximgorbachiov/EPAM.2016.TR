using System.Collections.Generic;

namespace WebInterfaces.Model
{
    public class Item
    {
        public int ItemId { get; set; }

        public int ToDoId { get; set; }

        public int UserId { get; set; }

        public bool IsCompleted { get; set; }

        public string Name { get; set; }

        public int ServiceId { get; set; }

        public virtual List<Request> Requests { get; set; }
    }
}
