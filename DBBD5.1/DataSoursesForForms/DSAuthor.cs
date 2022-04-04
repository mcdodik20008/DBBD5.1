
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBBD51
{
    class DSAuthor : IDataSourse
    {
        private IEnumerable<IEitem> dataSourse;
        public int GetMaxId() => SQL.maxIndex("SELECT MAx(id_Author) From InSy.dbo.Author");
        public IDataSourse Update() => new DSAuthor();

        public DSAuthor()
        {
            dataSourse = TransformData(getDataFromSql());
        }

        public IEnumerable<IEitem> GetRows()
        {
            foreach (var x in dataSourse)
                yield return x;
        }

        private IEnumerable<IEnumerable<string>> getDataFromSql() => SQL.ReadSql(@"select * from InSy.dbo.Author");

        private static IEnumerable<IEitem> TransformData(IEnumerable<IEnumerable<string>> data) => data
            .Select(x => new EAuthor(int.Parse(x.ElementAt(0)), x.ElementAt(1), DateTime.Parse(x.ElementAt(2))));
    }
}
