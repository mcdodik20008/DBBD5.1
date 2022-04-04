using System.Windows.Forms;

namespace BD_ITOG
{
    //патерн команда, с добавлением отсебятины в виде сохранения в скул
    interface ICommand
    {
        void Command(DataGridView x);
        void UnCommand(DataGridView x);
        void SqveInSql();
    }
}