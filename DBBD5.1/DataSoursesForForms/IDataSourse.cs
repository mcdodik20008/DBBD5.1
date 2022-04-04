using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBBD51
{
    public interface IDataSourse
    {
        IEnumerable<IEitem> GetRows();
        int GetMaxId();
       IDataSourse Update();
    }
}
