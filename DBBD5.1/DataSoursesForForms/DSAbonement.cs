using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DBBD51
{
    public class DSAbonement : IDataSourse
    {
        private List<ComboBoxItems> ComboBoxOnForm = new List<ComboBoxItems>();
        private IEnumerable<IEitem> dataSourse;
        private int currentId;
        private List<Control> textAndComboBox;

        public int GetMaxId() => SQL.maxIndex("SELECT MAX(id_zap) From InSy.dbo.Subscription");

        public IDataSourse Update() => new DSAbonement(currentId, textAndComboBox);

        public DSAbonement(int currentId, List<Control> textAndComboBox)
        {
            this.currentId = currentId;
            this.textAndComboBox = textAndComboBox;
            dataSourse = TransformData(getDataFromSql(currentId));
            for (int i = 0; i < 3; i++)
                ComboBoxOnForm.Add(new ComboBoxItems());
            (textAndComboBox[0] as ComboBox).FillingAutors(ComboBoxOnForm[0], currentId);
            (textAndComboBox[2] as ComboBox).FillLibrarian(ComboBoxOnForm[1]);
            (textAndComboBox[4] as ComboBox).FillLibrarian(ComboBoxOnForm[2]);
        }

        public IEnumerable<IEitem> GetRows()
        {
            foreach (var x in dataSourse)
                yield return x;
        }

        private IEnumerable<IEnumerable<string>> getDataFromSql(int currentId)
        {
            string getMoreInfo = @"SELECT id_zap, fk_book, bookName, id_Author, fullnameAuthor, fk_whoV, B1.fullName, dateV, fk_whoS,B2.fullName, dateS
                                   From InSy.dbo.Subscription
                                   JOIN InSy.dbo.Book ON id_book = fk_book
                                   JOIN InSy.dbo.Author ON id_book = fk_book and fk_author = id_Author
                                   JOIN InSy.dbo.Librarian AS B1 ON fk_whoV = B1.id_Librarian
                                   Left JOIN InSy.dbo.Librarian As B2 ON fk_whoS = B2.id_Librarian " +
                                   $"where fk_libCard = {currentId}";

            foreach (List<string> item in SQL.ReadSql(getMoreInfo))
            {
                item.Insert(1, currentId.ToString());
                yield return item;
            }
        }  

        public static IEnumerable<IEitem> TransformData(IEnumerable<IEnumerable<string>> data)
        {
            foreach (var item in data)
            {
                var row = item.ToList();
                int? fkS = null;
                DateTime? dateS = null;
                string nameS = null;

                if (!(row[9] == null || row[9] == ""))
                {
                    fkS = int.Parse(row[9]);
                    nameS = row[10];
                    dateS = DateTime.Parse(row[11]);
                }

                yield return new EAbonement(
                        int.Parse(row[0]), //первичный ключ
                        int.Parse(row[1]), //выбранный чел
                        int.Parse(row[2]), //внешний ключ книги
                        row[3], //название книги
                        int.Parse(row[4]), //внешний ключ автора
                        row[5], //фио автора 
                        int.Parse(row[6]), //внешний ключ выдовавшего
                        row[7], //фио выдовавшего
                        DateTime.Parse(row[8]), //дата выдачи
                        fkS, //внешний ключ принимавшего
                        nameS, //фио принимавшего
                        dateS); //дата сдачи
            }
        }

        public List<ComboBoxItems> GetDataComboBoxs() => ComboBoxOnForm;
    }
}
