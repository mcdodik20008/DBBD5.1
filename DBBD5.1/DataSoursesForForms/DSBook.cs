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

        private static IEnumerable<IEitem> TransformData(IEnumerable<IEnumerable<string>> data) => data
            .Select(x => new EBook(int.Parse(x.ElementAt(0)), x.ElementAt(1),
                    new DateTime(int.Parse(x.ElementAt(2)), 1, 1), int.Parse(x.ElementAt(3)), x.ElementAt(4)));
    }
}
