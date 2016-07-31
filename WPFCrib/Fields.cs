using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;


namespace WPFCrib
{

   internal enum NameParam
    {
        PropName = 1,
        PropInstance = 2,
        PropID = 3,
        PropDescript = 4,
        ClassID = 5,
        ClassName = 6,
        ClassDescript = 7
    }

   internal enum Param
    {
        cbbProp = 1,
        txtPropDescript = 2,
        txtPropShourtDescript = 3,

        txtClassDescript = 4,
        cbbClass = 0,

        btnClear = 6,
        btnSaveClass = 7,
        btnSaveProperty = 8
    }

    internal enum KeyWrd
    {
        CatID=0,
        SubCatID=1,
        KeywrdName=2,
        KeyDscr=3
    }


    internal enum Lang
    {
        Russian = 1,
        English = 2
    }




    static class Fields
    {
       static public TextBox[] TxtBox;
       static public ComboBox[] Cbb;
       static public Button[] Btn;
       static Singleton singl = Singleton.Instance();

    


       static public void Chech(RadioButton rb)
        {                        
            switch (rb.Name)
            {
                case "rbWathing": Watching(); break;
                case "rbRecord": Updating(); break;
                case "rbEditing": Updating(); break;                    
            }                           
        }


        //на просмотр 
      static void Watching()
        {
#region Watching
            foreach (TextBox text in TxtBox)
            {
                text.Background = Brushes.Gray;
                text.IsEnabled = false;          
            }

            foreach (ComboBox cb in Cbb)
            {
                cb.Background = Brushes.Gray;
                cb.IsEditable = false;                
            }

            foreach(Button bt in Btn)
            {
                bt.IsEnabled = false;
            }
#endregion
        }

        //на изменение и добавление
      static void Updating()
        {
#region Updat
            foreach (TextBox text in TxtBox)
            {
                text.Background = Brushes.White;
                text.IsEnabled = true;                
            }

            foreach (ComboBox cb in Cbb)
            {
                cb.Background = Brushes.White;
                cb.IsEditable = true;               
            }

            foreach (Button bt in Btn)
            {
                bt.IsEnabled = true;
            }
#endregion
        }

      static public void Clear()
        {
            #region Clear
            foreach (TextBox text in TxtBox)
            {
                text.Text = String.Empty;
            }

            //  foreach(ComboBox cb in   cbb)
            foreach (ComboBox cb in Cbb)
            {
                cb.Items.IndexOf(0);
            }
            #endregion
        }
    }
}
