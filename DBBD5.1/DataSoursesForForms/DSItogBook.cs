using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBBD51
{
    class DSItogBook : IDataSourse
    {
        private IEnumerable<IEitem> dataSourse;

        public DSItogBook()
        {
            var sql = @"select bookName, dateRelease, fullNameAuthor, (select count(fk_book) from InSy.dbo.Subscription where id_book = fk_book)
                        from InSy.dbo.Book
                        join InSy.dbo.Author on fk_author = id_Author";
            dataSourse = TransformData(getDataFromSql(sql));
        }

        DSItogBook(DateTime left, DateTime right)
        {
            var sql =  @"select bookName, dateRelease, fullNameAuthor, 
                                (select count(fk_book) 
                                   from InSy.dbo.Subscription " +
                                   $"where id_book = fk_book and dateV Between  '{left.ToShortDateString()}' and '{right.ToShortDateString()}') " +
                        @"from InSy.dbo.Book
                        join InSy.dbo.Author on fk_author = id_Author";
            dataSourse = TransformData(getDataFromSql(sql));
        }

        private IEnumerable<IEnumerable<string>> getDataFromSql(string sql)
        {
            foreach (List<string> item in SQL.ReadSql(sql))
            {
                yield return item;
            }
        }

        public static IEnumerable<IEitem> TransformData(IEnumerable<IEnumerable<string>> data)
        {
            foreach (var item in data)
            {
                var row = item.ToList();
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

        public IDataSourse Update() =>  new DSItogBook();

        public IDataSourse Update(DateTime l, DateTime r) => new DSItogBook(l, r);
    }
}
