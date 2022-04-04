using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BD_ITOG
{
    public class Directions : DefultForm
    {
        static HeadDataGrid inBaseConstructor = EDirections.HeadDataGrid;
        public Directions() : base(inBaseConstructor)
        {
            TextAndComboBox.Add(InicialItem.TextBox());
            commandMaxId = "SELECT MAx(id_napr) + 1 From InSy.dbo.Directions";
            AddControls();
        }

        internal override void Form_Load(object sender, EventArgs e)
        {
            itemsInDateGrid = SQL
                .ReadSql(@"select * from InSy.dbo.Directions")
                .Select(x => (IEitem)new EDirections(int.Parse(x[0]), x[1]))
                .ToList();
            FillingDatagrid(itemsInDateGrid);
        }

        internal override void FillingComboBox(List<List<IComboBoxItem>> xx) { }

        internal override IEitem NewIEitem()
        {
            var outt = GetValuesFromTextAndComboBox();
            return new EDirections(int.Parse(outt[0]), outt[1]);
        }

        internal override bool IsInputDontHaveErrors(List<Control> list) => true;
    }
}
