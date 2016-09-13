using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebInterfaces.Interfaces;

namespace WcfProxy.Generator
{
    public class IdGenerator : IIdGenerator
    {
        private int position = 0;

        public IdGenerator(int position)
        {
            this.position = position;
        }
        public IdGenerator() { }


        public int GetId()
        {
            return position++;
        }

        public void SetCurentPosition(int position)
        {
            this.position = position;
        }

        public int GetCurrentPosition()
        {
            return this.position;
        }
    }
}
