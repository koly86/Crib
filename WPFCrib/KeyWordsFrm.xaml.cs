using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFCrib
{
    /// <summary>
    /// Interaction logic for KeyWordsFrm.xaml
    /// </summary>
    public partial class KeyWordsFrm : Window
    {
        
        public KeyWordsFrm()
        {
            InitializeComponent();
            _keyWord.FillCat();
        }

        private Dictionary<KeyWrd, string> data;
        private readonly KeyWord _keyWord = new KeyWord();


        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            
            if ((txtDescript.Text == "") || (txtKeyWord.Text == "")) { MessageBox.Show("Поля не могут быть пустыми"); return;}
            if (Equals(cbCategory.SelectedValue, null)) { MessageBox.Show("Выберите категорию"); return; }
           

            var idsubcat = Equals(cbSubCategory.SelectedValue, null) ? 0 : (long)cbSubCategory.SelectedValue;

            data = new Dictionary<KeyWrd, string>()
            {
                {KeyWrd.CatID, cbCategory.SelectedValue.ToString() }, { KeyWrd.SubCatID, idsubcat.ToString()},
                {KeyWrd.KeywrdName, txtKeyWord.Text }, { KeyWrd.KeyDscr, txtDescript.Text }
            };
            _keyWord.InsKeyWord(data);
            txtDescript.Text = string.Empty;
            txtKeyWord.Text = string.Empty;
            txtKeyWord.Focus();


        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void RbEnglish_OnChecked(object sender, RoutedEventArgs e)
        {
            cbCategory.ItemsSource = _keyWord.ShowCat(Lang.English).DefaultView;
            cbCategory.DisplayMemberPath = "NAMECAT";
            cbCategory.SelectedValuePath = "KEYWRDCATID";
        }

        private void RbRussian_OnChecked(object sender, RoutedEventArgs e)
        {
            cbCategory.ItemsSource = _keyWord.ShowCat(Lang.Russian).DefaultView;
            cbCategory.DisplayMemberPath = "NAMECAT";
            cbCategory.SelectedValuePath = "KEYWRDCATID";
        }


        private void CbCategory_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbBox = sender as ComboBox;
            var CatId = Equals(cbBox.SelectedValue, null) ? 0 : (long)cbBox.SelectedValue;

            if (rbEnglish.IsChecked.Value)
            {
                cbSubCategory.ItemsSource = _keyWord.ShowSubCatTable(Lang.English, CatId).DefaultView;
                cbSubCategory.DisplayMemberPath = "NAMESUBCAT";
                cbSubCategory.SelectedValuePath = "KEYWRDSUBCATID";
            }
            else
            {
                cbSubCategory.ItemsSource = _keyWord.ShowSubCatTable(Lang.Russian, CatId).DefaultView;
                cbSubCategory.DisplayMemberPath = "NAMESUBCAT";
                cbSubCategory.SelectedValuePath = "KEYWRDSUBCATID";
            }
        }
    }
}
