using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Threading;

using System.Data.Sql;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ErrorWriteLog;

namespace WPFCrib
{
    static class DataBase
    {

        private static readonly SqlCommand Sqlcom = new SqlCommand();
        private static readonly string connectFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static readonly string pathToDataBase = Path.Combine(connectFolder, "CribDB.db");
        private static SQLiteCommand cmd;
        public  static SQLiteConnection Con;
        private static bool _result;
        private static SQLiteDataAdapter _adapter;
        private static DataSet _ds;
        

        private class SqlCommand
        {
            #region Container

            protected internal readonly string createTblClass =
                "CREATE TABLE tblCLASS('CLASSID' INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                " 'CLASSNAME' varchar(50) NULL, 'CLASSDESCRIPTION'  varchar(150) NULL,  Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP);";

            protected internal readonly string createTblProperty =
                "CREATE TABLE tblPROPERTY( 'PROPID' INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                " 'CLASSID' INTEGER NULL, 'PROPNAME' varchar(50) NULL, 'PROPDESCRIPTION' varchar(250) NULL, " +
                " 'PROPEXAMPLE' varchar(250) NULL,  Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP)";

            protected internal readonly string createTblKeyWords =
                 "CREATE TABLE tblKEYWORDS( 'KEYWRDID' INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                " 'KEYWRDCATID' INTEGER NOT NULL , 'KEYWRDSUBCATID' INTEGER , 'WORD' varchar(50) NOT NULL, 'KEYWRDESCRIPTION' varchar(250) NULL)";


            protected internal readonly string createTblKWСATEGORY =
              "CREATE TABLE tblKWСATEGORY( 'KEYWRDCATID' INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , 'LANGID' INTEGER NOT NULL , 'NAMECAT' varchar(50) NOT NULL )";

            protected internal readonly string createTblKWSUBCATEGORY = 
                "CREATE TABLE tblKWSUBCATEGORY( 'KEYWRDSUBCATID' INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , "+
                " 'KEYWRDCATID' INTEGER , 'LANGID' INTEGER NOT NULL , 'NAMESUBCAT' varchar(50) NOT NULL)";

            protected internal readonly string createTblLanguage =
                 "CREATE TABLE tblLANGUAGE('LONGUAGEID' INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , 'LANGUAGE' varchar(10) NOT NULL)";

            protected internal readonly string ClearDB = " delete from tblPROPERTY; delete from tblCLASS ";

            protected internal readonly string SelectDG =
                "select propname as ИмяПараметра , propdescription as Описание, PROPEXAMPLE as Пример from tblPROPERTY ";
           

            #endregion
        }

        //Создаем БД
        static public void CreateDB()
        {
            #region CreateDB

            try
            {
                if (!File.Exists(pathToDataBase))
                {
                    SQLiteConnection.CreateFile(pathToDataBase);
                    Con = new SQLiteConnection($"Data Source = {pathToDataBase} ;");
                    CreateTable();
                }
                else
                {
                    Con = new SQLiteConnection($"Data Source={pathToDataBase}");
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message);
                ErrorWriter.WriteToLog(ex.Message + " " + ex.Data + " " + DateTime.Now);
            }

            #endregion
        }

        //Создаем таблицы в БД
        private static void CreateTable()
        {
            #region CreateTable

            try
            {
                Con.Open();
                using (var cmd = Con.CreateCommand())
                {
                    cmd.CommandText = Sqlcom.createTblClass;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }


                using (var cmd = Con.CreateCommand())
                {
                    cmd.CommandText = Sqlcom.createTblProperty;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = Con.CreateCommand())
                {
                    cmd.CommandText = Sqlcom.createTblKeyWords;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }



                using (var cmd = Con.CreateCommand())
                {
                    cmd.CommandText = Sqlcom.createTblKWСATEGORY;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = Con.CreateCommand())
                {
                    cmd.CommandText = Sqlcom.createTblKWSUBCATEGORY;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = Con.CreateCommand())
                {
                    cmd.CommandText = Sqlcom.createTblLanguage;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ErrorWriter.WriteToLog(ex.Message + " " + ex.Data + " " + DateTime.Now);
            }
            finally
            {
                Con.Close();
            }

            #endregion
        }


        static public bool ClearDB()
        {
            #region ClearDB

            Con = new SQLiteConnection($"Data Source = {pathToDataBase} ");
            if (File.Exists(pathToDataBase))
            {
                using (var cmd = Con.CreateCommand())
                {
                    try
                    {
                        Con.Open();
                        cmd.CommandText = Sqlcom.ClearDB;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        _result = true;
                    }
                    catch (Exception ex)
                    {
                        ErrorWriter.WriteToLog(ex.Message + " " + ex.Data + " " + DateTime.Now);
                        _result = false;
                    }
                    finally
                    {
                        Con.Close();
                    }
                }
            }
            return _result;

            #endregion
        }


        static public void FillDG(DataGrid dg)
        {
            #region FillDF

            Con = new SQLiteConnection($"Data Source = {pathToDataBase}");
            if (File.Exists(pathToDataBase))
            {
                using (cmd = Con.CreateCommand())
                {
                    try
                    {
                        Con.Open();
                        _adapter = new SQLiteDataAdapter(Sqlcom.SelectDG, Con);
                        _ds = new DataSet();
                        _adapter.Fill(_ds);
                        dg.ItemsSource = _ds.Tables[0].DefaultView;
                    }
                    catch (SQLiteException ex)
                    {
                        ErrorWriter.WriteToLog(ex.Message + " " + ex.Data + " " + DateTime.Now);
                    }
                    finally
                    {
                        Con.Close();
                    }

                }

            }

            #endregion
        }

    }
}


