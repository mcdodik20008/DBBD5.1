using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BD_ITOG
{
    public class Book : DefultForm
    {
        static HeadDataGrid inBaseConstructor = EBook.HeadDataGrid;
        public Book() : base(inBaseConstructor)
        {
            TextAndComboBox.Add(InicialItem.TextBox());
            TextAndComboBox.Add(InicialItem.TextBox());
            TextAndComboBox.Add(new ComboBox());
            commandMaxId = "SELECT MAx(id_book) From InSy.dbo.Book";
            AddControls();
        }

        internal override void Form_Load(object sender, EventArgs e)
        {
            string book = @"select id_book, bookName, dateRelease, fk_author, fullNameAuthor
                            from InSy.dbo.Book
                            join InSy.dbo.Author ON  fk_author = id_Author";
            var itemsInDateGrid = SQL
                .ReadSql(book)
                .Select(x => new EBook(int.Parse(x[0]), x[1], new DateTime(int.Parse(x[2]), 1, 1), int.Parse(x[3]), x[4]))
                .ToList();
            FillingDatagrid(itemsInDateGrid);
            FillingComboBox(forSave);
        }

        internal override void FillingComboBox(List<List<IComboBoxItem>> xx)
        {
            foreach (var item in TextAndComboBox)
            {
                if (item is ComboBox comboBox)
                {
                    xx.Add(new List<IComboBoxItem>());
                    FillBooksAuthors(comboBox, xx[0]);
                }
            }
        }

        private void FillBooksAuthors(ComboBox comboBox, List<IComboBoxItem> comboBoxItems)
        {
            string command = @"SELECT id_Author, fullNameAuthor
                                From InSy.dbo.Author";
            foreach (var item in SQL.ReadSql(command))
            {
                comboBoxItems.Add(new ComboBoxItemAuthor(int.Parse(item[0]), item[1]));
                comboBox.Items.Add(item[1]);
            }
        }

        internal override IEitem NewIEitem()
        {
            var outt = GetValuesFromTextAndComboBox();
            return new EBook(int.Parse(outt[0]), outt[1], DateTime.Parse(outt[2]), int.Parse(outt[3]), outt[4]);
        }

        internal override bool IsInputDontHaveErrors(List<Control> list)
        {
            List<Tuple<bool, string>> tupl = new List<Tuple<bool, string>>();

            if (!DateTime.TryParse(list[1].Text, out DateTime dT))
                tupl.Add(Tuple.Create(false, "Не правильно ввели дату выпуска"));

            foreach (var t in tupl)
                MessageBox.Show(t.Item2, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return tupl.Count == 0;
        }
    }
}
