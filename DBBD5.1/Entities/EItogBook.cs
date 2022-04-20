using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBBD51
{
    class EItogBook : IEitem
    {
        string bookName;
        string dateTime;
        string fullnameAuthor;
        int count;

        public EItogBook(string bookName, string dateTime, string fullnameAuthor, int count)
        {
            this.bookName = bookName;
            this.dateTime = dateTime;
            this.fullnameAuthor = fullnameAuthor;
            this.count = count;
        }

        public List<string> GetListValForDataGrid() => new List<string>()
                    { bookName, dateTime, fullnameAuthor, count.ToString() };

        public HeadDataGrid GetHeadDataGrid() => throw new NotImplementedException();

        public List<string> GetListValForSql() => new List<string>();

        public string GetNameTable() => "";

        public string GetValueForSql() => "";

        public bool IsGood() => true;
    }
}
