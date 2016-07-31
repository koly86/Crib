using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFCrib
{
    static class TableBuild
    {
        static TreeViewItem item;
     
       

        static public void Show(TreeView tree)
        {
           /* tree.ItemsSource= Class.GetDS().Tables[0].DefaultView;
            tree.SelectedValuePath = "CLASSID";  
            tree.DisplayMemberPath = "CLASSNAME"; */ 
          
            #region Show

              DataSet dsClasSet = Class.GetDS();
            DataSet dsPropertySet;

            for (int i = 0; dsClasSet.Tables[0].Rows.Count > i; i++)
            {
                item = new TreeViewItem();


                item.Header = dsClasSet.Tables[0].Rows[i][1];

                dsPropertySet = Property.GetDS((long) dsClasSet.Tables[0].Rows[i][0]);

                for (int j = 0; dsPropertySet.Tables[0].Rows.Count > j; j++)
                {
                    
                    item.Items.Add(dsPropertySet.Tables[0].Rows[j][0]);
                   
                }
                tree.Items.Add(item);
            }  
          
            #endregion
        }
    }
}