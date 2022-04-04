using System.Collections.Generic;
using System.Linq;

namespace DBBD51
{
    public class DSDirections : IDataSourse
    {
        private IEnumerable<IEitem> dataSourse;
        public int GetMaxId() => SQL.maxIndex("SELECT MAx(id_napr) + 1 From InSy.dbo.Directions");
        public IDataSourse Update() => new DSDirections();

        public DSDirections()
        {
            dataSourse = TransformData(getDataFromSql());
        }

        public IEnumerable<IEitem> GetRows()
        {
            foreach (var x in dataSourse)
                yield return x;
        }

        private IEnumerable<IEnumerable<string>> getDataFromSql() => SQL.ReadSql(@"select * from InSy.dbo.Directions");

        private static IEnumerable<IEitem> TransformData(IEnumerable<IEnumerable<string>> data) => data
            .Select(x => new EDirections(int.Parse(x.First()), x.Last()));
    }
}
