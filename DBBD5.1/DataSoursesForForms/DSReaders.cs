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

        private static IEnumerable<IEitem> TransformData(IEnumerable<IEnumerable<string>> data)
        {
            foreach (var item in data)
            {
                var x = item.ToList();
                yield return new EReaders(int.Parse(x.ToList()[0]), x.ToList()[1], DateTime.Parse(x.ToList()[2]),
                    x.ToList()[3], x.ToList()[4], int.Parse(x.ToList()[5]), x.ToList()[6],
                    int.Parse(x.ToList()[7]), int.Parse(x.ToList()[8]));
            }
        }
    }
}
