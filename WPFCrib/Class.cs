using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations.Model;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ErrorWriteLog;

namespace WPFCrib
{
   static class Class
    {
        private static SQLiteCommand cmd;
        private static readonly SQLCommand Sqlcom = new SQLCommand();
        private static SQLiteDataAdapter adapter;
        private static DataSet ds;

        static public Singleton singl = Singleton.Instance();
        static public TextBox[] txtBox;
        static public ComboBox[] cbb;
       

        class SQLCommand
        {
            public readonly string Insert = "insert into tblCLASS(CLASSNAME, CLASSDESCRIPTION) " +
                                       "  values (@CLASSNAME, @CLASSDESCRIPTION); " +
                                       " SELECT last_insert_rowid() as id";
            public readonly string Select = " select CLASSID, CLASSNAME from tblCLASS ";
            public readonly string SelectDescript = "select CLASSDESCRIPTION from tblCLASS where CLASSID = @CLASSID";
            public readonly string Update = "update tblCLASS set CLASSDESCRIPTION = @CLASSDESCRIPTION where CLASSID = @CLASSID";
        }

        //Пишем класс
        static public bool Insert(Dictionary<NameParam, string> data)
        {
#region Insert
            using (cmd = new SQLiteCommand(Sqlcom.Insert, DataBase.Con))
            {
                cmd.Parameters.Add("@CLASSNAME", DbType.String, 50);
                cmd.Parameters.Add("@CLASSDESCRIPTION", DbType.String, 150);

                cmd.Parameters["@CLASSNAME"].Value =  data[NameParam.ClassName];
                cmd.Parameters["@CLASSDESCRIPTION"].Value = data[NameParam.ClassDescript];
                try
                {
                    DataBase.Con.Open();
                    var classid = cmd.ExecuteScalar();
                    var ClassID = classid is long ? (long)classid : 0; //получаем ID нового класса
                    DataBase.Con.Close();
                    Show();
                   
                    cbb[(byte)Param.cbbClass].SelectedValue = ClassID;
                    return true;
                   
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message);
                    ErrorWriter.WriteToLog(ex.Message + " "+ex.ErrorCode + "" +DateTime.Now);

                    return false;
                }
                finally
                {
                    DataBase.Con.Close();
                }
            }
#endregion
        }

        
       static public void Show()
        {
#region Show
            try
            {               
                if (!cbb[(byte) Param.cbbClass].Items.IsEmpty)
                {
                    cbb[(byte)Param.cbbClass].ItemsSource = null;
                    cbb[(byte)Param.cbbClass].Items.Clear();
                }
                cbb[(byte)Param.cbbClass].ItemsSource = GetDS().Tables[0].DefaultView;
                cbb[(byte)Param.cbbClass].DisplayMemberPath = "CLASSNAME";
                cbb[(byte)Param.cbbClass].SelectedValuePath = "CLASSID";
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ошибка с сообщением " + ex.Message);
                ErrorWriter.WriteToLog(ex.Message + " " + ex.ErrorCode + "" + DateTime.Now);
            }
#endregion

        }

       
         static public DataSet GetDS()
           {
#region GetDS
            try
               {
                  
                DataBase.Con.Open();
                adapter = new SQLiteDataAdapter(Sqlcom.Select, DataBase.Con);
                ds = new DataSet();

                   adapter.Fill(ds);
                   return ds;
               }
               catch (SQLiteException ex)
               {
                   MessageBox.Show("Ошибка с сообщением " + ex.Message);
                   ErrorWriter.WriteToLog(ex.Message + " " + ex.ErrorCode + "" + DateTime.Now);
                   return ds;
               }
               finally
               {
                   DataBase.Con.Close();
               }
#endregion
        }


       static public void ShowDescript(Int64 idClass)
        {
#region ShowDescript
            try
            {
                DataBase.Con.Open();
                using (cmd = new SQLiteCommand(Sqlcom.SelectDescript, DataBase.Con))
                {
                    cmd.Parameters.Add("@CLASSID", DbType.Int64);
                    cmd.Parameters["@CLASSID"].Value = idClass;
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            txtBox[0].Text = reader.GetString(0);
                        }
                    }
                    else
                    {
                        txtBox[0].Text = string.Empty;
                    }
                }
            }

            catch (SQLiteException ex)
            {
                MessageBox.Show("Ошибка с сообщением " + ex.Message);
                ErrorWriter.WriteToLog(ex.Message + " " + ex.ErrorCode + "" + DateTime.Now);
            }

            finally
            {
                DataBase.Con.Close();
            }
#endregion
        }


        static public void Update(Dictionary<NameParam,string> data)
        {
#region Update
            using (cmd = new SQLiteCommand(Sqlcom.Update, DataBase.Con))
            {
                cmd.Parameters.Add("@CLASSDESCRIPTION", DbType.String, 150);
                cmd.Parameters.Add("@CLASSID", DbType.Int32);

                cmd.Parameters["@CLASSDESCRIPTION"].Value = data[NameParam.ClassDescript] ;
                cmd.Parameters["@CLASSID"].Value = data[NameParam.ClassID];
                try
                {
                    DataBase.Con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Ошибка с сообщением " + ex.Message);
                    ErrorWriter.WriteToLog(ex.Message + " " + ex.ErrorCode + "" + DateTime.Now);
                }
                finally
                {
                    DataBase.Con.Close();
                }
            }
#endregion
        }
    }
}
