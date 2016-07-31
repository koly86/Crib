using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure.Pluralization;
using System.Data.SQLite;
using System.Linq;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ErrorWriteLog;

namespace WPFCrib
{


    internal class KeyWord
    {
        private SQLiteCommand _cmd;
        private DataSet _dsSet;
       // private SQLiteDataAdapter _adapter;
        private readonly SqlCom _sqlCom = new SqlCom();
        private DataTable _dataTable;
        protected internal Button[] btn;
        protected internal TextBlock[] TxtBlocks;
        protected internal Grid Grid;
        protected internal ListBox List;
        protected internal TextBlock TxtDescript;



        private class SqlCom
        {
            protected internal readonly string selCAT =
                " SELECT * FROM tblKWСATEGORY WHERE LANGID = 1  ;  SELECT * FROM tblKWСATEGORY WHERE LANGID = 2 ; ";

            protected internal readonly string selSUBCAT =
                "select KEYWRDSUBCATID , NAMESUBCAT FROM tblKWSUBCATEGORY WHERE KEYWRDCATID = {0} ";

            protected internal readonly string selKeyWordfrnCat =
                "select KEYWRDID , WORD FROM tblKEYWORDS where tblKEYWORDS.KEYWRDCATID = {0} ";

            protected internal readonly string InsertCat =
                " INSERT INTO tblKWСATEGORY ( LANGID , NAMECAT) VALUES( @LANGID , @NAMECAT ) ";

            protected internal readonly string InsertSubCat =
                "INSERT INTO tblKWSUBCATEGORY ( LANGID , KEYWRDCATID,  NAMESUBCAT ) VALUES ( @LANGID , @KEYWRDCATID , @NAMESUBCAT )";

            protected internal readonly string InsertLang =
                " INSERT INTO tblLANGUAGE ( LANGUAGE ) VALUES ( @LANGUAGE )  ";

            protected internal readonly string selCategory = 
                " select KEYWRDCATID , NAMECAT from tblKWСATEGORY WHERE LANGID = {0} ";

            protected internal readonly string selKeyWordfrmSubCat =
                "select WORD , KEYWRDID FROM tblKEYWORDS where tblKEYWORDS.KEYWRDSUBCATID = {0} ";
           

            protected internal readonly string selDescriptKeyWrd =
                " select KEYWRDESCRIPTION FROM tblKEYWORDS where tblKEYWORDS.KEYWRDID = {0} ";

            protected internal readonly string InsertKEYWORDS =
                "INSERT INTO tblKEYWORDS ( KEYWRDCATID, KEYWRDSUBCATID, WORD, KEYWRDESCRIPTION ) VALUES ( @KEYWRDCATID, @KEYWRDSUBCATID ,@WORD, @KEYWRDESCRIPTION) ";
        }


        internal DataTable ShowCat(Lang lang)
        {
            using (_cmd = new SQLiteCommand())
            {
                try
                {
                    DataBase.Con.Open();
                    var _adapter = new SQLiteDataAdapter(_sqlCom.selCAT, DataBase.Con);
                    var DsSet = new DataSet();
                    _adapter.Fill(DsSet);
                    switch (lang)
                    {
                        case Lang.Russian:
                            _dataTable = DsSet.Tables[0];
                            break;
                        case Lang.English:
                            _dataTable = DsSet.Tables[1];
                            break;
                        default:
                            break;
                    }
                    return _dataTable;
                }
                catch (SQLiteException ex)
                {
                    ErrorWriter.WriteToLog(ex.Message + " " + ex.ErrorCode + "" + DateTime.Now);
                }
                finally
                {
                    DataBase.Con.Close();
                }
            }
            return _dataTable;
        }

