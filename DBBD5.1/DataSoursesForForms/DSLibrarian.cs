using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBBD51
{
    public class DSLibrarian : IDataSourse
    {
        private IEnumerable<IEitem> dataSourse;
        public int GetMaxId() => SQL.maxIndex("SELECT MAx(id_Librarian) From InSy.dbo.Librarian");
        public IDataSourse Update() => new DSLibrarian();

        public DSLibrarian()
        {
            dataSourse = TransformData(getDataFromSql());
        }

        public IEnumerable<IEitem> GetRows()
        {
            foreach (var x in dataSourse)
                yield return x;
        }

        private IEnumerable<IEnumerable<string>> getDataFromSql() => SQL.ReadSql(@"select * from InSy.dbo.Librarian");  

        private static IEnumerable<IEitem> TransformData(IEnumerable<IEnumerable<string>> data)
        {
            foreach (var item in data)
            {
                var x = item.ToList();
                yield return new ELibrarian(int.Parse(x.ToList()[0]), x.ToList()[1], DateTime.Parse(x.ToList()[2]));
            }
        }
    }
}
