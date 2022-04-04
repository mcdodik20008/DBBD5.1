using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BD_ITOG
{
    public class LibrarianForm : DefultForm
    {
        static HeadDataGrid inBaseConstructor = ELibrarian.HeadDataGrid;
        public LibrarianForm() : base(inBaseConstructor)
        {
            TextAndComboBox.Add(InicialItem.TextBox());
            TextAndComboBox.Add(InicialItem.TextBox());

            commandMaxId = "SELECT MAx(id_Librarian) From InSy.dbo.Librarian";

            AddControls();
        }

        internal override void Form_Load(object sender, EventArgs e)
        {
            itemsInDateGrid = SQL
                .ReadSql(@"select * from InSy.dbo.Librarian")
                .Select(x => (IEitem)new ELibrarian(int.Parse(x[0]), x[1], DateTime.Parse(x[2])))
                .ToList();
            FillingDatagrid(itemsInDateGrid);
        }

        internal override void FillingComboBox(List<List<IComboBoxItem>> xx) { }

        internal override IEitem NewIEitem()
        {
            var outt = GetValuesFromTextAndComboBox();
            return new ELibrarian(int.Parse(outt[0]), outt[1], DateTime.Parse(outt[2]));
        }

        internal override bool IsInputDontHaveErrors(List<Control> list)
        {
            List<Tuple<bool, string>> tupl = new List<Tuple<bool, string>>();

            if (list[0].Text.Split().Length != 3)
                tupl.Add(Tuple.Create(false, "Введите ФИО корректно"));

            if (!DateTime.TryParse(list[1].Text, out DateTime dT))
                tupl.Add(Tuple.Create(false, "Не правильно ввели дату рождения"));

            foreach (var t in tupl)
                MessageBox.Show(t.Item2, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return tupl.Count == 0;
        }
    }
}
