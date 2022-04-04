using System.Windows.Forms;

namespace BD_ITOG
{
    public class NewLine : ICommand
    {
        int index;

        public NewLine() { }

        public void Command(DataGridView x)
        {
            x.Rows.Add();
            index = x.Rows.Count - 1;
        }

        public void UnCommand(DataGridView x) =>
            x.Rows.RemoveAt(index);
        

        public void SqveInSql() { }
    }
}
