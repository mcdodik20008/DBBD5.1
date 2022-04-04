using System.Collections.Generic;

namespace BD_ITOG
{
    //якобы сущности Базы данных с "Удобными" методами для получения данных
    public interface IEitem
    { 
        string GetNameTable();
        HeadDataGrid GetHeadDataGrid();
        List<string> GetListValForDataGrid();
        string GetValueForSql();
        List<string> GetListValForSql();
        bool IsGood();
    }
}
