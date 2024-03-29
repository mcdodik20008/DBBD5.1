﻿using System;
using System.Collections.Generic;
using System.Linq;

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
            var sql = @"select bookName, dateRelease, fullNameAuthor, 
                                (select count(fk_book) 
                                   from InSy.dbo.Subscription " +
                                   $"where id_book = fk_book and dateV Between  '{left.ToShortDateString()}' and '{right.ToShortDateString()}') " +
                        @"from InSy.dbo.Book
                        join InSy.dbo.Author on fk_author = id_Author";
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

        public IEnumerable<IEitem> GetRows() => dataSourse.Select(x => x);

        public IDataSourse Update() => new DSItogBook();

        public IDataSourse Update(DateTime l, DateTime r) => new DSItogBook(l, r);
    }
}
