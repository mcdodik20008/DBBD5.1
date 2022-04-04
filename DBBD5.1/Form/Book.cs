﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DBBD51
{
    public class Book : DefultForm
    {
        static HeadDataGrid inBaseConstructor = EBook.HeadDataGrid;
        public Book() : base(inBaseConstructor)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            TextAndComboBox.Add(InicialItem.TextBox());
            TextAndComboBox.Add(InicialItem.TextBox());
            TextAndComboBox.Add(new ComboBox());
            AddControls();
        }

        internal override void Form_Load(object sender, EventArgs e)
        {
            DataSourse = new DSBook();
            FillingDatagrid(DataSourse.GetRows());
            FillingComboBox(forSave);
        }

        internal override void FillingComboBox(List<ComboBoxItems> xx)
        {
            xx.Add(new ComboBoxItems());
            (TextAndComboBox[2] as ComboBox).FillBooksAuthors(xx[0]);
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
