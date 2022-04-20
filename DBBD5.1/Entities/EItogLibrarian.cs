using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBBD51
{
    class EItogLibrarian : IEitem
    {
        string libName;
        string dateTime;
        string vidal;
        string prinyal;

        public EItogLibrarian(string libName, string dateTime, string vidal, string prinyal)
        {
            this.libName = libName;
            this.dateTime = dateTime;
            this.vidal = vidal;
            this.prinyal = prinyal;
        }

        public List<string> GetListValForDataGrid() => new List<string>()
                    { libName, dateTime, vidal, prinyal };

        public HeadDataGrid GetHeadDataGrid() => throw new NotImplementedException();

        public List<string> GetListValForSql() => new List<string>();

        public string GetNameTable() => "";

        public string GetValueForSql() => "";

        public bool IsGood() => true;
    }
}
