using System;
using System.Collections.Generic;
using System.Linq;

namespace DBBD51
{
    public class DSReaders : IDataSourse
    {
        private IEnumerable<IEitem> dataSourse;

        public int GetMaxId() => SQL.maxIndex("SELECT Max(id_Lk) From InSy.dbo.LibraryCard");

        public IDataSourse Update() => new DSReaders();

        public DSReaders()
        {
            dataSourse = TransformData(getDataFromSql());
        }

        public IEnumerable<IEitem> GetRows()
        {
            foreach (var x in dataSourse)
                yield return x;
        }

        private IEnumerable<IEnumerable<string>> getDataFromSql()
        {
            string readers = @"SELECT id_Lk, fullName, dateBirth, phoneNumber, homeAdres, fk_dir, name, 
		                                (select count(fk_whoV) from InSy.dbo.Subscription where id_Lk = fk_libCard), 
			                                   (select count(fk_whoV) - count(fk_whoS)  from InSy.dbo.Subscription where id_Lk = fk_libCard)
                                    from InSy.dbo.LibraryCard
                                    join InSy.dbo.Directions on fk_dir = id_napr";
            return SQL.ReadSql(readers);
        }

        private static IEnumerable<IEitem> TransformData(IEnumerable<IEnumerable<string>> data) => data
            .Select(x => 
                new EReaders
                (
                    int.Parse(x.ElementAt(0)), x.ElementAt(1), DateTime.Parse(x.ElementAt(2)),
                    x.ElementAt(3), x.ElementAt(4), int.Parse(x.ElementAt(5)), x.ElementAt(6),
                    int.Parse(x.ElementAt(7)), int.Parse(x.ElementAt(8))
                )
            );

    }
}
