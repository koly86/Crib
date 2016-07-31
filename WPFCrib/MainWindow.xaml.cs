using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ErrorWriteLog;

namespace WPFCrib
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            DataBase.CreateDB();

            InitializeComponent();

          
            Fields.Cbb = new ComboBox[2] { cbbClass, СbbProperty };
            Fields.TxtBox = new TextBox[3] { txtDescriptClass, txtDescriptProperty, TxtShourtDescriptProp };
            Fields.Btn = new Button[3] { btnClear, btnSaveClass, btnSaveProperty };

            Class.cbb = new ComboBox[2] { cbbClass, СbbProperty };
            Class.txtBox = new TextBox[3] { txtDescriptClass, txtDescriptProperty, TxtShourtDescriptProp };

            Property.Cbb = new ComboBox[2] { cbbClass, СbbProperty };
            Property.TxtBox = new TextBox[3] { txtDescriptClass, txtDescriptProperty, TxtShourtDescriptProp };

            Class.Show();
            Property.Show();
        }

        private Dictionary<NameParam, string> data;


        private void rbWathing_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var rb = sender as RadioButton;
                Fields.Chech(rb);
                Class.Show();
                Property.Show();
            }
            catch(Exception ex)
            {
                ErrorWriter.WriteToLog(ex.Message + " "+ ex.TargetSite  +" "+DateTime.Now + " "+ ex.Source);
            }
        }
        
        private void cbbClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox classId = sender as ComboBox;
            try
            {
                long classid = Equals(classId.SelectedValue, null) ? 0 : (long)classId.SelectedValue;
                Class.ShowDescript(classid);
            }
            catch (Exception ex)
            {
                ErrorWriter.WriteToLog( ex.Message +" "+ ex.TargetSite+" "+ DateTime.Now);
            } 
        }

        private void cbbPropertyName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbClass.Text == string.Empty)
            {
                MessageBox.Show("Перед тем как внести имя метода \nвыберите класс");
                cbbClass.Focus();
            }
        }

        private void btnSaveProperty_Click(object sender, RoutedEventArgs e)
        {
            if (Equals(cbbClass.SelectedValue, null)) { MessageBox.Show("Выберите класс"); cbbClass.Focus(); return; }
            if (СbbProperty.Text.Equals("")) { MessageBox.Show("Внесите название метода"); СbbProperty.Focus(); return; }
            data = new Dictionary<NameParam, string>()
            {{ NameParam.PropName, СbbProperty.Text }, { NameParam.ClassID, cbbClass.SelectedValue.ToString() },
             {NameParam.PropDescript, txtDescriptProperty.Text},{NameParam.PropInstance, TxtShourtDescriptProp.Text}};
            if (Property.Insert(data)) Property.Show();
        }

        private void bntClear_Click(object sender, RoutedEventArgs e)
        {
            Fields.Clear();
        }

        private void btnSaveClass_Click(object sender, RoutedEventArgs e)
        {
             data = new Dictionary<NameParam, string>()
            {{NameParam.ClassName, cbbClass.Text}, {NameParam.ClassDescript, txtDescriptClass.Text} };
            

            Class.Insert(data);
        }

       private void CbbProperty_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (cbbClass.Text == "")
            {
                MessageBox.Show("Перед заполнением поля выберите класс");
                cbbClass.Focus();
            }
        }

        private void ItemClickShowDataTabelFrm(object sender, RoutedEventArgs e)
        {
           
            var dtDataTable = new DataTableD();
            dtDataTable.ShowDialog();
        }

        private void MenuItem_OnClickShow(object sender, RoutedEventArgs e)
        {
            var frm = new KeyWrdShow();
            frm.ShowDialog();
        }

        private void MenuItem_OnClickAdd(object sender, RoutedEventArgs e)
        {
           var frm = new KeyWordsFrm();
           frm.ShowDialog();
        }

        private void MenuItem_OnClickDelete(object sender, RoutedEventArgs e)
        {
            DataBase.ClearDB();
        }
    }
}