        internal void FillCat()
        {

            #region FillCat

            if (SelIsNull(_sqlCom.selCAT)) return;
            FillLang(); //заполняем справочник языков




            byte count = 0; //счетчик категорий и подставляется номер категории в таблицу подкатегории




            var strTypeE = new string[2] {"Value Types", "Reference Types"};
            var strModifiE = new string[1] {"Access Modifiers"};
            var strStatemenE = new string[5]
            {
                "Selection Statements", "Iteration Statements", "Jump Statements",
                "Exception Handling Statements", "Checked and Unchecked"
            };




            var strTypeR = new string[2] { "Типы значений", "Ссылочные типы" };
            var strModifiR = new string[1] {"Модификаторы доступа"};
            var strStatemenR = new string[5]
            {
                "Инструкции выбора", "Операторы итерации", "Операторы перехода", "оп-ры Обработки исключений",
                "Checked и Unchecked"
            };

            var canEng = new string[11]
            {
                "Types", "Modifiers", "Statement Keywords", "Method Parameters", "Namespace Keywords",
                "Operator Keywords", "Conversion Keywords",
                "Access Keywords", "Literal Keywords", "Contextual Keywords", "Query Keywords(LINQ)"
            };

            var canRus = new string[11]
            {
                "Типы", "Модификаторы", "Ключевые слова операторов1", "Параметры методов", "Слова для простр.имен",
                "Ключевые слова операторов2",
                "Слова преобразований", "Слова доступа", "Буквенные ключевые слова", "Контекстные ключевые слова",
                "Ключевые слова LINQ"
            };

            var myListsE = new List<string>[11]
            {
                new List<string>(strTypeE), new List<string>(strModifiE), new List<string>(strStatemenE),
                new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(),
                new List<string>(), new List<string>(), new List<string>()
            };


            var myListsR = new List<string>[11]
            {
                new List<string>(strTypeR), new List<string>(strModifiR), new List<string>(strStatemenR),
                new List<string>(), new List<string>(), new List<string>(), new List<string>(), new List<string>(),
                new List<string>(), new List<string>(), new List<string>()
            };

            for (var i = 0; i < canRus.Length; i++)
            {
                using (_cmd = new SQLiteCommand(_sqlCom.InsertCat, DataBase.Con))
                {
                    DataBase.Con.Open();
                    try
                    {
                        _cmd.Parameters.Add("@LANGID", DbType.Int16);
                        _cmd.Parameters.Add("@NAMECAT", DbType.String);

                        _cmd.Parameters["@NAMECAT"].Value = canRus[i];
                        _cmd.Parameters["@LANGID"].Value = Lang.Russian;

                        _cmd.ExecuteNonQuery();
                        count++;
                    }
                    catch (SQLiteException ex)
                    {
                        ErrorWriter.WriteToLog(ex.Message + " " + ex.Data + " " + DateTime.Now);
                    }
                    finally
                    {
                        DataBase.Con.Close();
                    }
                }
                for (uint j = 0; j < myListsR[i].Count; j++)
                {
                    using (_cmd = new SQLiteCommand(_sqlCom.InsertSubCat, DataBase.Con))
                    {
                        DataBase.Con.Open();
                        try
                        {
                            _cmd.Parameters.Add("@LANGID", DbType.Int16);
                            _cmd.Parameters.Add("@KEYWRDCATID", DbType.Byte);
                            _cmd.Parameters.Add("@NAMESUBCAT", DbType.String);


                            _cmd.Parameters["@LANGID"].Value = Lang.Russian;
                            _cmd.Parameters["@KEYWRDCATID"].Value = count;
                            _cmd.Parameters["@NAMESUBCAT"].Value = myListsR[i][j];

                            _cmd.ExecuteNonQuery();
                        }
                        catch (SQLiteException ex)
                        {
                            ErrorWriter.WriteToLog(ex.Message + " " + ex.Data + " " + DateTime.Now);
                        }
                        finally
                        {
                            DataBase.Con.Close();
                        }
                    }
                }
            }


            for (var i = 0; i < canEng.Length; i++)
            {
                using (_cmd = new SQLiteCommand(_sqlCom.InsertCat, DataBase.Con))
                {
                    DataBase.Con.Open();
                    try
                    {
                        _cmd.Parameters.Add("@LANGID", DbType.Int16);
                        _cmd.Parameters.Add("@NAMECAT", DbType.String);

                        _cmd.Parameters["@NAMECAT"].Value = canEng[i];
                        _cmd.Parameters["@LANGID"].Value = Lang.English;

                        _cmd.ExecuteNonQuery();
                        count++;
                    }
                    catch (SQLiteException ex)
                    {
                        ErrorWriter.WriteToLog(ex.Message + " " + ex.Data + " " + DateTime.Now);
                    }
                    finally
                    {
                        DataBase.Con.Close();
                    }
                }
                for (uint j = 0; j < myListsE[i].Count; j++)
                {
                    using (_cmd = new SQLiteCommand(_sqlCom.InsertSubCat, DataBase.Con))
                    {
                        DataBase.Con.Open();
                        try
                        {
                            _cmd.Parameters.Add("@LANGID", DbType.Int16);
                            _cmd.Parameters.Add("@KEYWRDCATID", DbType.Byte);
                            _cmd.Parameters.Add("@NAMESUBCAT", DbType.String);


                            _cmd.Parameters["@LANGID"].Value = Lang.English;
                            _cmd.Parameters["@KEYWRDCATID"].Value = count;
                            _cmd.Parameters["@NAMESUBCAT"].Value = myListsE[i][j];

                            _cmd.ExecuteNonQuery();
                        }
                        catch (SQLiteException ex)
                        {
                            ErrorWriter.WriteToLog(ex.Message + " " + ex.Data + " " + DateTime.Now);
                        }
                        finally
                        {
                            DataBase.Con.Close();
                        }
                    }
                }
            }


            #endregion
        }

