﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DBBD51
{
    public class Directions : DefultForm
    {
        static HeadDataGrid inBaseConstructor = EDirections.HeadDataGrid;
        public Directions() : base(inBaseConstructor)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            TextAndComboBox.Add(InicialItem.TextBox());
            AddControls();
        }

        internal override void Form_Load(object sender, EventArgs e)
        {
            DataSourse = new DSDirections();
            FillingDatagrid(DataSourse.GetRows());
        }

        internal override void FillingComboBox(List<ComboBoxItems> xx) { }

        internal override IEitem NewIEitem()
        {
            var outt = GetValuesFromTextAndComboBox();
            return new EDirections(int.Parse(outt[0]), outt[1]);
        }

        internal override bool IsInputDontHaveErrors(List<Control> list) => true;
    }
}
