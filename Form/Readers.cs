using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BD_ITOG
{
    public class Readers : DefultForm
    {
        public Readers() : base(EReaders.HeadDataGrid)
        {
            for (int i = 0; i < 4; i++)
                TextAndComboBox.Add(InicialItem.TextBox());
            TextAndComboBox.Add(new ComboBox());
            var x1 = InicialItem.TextBox();
            x1.Enabled = false;
            TextAndComboBox.Add(x1);
            var x2 = InicialItem.TextBox();
            x2.Enabled = false;
            TextAndComboBox.Add(x2);
            commandMaxId = "SELECT Max(id_Lk) From InSy.dbo.LibraryCard";
            var button = InicialItem.Button("Подробнее");
            button.Click += (sender, args) => { new Abonement(currentId, aboutCurrent).ShowDialog(); Form_Load(this, EventArgs.Empty); };
            Buttons.Add(button);
            AddControls();
        }

        internal override void Form_Load(object sender, EventArgs e)
        {
            dataGrid.Location = new Point(10, 10);

            string readers = @"SELECT id_Lk, fullName, dateBirth, phoneNumber, homeAdres, fk_dir, name, 
		                                (select count(fk_whoV) from InSy.dbo.Subscription where id_Lk = fk_libCard), 
			                                   (select count(fk_whoV) - count(fk_whoS)  from InSy.dbo.Subscription where id_Lk = fk_libCard)
                                    from InSy.dbo.LibraryCard
                                    join InSy.dbo.Directions on fk_dir = id_napr";
            itemsInDateGrid = SQL
                .ReadSql(readers)
                .Select(x => (IEitem)
                new EReaders(int.Parse(x[0]), x[1], DateTime.Parse(x[2]), x[3], x[4], int.Parse(x[5]), x[6], int.Parse(x[7]), int.Parse(x[8])))
                .ToList();
            dataGrid.Rows.Clear();
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
                    FillBooksDirections(comboBox, xx[0]);
                }
            }
        }

        private void FillBooksDirections(ComboBox comboBox, List<IComboBoxItem> comboBoxItems)
        {
            string command = @"SELECT id_napr, name
                                From InSy.dbo.Directions";
            comboBox.Items.Clear();
            foreach (var item in SQL.ReadSql(command))
            {
                comboBoxItems.Add(new ComboBoxItemAuthor(int.Parse(item[0]), item[1]));
                comboBox.Items.Add(item[1]);
            }
        }

        internal override IEitem NewIEitem()
        {
            var outt = GetValuesFromTextAndComboBox();
            if (outt.Count == 0) return new EReaders();
            return new EReaders(int.Parse(outt[0]), outt[1], DateTime.Parse(outt[2]), outt[3], outt[4], int.Parse(outt[5]), outt[6], int.Parse(outt[7]), int.Parse(outt[8]));
        }

        internal override bool IsInputDontHaveErrors(List<Control> list)
        {
            List<Tuple<bool, string>> tupl = new List<Tuple<bool, string>>();

            if (list[0].Text.Split().Length != 3)
                tupl.Add(Tuple.Create(false, "Введите ФИО корректно"));

            if (!DateTime.TryParse(list[1].Text, out DateTime dT))
                tupl.Add(Tuple.Create(false, "Не правильно ввели дату рождения"));

            if (list[2].Text.Length != 11 || !long.TryParse(list[2].Text, out long l))
                tupl.Add(Tuple.Create(false, "Не правильно ввели телефон"));

            if (!(
                (list[3].Text.Split(' ', '/').Length == 2 || list[3].Text.Split(' ', '/').Length == 3) 
                    && int.TryParse(list[3].Text.Split(' ', '/')[1], out int i))
                    )
                tupl.Add(Tuple.Create(false, "Введите адрес корректно"));

            foreach (var t in tupl)
                MessageBox.Show(t.Item2, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return tupl.Count == 0;
        }
    }
}
