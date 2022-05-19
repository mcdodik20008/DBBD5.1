using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DBBD51
{
    public class Readers : DefultForm
    {
        public Readers() : base(EReaders.HeadDataGrid, "Читатели")
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            for (int i = 0; i < 4; i++)
                TextAndComboBox.Add(InicialItem.TextBox());
            TextAndComboBox.Add(new ComboBox());
            var t2 = new TextBox() { Enabled = false };
            var t1 = new TextBox() { Enabled = false };

            TextAndComboBox.Add(t1);
            TextAndComboBox.Add(t2);
            var button1 = InicialItem.Button("Поиск");
            button1.Click += (sender, args) => 
            { 
                var sr = new Search(DataSourse, dataGrid).ShowDialog(); 
            };
            var button2 = InicialItem.Button("Подробнее");
            button2.Click += (sender, args) => { new Abonement(currentId, aboutCurrent).ShowDialog(); dataGrid.FillingDatagrid(DataSourse.Update().GetRows()); };
            Buttons.Add(button1);
            Buttons.Add(button2);
            AddControls();
        }

        internal override void Form_Load(object sender, EventArgs e)
        {
            dataGrid.Location = new Point(10, 10);
            DataSourse = new DSReaders(TextAndComboBox);
            dataGrid.FillingDatagrid(DataSourse.GetRows());
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
