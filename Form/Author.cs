using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BD_ITOG
{
    public class Author : DefultForm
    {
        static HeadDataGrid inBaseConstructor = EAuthor.HeadDataGrid;
        public Author() : base(inBaseConstructor)
        {
            TextAndComboBox.Add(InicialItem.TextBox());
            TextAndComboBox.Add(InicialItem.TextBox());
            commandMaxId = "SELECT MAx(id_Author) From InSy.dbo.Author";
            AddControls();
        }

        internal override void Form_Load(object sender, EventArgs e)
        {
            string book = @"select *
                              from InSy.dbo.Author";

            itemsInDateGrid = SQL
                .ReadSql(book)
                .Select(x => (IEitem)new EAuthor(int.Parse(x[0]), x[1], DateTime.Parse(x[2])))
                .ToList();
            FillingDatagrid(itemsInDateGrid);
        }

        internal override void FillingComboBox(List<List<IComboBoxItem>> xx)
        {
            return;
        }
        internal override IEitem NewIEitem()
        {
            var outt = GetValuesFromTextAndComboBox();
            return new EAuthor(int.Parse(outt[0]), outt[1], DateTime.Parse(outt[2]));
        }

        internal override bool IsInputDontHaveErrors(List<Control> list)
        {
            List<Tuple<bool, string>> tupl = new List<Tuple<bool, string>>();

            if (!DateTime.TryParse(list[1].Text, out DateTime dT))
                tupl.Add(Tuple.Create(false, "Не правильно ввели дату рождения"));

            foreach (var t in tupl)
                MessageBox.Show(t.Item2, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return tupl.Count == 0;
        }
    }
}
