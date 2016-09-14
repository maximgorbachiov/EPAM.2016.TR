using System.Data.Entity;
using WebInterfaces.Model;

namespace WcfProxy.DataBaseContext
{
    public class TemporaryDB : DbContext
    {
        public TemporaryDB() : base("name=TemporaryDB")
        {
        }

        public virtual DbSet<Request> Requests { get; set; }

        public virtual DbSet<Item> Items { get; set; }
    }
}