        private void FillLang()
        {
            #region FillLang

            string[] lang = new string[2] {"Русский", "English"};
            using (_cmd = new SQLiteCommand(_sqlCom.InsertLang, DataBase.Con))
            {
                try
                {
                    DataBase.Con.Open();
                    foreach (var lan in lang)
                    {
                        _cmd.Parameters.Add("@LANGUAGE", DbType.String);
                        _cmd.Parameters["@LANGUAGE"].Value = lan;
                        _cmd.ExecuteNonQuery();
                    }
                }
                catch (SQLiteException ex)
                {
                    ErrorWriter.WriteToLog(ex.Message + " " + ex.Data + " " + DateTime.Now);
                }
                finally
                {
                    DataBase.Con.Close();
                }
            }

            #endregion
        }

        private bool SelIsNull(string select)
        {
            using (_cmd = new SQLiteCommand(select, DataBase.Con))
            {
                try
                {
                    DataBase.Con.Open();
                    var propid = _cmd.ExecuteScalar();
                    return propid is long ? true : false;
                }
                catch (SQLiteException ex)
                {
                    ErrorWriter.WriteToLog(ex.Message + " " + ex.Data + " " + DateTime.Now);
                }
                finally
                {
                    DataBase.Con.Close();
                }
                return true;
            }
        }

        internal DataTable ShowSubCatTable(Lang lang, long catId)
        {
            using (_cmd = new SQLiteCommand())
            {
                try
                {
                    DataBase.Con.Open();
                    var adapter = new SQLiteDataAdapter(string.Format(_sqlCom.selSUBCAT, catId), DataBase.Con);
                    var DsSet = new DataSet();
                    adapter.Fill(DsSet);
                    return DsSet.Tables[0];
                    
                }
                catch (SQLiteException ex)
                {
                    ErrorWriter.WriteToLog(ex.Message + " " + ex.ErrorCode + "" + DateTime.Now);
                }
                finally
                {
                    DataBase.Con.Close();
                }
            }
            return _dataTable;
        }

        internal void InsKeyWord(Dictionary<KeyWrd, string> data)
        {
            using (_cmd = new SQLiteCommand(_sqlCom.InsertKEYWORDS, DataBase.Con))
            {
                _cmd.Parameters.Add("@KEYWRDCATID", DbType.Int64);
                _cmd.Parameters.Add("@WORD", DbType.String, 50);
                _cmd.Parameters.Add("@KEYWRDESCRIPTION", DbType.String, 250);
                _cmd.Parameters.Add("@KEYWRDSUBCATID", DbType.Byte);



                _cmd.Parameters["@KEYWRDCATID"].Value = data[KeyWrd.CatID];
                _cmd.Parameters["@WORD"].Value = data[KeyWrd.KeywrdName];
                _cmd.Parameters["@KEYWRDESCRIPTION"].Value = data[KeyWrd.KeyDscr];
                _cmd.Parameters["@KEYWRDSUBCATID"].Value = data[KeyWrd.SubCatID];

                try
                {
                    DataBase.Con.Open();
                    var keyWrdId = _cmd.ExecuteScalar();
                    var KeyWrdID = keyWrdId is long ? (long) keyWrdId : 0; //получаем ID нового класса
                    DataBase.Con.Close();

                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message);
                    ErrorWriter.WriteToLog(ex.Message + " " + ex.ErrorCode + "" + DateTime.Now);
                }
                finally
                {
                    DataBase.Con.Close();
                }
            }
        }

