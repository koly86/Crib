using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFCrib
{
    /// <summary>
    /// Interaction logic for DataTable.xaml
    /// </summary>
    public partial class DataTableD : Window
    {

        public DataTableD()
        {
            InitializeComponent();
            DataBase.FillDG(dataGrid);
            TableBuild.Show(treeView);
        }

       

        private void TreeView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tree = sender as TreeView;
            MessageBox.Show(Convert.ToString(tree.SelectedValue));
        }
    }
}