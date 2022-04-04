using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace BD_ITOG
{
    class Abonement : DefultForm
    {     
        List<string> aboutReader;
        static HeadDataGrid inBaseConstructor = EAbonement.HeadDataGrid;

        public Abonement(int currentId, List<string> aboutCurrent) : base(inBaseConstructor)
        {
            this.currentId = currentId;
            commandMaxId = "SELECT MAX(id_zap) From InSy.dbo.Subscription";
            for (int i = 0; i < 3; i++)
            {
                TextAndComboBox.Add(InicialItem.ComboBox());
                TextAndComboBox.Add(InicialItem.TextBox(DockStyle.None, true));
            }
            dataGrid.Location = new Point(10, 70);
            InicializeChangeCB();
            this.OnSizeChanged(EventArgs.Empty);
        }

        internal override void Form_Load(object sender, EventArgs e)
        {
            DataPreparation();

            Labels.Add(new Label() { Text = "ФИО:", Width = 40 });
            Labels.Add(new Label() { Text = aboutReader[1], Width = 100 }); // фио

            Labels.Add(new Label() { Text = "Дата рождения:", Width = 100 });
            Labels.Add(new Label() { Text = aboutReader[2], Width = 70 }); // дата р
            Labels.Add(new Label() { Text = "Номер телефона:", Width = 100 });
            Labels.Add(new Label() { Text = aboutReader[3], Width = 80 }); // телефон номер 

            Labels.Add(new Label() { Text = "Домашний адрес:", Width = 100 });
            Labels.Add(new Label() { Text = aboutReader[4], Width = 100 }); // адрес

            Labels.Add(new Label() { Text = "Направление:", Width = 80 });
            Labels.Add(new Label() { Text = aboutReader[6], Width = 50 }); // направление

            AddControls();
        }

        private void DataPreparation()
        {
            //тут без линку
            //информация о выбранном пользователе
            string getInfo = @"SELECT id_Lk, fullName, dateBirth, phoneNumber, homeAdres, fk_dir, name  
                                from InSy.dbo.LibraryCard
                                join InSy.dbo.Directions on fk_dir = id_napr " + 
                                $"where id_Lk = {currentId}";
            var aboutReader = SQL.ReadSql(getInfo);
            this.aboutReader = aboutReader[0];

            //почти вся информация, кроме библиотекоря, который принял книгу
            string getMoreInfo = @"SELECT id_zap, fk_book, bookName, id_Author, fullnameAuthor, fk_whoV, fullName, dateV, fk_whoS, dateS
	                            From InSy.dbo.Subscription
	                            JOIN InSy.dbo.Book ON id_book = fk_book
	                            JOIN InSy.dbo.Author ON id_book = fk_book and fk_author = id_Author
	                            JOIN InSy.dbo.Librarian ON fk_whoV = id_Librarian " +
                                $"where fk_libCard = {currentId}";

            //библиотекарь, который принял книгу
            string getWhoS = @"SELECT fk_whoS, fullName
                            FROM InSy.dbo.Subscription
                            JOIN InSy.dbo.Librarian ON fk_whoS = id_Librarian " +
                            $"where fk_libCard = {currentId}";
            
            //лучше не вчитываться в то, что ниже о_0
            var tS = SQL.ReadSql(getWhoS);
            var pTable = SQL.ReadSql(getMoreInfo);
            var table = new List<EAbonement>();
            for (int i = 0; i < pTable.Count(); i++)
            {
                int? fkS = null;
                DateTime? dateS = null;
                string nameS = null;

                if (tS.Count > i && tS[i][0] != null && pTable[i][9] != "")
                {
                    fkS = int.Parse(tS[i][0]);
                    dateS = DateTime.Parse(pTable[i][9]);
                    nameS = tS[i][1];
                }

                table.Add(new EAbonement
                   (
                        int.Parse(pTable[i][0]), //первичный ключ
                        currentId, //выбранный чел
                        int.Parse(pTable[i][1]), //внешний ключ книги
                        pTable[i][2], //название книги
                        int.Parse(pTable[i][3]), //внешний ключ автора
                        pTable[i][4], //фио автора 
                        int.Parse(pTable[i][5]), //внешний ключ выдовавшего
                        pTable[i][6], //фио выдовавшего
                        DateTime.Parse(pTable[i][7]), //дата выдачи
                        fkS, //внешний ключ принимавшего
                        nameS, //фио принимавшего
                        dateS) //дата сдачи
                    );
            }

            foreach (var item in table)
                itemsInDateGrid.Add(item);
            
            FillingDatagrid(table);
            FillingComboBox(forSave);
        }

        internal override void FillingComboBox(List<List<IComboBoxItem>> xx)
        {
            int counter = 0;
            foreach (var item in TextAndComboBox)
            {
                if (item is ComboBox comboBox)
                {
                    switch (counter)
                    {
                        case 0:
                            xx.Add(new List<IComboBoxItem>());
                            FillBooksAuthors(comboBox, xx[counter++]);
                            break;
                        case 1:
                            xx.Add(new List<IComboBoxItem>());
                            FillLibrarian(comboBox, xx[counter++]);
                            break;
                        case 2:
                            xx.Add(new List<IComboBoxItem>());
                            FillLibrarian(comboBox, xx[counter++]);
                            break;
                    }
                }
            }
        }

        private void FillBooksAuthors(ComboBox comboBox, List<IComboBoxItem> comboBoxItems)
        {
            string command = @"SELECT id_book, bookName, fk_author, fullNameAuthor
	                            From InSy.dbo.Book
	                            JOIN InSy.dbo.Author ON  fk_author = id_Author";
            foreach (var item in SQL.ReadSql(command))
            {
                comboBoxItems.Add(new ComboBoxItemBook(currentId, int.Parse(item[0]), item[1], int.Parse(item[2]), item[3]));
                comboBox.Items.Add(item[1]);
            }
        }

        private void FillLibrarian(ComboBox comboBox, List<IComboBoxItem> comboBoxItems)
        {
            string command = @"SELECT id_Librarian, fullName
                                From InSy.dbo.Librarian";
            comboBoxItems.Add(new ComboBoxItemLibrarian(null, null));
            comboBox.Items.Add("");
            foreach (var item in SQL.ReadSql(command))
            {
                comboBoxItems.Add(new ComboBoxItemLibrarian(int.Parse(item[0]), item[1]));
                comboBox.Items.Add(item[1]);
            }
        }

        internal void InicializeChangeCB()
        {
            int n = 0;
            for (int i = 0; i < TextAndComboBox.Count(); i++)
            {
                if (TextAndComboBox[i] is ComboBox cB)
                {
                    if (n == 0)
                    {
                        cB.SelectedIndexChanged += (sender, Empty) =>
                        {
                            if (TextAndComboBox[1] is TextBox tB
                                && forSave[0][cB.SelectedIndex] is ComboBoxItemBook cBB)
                            {
                                tB.Text = cBB.NameAut;
                            }
                        }; 
                    }

                    if (n == 1)
                    {
                        cB.SelectedIndexChanged += (sender, Empty) =>
                        {
                            if (TextAndComboBox[3] is TextBox tB
                                && forSave[1][cB.SelectedIndex] is ComboBoxItemLibrarian cBB)
                            {
                                if (tB.Text == "" || tB.Text == null)
                                {
                                    tB.Text = DateTime.Now.ToString().Substring(0, 10);
                                }
                            }
                        };
                    }

                    if (n == 2)
                    {
                        cB.SelectedIndexChanged += (sender, Empty) =>
                        {
                            if (TextAndComboBox[5] is TextBox tB
                                && forSave[2][cB.SelectedIndex] is ComboBoxItemLibrarian cBB)
                            {
                                if ((cB.Text != "" || tB.Text == null) && (tB.Text == "" || tB.Text == null))
                                {
                                    tB.Text = DateTime.Now.ToString().Substring(0, 10);
                                }
                            }
                        };
                    }
                    n++;
                }
            }
        }

        internal override IEitem NewIEitem()
        {
            var outt = GetValuesFromTextAndComboBox();
            int? fk = null;
            if (outt[9] != null && outt[9] != "") fk = int.Parse(outt[9]);

            DateTime? dT = null;
            if (outt[9] != null && outt[9] != "") dT = DateTime.Parse(outt[11]);
            return new EAbonement(int.Parse(outt[0]), int.Parse(outt[1]), int.Parse(outt[2]), outt[3], 
                int.Parse(outt[4]), outt[5], int.Parse(outt[6]), outt[7], DateTime.Parse(outt[8]), fk, outt[10], dT);
        }

        internal override bool IsInputDontHaveErrors(List<Control> list) => true;
    }
}