        internal void FillbtnCat(Lang lang) //Заполняется строка категорий
        {
          
            switch (lang)
            {
                   case Lang.Russian:
                   _dsSet = GetDataSet(string.Format(_sqlCom.selCategory, 1));
                    break;
                    case Lang.English:
                    _dsSet = GetDataSet(string.Format(_sqlCom.selCategory, 2));
                    break;
            }

            for (int i = 0; _dsSet.Tables[0].Rows.Count > i; i++)
            {
                TxtBlocks[i].Text = _dsSet.Tables[0].Rows[i][1].ToString();
                btn[i].Uid = _dsSet.Tables[0].Rows[i][0].ToString();
            }
        }


        private DataSet GetDataSet(string _Сmd)
        {
            using (_cmd = new SQLiteCommand(_Сmd, DataBase.Con))
            {
                try
                {
                    DataBase.Con.Open();
                    _cmd.CommandType=CommandType.Text;
                 var  _adapter = new SQLiteDataAdapter(_cmd);
                    _dsSet=new DataSet();
                    _adapter.Fill(_dsSet);
                   
                }
                catch (SQLiteException ex)
                {
                    ErrorWriter.WriteToLog(ex.Message + " " + ex.ErrorCode + "" + DateTime.Now);
                }
                finally
                {
                    DataBase.Con.Close();
                }
                return _dsSet;
            }
        }


        internal void ButtonClick(Button btn)
        {
           
            switch (btn.Name)
            {
                case "BtnCat1":
                case "BtnCat2":
                case "BtnCat3":
                    FillbtnSubCat(Convert.ToByte(btn.Uid), _sqlCom.selSUBCAT, MyButton_Click);
                    break;
                default: FillbtnSubCat(Convert.ToByte(btn.Uid), _sqlCom.selKeyWordfrnCat, KeyWrdClick);
                    break;
                
            }
        }


        internal void ClearFields()
        {
            Grid.ColumnDefinitions.Clear();
            Grid.Children.Clear();
            TxtDescript.Text = string.Empty;
            List.ItemsSource = null;
        }




        private void FillbtnSubCat(byte idCat, string Cmd, RoutedEventHandler eventHandler)
        {
            ClearFields();
            var table = GetDataSet(string.Format(Cmd, idCat)).Tables;
           for (int i = 0; table[0].Rows.Count > i; i++)
            {
                var myBlock = new TextBlock()
                {
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.WrapWithOverflow,
                    Text = table[0].Rows[i][1].ToString(),
                    Margin = new Thickness(5, 10, 5, 10)
                };
                var myButton = new Button();
                var myDefinition = new ColumnDefinition();
                Grid.SetColumn(myButton, i);
                myButton.Margin = new Thickness(5, 10, 5, 25);
                myButton.MinWidth = 30;
                myButton.Uid = table[0].Rows[i][0].ToString();
                myButton.Click += eventHandler;
                myButton.Content = myBlock;
              
                myDefinition.Width = new GridLength(68);
                Grid.ColumnDefinitions.Add(myDefinition);
                Grid.Children.Add(myButton);
            }
        }



        private void KeyWrdClick(object sender, RoutedEventArgs e)
        {
            var myButton = sender as Button;
            TxtDescript.Text = string.Empty;
            TxtDescript.Text = (string)ShowDescript(myButton.Uid).Tables[0].Rows[0][0];
        }



        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            var myButton = sender as Button;
            List.ItemsSource = null;
            TxtDescript.Text = string.Empty;
            var ds = GetDataSet(string.Format(_sqlCom.selKeyWordfrmSubCat, myButton.Uid));
            List.ItemsSource = ds.Tables[0].DefaultView;
            List.DisplayMemberPath = "WORD";
            List.SelectedValuePath = "KEYWRDID";
        }

        internal DataSet ShowDescript(string id)
        {
            return GetDataSet(string.Format(_sqlCom.selDescriptKeyWrd, id));
        }

    



    internal void FillRowDefinition(Grid grid)
        {
            MessageBox.Show("ColumnDefenition: "+ grid.ColumnDefinitions.Count);
        }
    }
}