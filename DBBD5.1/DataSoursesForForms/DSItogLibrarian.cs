using System;
using System.Collections.Generic;
using System.Linq;

namespace DBBD51
{
    class DSItogLibrarian : IDataSourse
    {
        private IEnumerable<IEitem> dataSourse;

        public DSItogLibrarian()
        {
            var sql = @"select fullName, dateBirth, 
                            (select count(fk_book) from InSy.dbo.Subscription where id_Librarian = fk_whoV),
                            (select count(fk_book) from InSy.dbo.Subscription where id_Librarian = fk_whoS)
                        from InSy.dbo.Librarian";
            dataSourse = TransformData(getDataFromSql(sql));
        }

        DSItogLibrarian(DateTime left, DateTime right)
        {
            var sql = @"select fullName, dateBirth,  
                        	(select count(fk_book) from InSy.dbo.Subscription " + 
                        		$"where id_Librarian = fk_whoV and dateV Between  '{left.ToShortDateString()}' and '{right.ToShortDateString()}'), " +
                        	@" (select count(fk_book) from InSy.dbo.Subscription " +
                        		$" where id_Librarian = fk_whoS and dateS Between  '{left.ToShortDateString()}' and '{right.ToShortDateString()}') " +
                        @" from InSy.dbo.Librarian";
            dataSourse = TransformData(getDataFromSql(sql));
        }

        private IEnumerable<IEnumerable<string>> getDataFromSql(string sql) => SQL.ReadSql(sql).Select(x => x);

        public static IEnumerable<IEitem> TransformData(IEnumerable<IEnumerable<string>> data)
        {
            foreach (var item in data)
            {
                var row = item.ToList();
                if (row[3] == "0")
                    continue;
                yield return new EItogBook(
                    row[0], row[1], row[2], int.Parse(row[3]));
            }
        }

        public List<ComboBoxItems> GetDataComboBoxs() => new List<ComboBoxItems>();

        public int GetMaxId() => 0;

        public IEnumerable<IEitem> GetRows()
        {
            foreach (var item in dataSourse)
                yield return item;
        }

        public IDataSourse Update() => new DSItogBook();

        public IDataSourse Update(DateTime l, DateTime r) => new DSItogLibrarian(l, r);
    }
}
