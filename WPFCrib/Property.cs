using System;
using System.Data.SQLite;
using System.Data;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Generic;
using ErrorWriteLog;

namespace WPFCrib
{



   static class Property
    {
        private static readonly  SqlCom Sqlcom = new SqlCom();
        static public ComboBox[] Cbb;
        static public TextBox[] TxtBox;
        static public readonly string SelectProperty = " select  propname from tblPROPERTY where classid = {0} ";


        class SqlCom
        {
#region Container
            public readonly string SelectName = "Select PROPID, PROPNAME from tblPROPERTY";
            public readonly string Insert = "insert into tblPROPERTY (CLASSID, PROPNAME, PROPDESCRIPTION, PROPEXAMPLE) " +
               " VALUES(@CLASSID, @PROPNAME, @PROPDESCRIPTION, @PROPEXAMPLE); " +
               "SELECT last_insert_rowid() as id ";

            public readonly string SelectDescript = "Select PROPDESCRIPTION, PROPEXAMPLE from tblPROPERTY " +
                                                  " where PROPID = @PROPID ";
            public readonly string Update = "UPDATE tblPROPERTY " +
                " SET CLASSID = @CLASSID, PROPNAME  = @PROPNAME ,  PROPDESCRIPTION  = @PROPDESCRIPTION, " +
                " PROPEXAMPLE = @PROPEXAMPLE WHERE PROPID = @PROPID ";
             
           

            public SQLiteCommand Command;
            public SQLiteDataAdapter Adapter;
            public DataSet DataSet;
#endregion

        }



       static public void ShowDescript(long idProperty)
        {
#region ShowDescription
            try
            {
                DataBase.Con.Open();
                using (Sqlcom.Command = new SQLiteCommand(Sqlcom.SelectDescript, DataBase.Con))
                {
                    Sqlcom.Command.Parameters.Add("@PROPID", DbType.Int64);
                    Sqlcom.Command.Parameters["@PROPID"].Value = idProperty;
                    SQLiteDataReader reader = Sqlcom.Command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            TxtBox[0].Text = reader.GetValue(0) is string ? reader.GetString(0) : "";
                            TxtBox[1].Text = reader.GetValue(1) is string ? reader.GetString(1) : "";
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ошибка с сообщением " + ex.Message);
                ErrorWriter.WriteToLog(ex.Message);
            }
            finally
            {
                DataBase.Con.Close();
            }
#endregion
        }


        static public void Show()
        {
#region Show
            try
            {
                DataBase.Con.Open();
                Sqlcom.Adapter = new SQLiteDataAdapter(Sqlcom.SelectName, DataBase.Con);
                Sqlcom.DataSet = new DataSet();
                Sqlcom.Adapter.Fill(Sqlcom.DataSet);
                if (!Cbb[(byte) Param.cbbProp].Items.IsEmpty)
                {
                    Cbb[(byte)Param.cbbProp].ItemsSource = null;
                    Cbb[(byte)Param.cbbProp].Items.Clear();
                }
                Cbb[(byte)Param.cbbProp].ItemsSource = Sqlcom.DataSet.Tables[0].DefaultView;
                Cbb[(byte)Param.cbbProp].DisplayMemberPath = "PROPNAME";
                Cbb[(byte)Param.cbbProp].SelectedValuePath = "PROPID";
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

       static public DataSet GetDS(long clssID)
       {
#region getDS
            try
            {
                DataBase.Con.Open();
                Sqlcom.Adapter = new SQLiteDataAdapter(string.Format(SelectProperty, clssID), DataBase.Con);
                Sqlcom.DataSet = new DataSet();

                Sqlcom.Adapter.Fill(Sqlcom.DataSet);
                return Sqlcom.DataSet;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Ошибка с сообщением " + ex.Message);
                ErrorWriter.WriteToLog(ex.Message + " " + ex.ErrorCode + "" + DateTime.Now);
                return Sqlcom.DataSet;
            }
            finally
            {
                DataBase.Con.Close();
            }
#endregion
       }


        static public bool Insert(Dictionary<NameParam, string> data)
        {
#region Insert
            using (Sqlcom.Command = new SQLiteCommand(Sqlcom.Insert, DataBase.Con))
            {
                Sqlcom.Command.Parameters.Add("@CLASSID", DbType.Int64);
                Sqlcom.Command.Parameters.Add("@PROPNAME", DbType.String, 50);
                Sqlcom.Command.Parameters.Add("@PROPDESCRIPTION", DbType.String, 500);
                Sqlcom.Command.Parameters.Add("@PROPEXAMPLE", DbType.String, 250);

                Sqlcom.Command.Parameters["@CLASSID"].Value = data[NameParam.ClassID];
                Sqlcom.Command.Parameters["@PROPNAME"].Value = data[NameParam.PropName];
                Sqlcom.Command.Parameters["@PROPDESCRIPTION"].Value = data[NameParam.PropDescript];
                Sqlcom.Command.Parameters["@PROPEXAMPLE"].Value = data[NameParam.PropInstance];
                try
                {
                    DataBase.Con.Open();
                    var propid = Sqlcom.Command.ExecuteScalar();
                    var PropID = propid is long ? (long)propid : 0;

                    return true;
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show(ex.Message);
                    ErrorWriter.WriteToLog(ex.Message);
                    return false;
                }
                finally
                {
                    DataBase.Con.Close();
                }
            }
            #endregion
        }



        static public void Update(Dictionary<NameParam, string> data)
        {
#region DataUpdate   

            using (Sqlcom.Command = new SQLiteCommand(Sqlcom.Update, DataBase.Con))
            {
                Sqlcom.Command.Parameters.Add("@CLASSID", DbType.Int64);
                Sqlcom.Command.Parameters.Add("@PROPNAME", DbType.String, 50);
                Sqlcom.Command.Parameters.Add("@PROPDESCRIPTION", DbType.String, 250);
                Sqlcom.Command.Parameters.Add("@PROPEXAMPLE", DbType.String, 250);
                Sqlcom.Command.Parameters.Add("@PROPID", DbType.Int32);

                Sqlcom.Command.Parameters["@CLASSID"].Value = data[NameParam.ClassID];
                Sqlcom.Command.Parameters["@PROPNAME"].Value = data[NameParam.PropName];
                Sqlcom.Command.Parameters["@PROPDESCRIPTION"].Value = data[NameParam.PropDescript];
                Sqlcom.Command.Parameters["@PROPEXAMPLE"].Value = data[NameParam.PropInstance];
                Sqlcom.Command.Parameters["@PROPID"].Value = data[NameParam.PropID];
                try
                {
                    DataBase.Con.Open();
                    Sqlcom.Command.ExecuteNonQuery();
                }
                catch (SQLiteException ex)
                {
                    MessageBox.Show("Ошибка с сообщением " + ex.Message);
                    ErrorWriter.WriteToLog(ex.Message);
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
