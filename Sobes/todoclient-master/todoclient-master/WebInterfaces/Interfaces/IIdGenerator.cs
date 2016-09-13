using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterfaces.Interfaces
{
    public interface IIdGenerator
    {
        int GetId();
        void SetCurentPosition(int position);
        int GetCurrentPosition();
    }
}
