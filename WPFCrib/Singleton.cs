using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFCrib
{
    


    class Singleton
    {       

        static Singleton uniqueInstance;
       

        protected Singleton()
        {
            
        }

        public static Singleton Instance()
        {
            if (uniqueInstance == null)
                uniqueInstance = new Singleton();

            return uniqueInstance;
        }
    }
}
