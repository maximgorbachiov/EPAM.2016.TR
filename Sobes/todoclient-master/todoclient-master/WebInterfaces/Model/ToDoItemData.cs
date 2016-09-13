using System.Runtime.Serialization;

namespace WebInterfaces.Model
{
    [DataContract]
    public class ToDoItemData
    {
        [DataMember]
        public int ToDoId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public bool IsCompleted { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
