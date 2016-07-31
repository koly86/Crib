using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for KeyWrdShow.xaml
    /// </summary>
    public partial class KeyWrdShow : Window
    {
       
        internal KeyWord keyWord = new KeyWord();
        public  KeyWrdShow()
        {
            InitializeComponent();
            keyWord.btn = new Button[11] { BtnCat1, BtnCat2, BtnCat3, BtnCat4, BtnCat5, BtnCat6, BtnCat7, BtnCat8, BtnCat9, BtnCat10, BtnCat11 };
            keyWord.TxtBlocks = new TextBlock[11] {Block1, Block2, Block3 , Block4, Block5, Block6, Block7, Block8, Block9, Block10, Block11 };
            keyWord.Grid = Grid1;
            keyWord.List = ListSub;
            keyWord.TxtDescript = TextBlockDescrip;
            keyWord.FillbtnCat(Lang.English);
        }


        private void BtnCat1_OnClick(object sender, RoutedEventArgs e)
        {
            var bntButton = sender as Button;
             keyWord.ButtonClick(bntButton);
        }
       

        private void ListSub_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBlockDescrip.Text = string.Empty;
            var idKyeSubWord = Equals(ListSub.SelectedValue, null) ? "0" : ListSub.SelectedValue.ToString();
            TextBlockDescrip.Text = (string)keyWord.ShowDescript(idKyeSubWord).Tables[0].Rows[0][0];
        }

        
    }
}

