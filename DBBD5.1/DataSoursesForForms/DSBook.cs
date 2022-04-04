using System;
using System.Collections.Generic;
using System.Linq;

namespace DBBD51
{
    public class DSBook : IDataSourse
    {
        private IEnumerable<IEitem> dataSourse;
        public int GetMaxId() => SQL.maxIndex("SELECT MAx(id_book) From InSy.dbo.Book");
        public IDataSourse Update() => new DSReaders();

        public DSBook()
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
            string book = @"select id_book, bookName, dateRelease, fk_author, fullNameAuthor
                            from InSy.dbo.Book
                            join InSy.dbo.Author ON  fk_author = id_Author";
            return SQL.ReadSql(book);
        }

        private static IEnumerable<IEitem> TransformData(IEnumerable<IEnumerable<string>> data)
        {
            foreach (var item in data)
            {
                var x = item.ToList();
                yield return new EBook(int.Parse(x.ToList()[0]), x.ToList()[1], 
                    new DateTime(int.Parse(x.ToList()[2]), 1, 1), int.Parse(x.ToList()[3]), x.ToList()[4]);
            }
        }
    }
}
