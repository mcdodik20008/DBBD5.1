using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace DBBD51
{
    public class Search : Form
    {
        private IDataSourse dataSourse;
        private DataGridView dG;
        private Button button1;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private Label label1;
        private Label label2;
        private Label label3;
        
        public Search(IDataSourse dataSourse, DataGridView dG)
        {
            InitializeComponent();
            this.dG = dG;
            this.dataSourse = dataSourse.Update();
            StartPosition = FormStartPosition.CenterScreen;
            MaximumSize = new Size(300, 220);
            MinimumSize = MaximumSize;
            button1.Click += (sender, args) =>
            {
                dG.Rows.Clear();
                dG.FillingDatagrid(GetDataWhithFilter());
                Close();
            };
        }

        public IEnumerable<IEitem> GetDataWhithFilter()
        {
            var sourse = dataSourse.GetRows();
            if (textBox1.Text != "Не включено в поиск")
                sourse.Where(x => x.GetListValForDataGrid()[0] == textBox1.Text);
            if (textBox2.Text != "Не включено в поиск")
                sourse.Where(x => x.GetListValForDataGrid()[1] == textBox2.Text);
            if (textBox3.Text != "Не включено в поиск")
                sourse.Where(x => x.GetListValForDataGrid()[6] == textBox3.Text);
            return sourse;
        }

        public void InitializeComponent()
        {
            button1 = new Button()
            {
                Location = new Point(12, 117),
                Size = new Size(260, 52),
                Text = "Найти",
            };
            textBox1 = new TextBox()
            {
                Location = new Point(121, 18),
                Size = new Size(151, 23),
                Text = "Не включено в поиск"
            };
            textBox2 = new TextBox()
            {
                Location = new Point(121, 47),
                Size = new Size(151, 23),
                Text = "Не включено в поиск"
            };
            textBox3 = new TextBox()
            {
                Location = new Point(121, 81),
                Size = new Size(151, 23),
                Text = "Не включено в поиск"
            };
            label1 = new Label()
            {
                Location = new Point(12, 26),
                Size = new Size(95, 15),
                Text = "Номер клиента:"
            };
            label2 = new Label()
            {
                Location = new Point(70, 55),
                Size = new Size(37, 15),
                Text = "Номер клиента:"
            };
            label3 = new Label()
            {
                Location = new Point(23, 84),
                Size = new Size(84, 15),
                Text = "Направление:"
            };
            SuspendLayout();

            //contorls
            {
                ClientSize = new Size(300, 220);
                Controls.Add(this.label3);
                Controls.Add(this.label2);
                Controls.Add(this.label1);
                Controls.Add(this.textBox3);
                Controls.Add(this.textBox2);
                Controls.Add(this.textBox1);
                Controls.Add(this.button1);
                Name = "Search";
                ResumeLayout(false);
                PerformLayout();
            }
        }
    }
}
