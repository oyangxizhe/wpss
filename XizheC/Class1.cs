using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XizheC;
using System.Web;

namespace XizheC
{
    public class Class1:Interface1 
    {
        basec bc = new basec();
        private string _a;
        public string a
        {
            set { _a = value; }
            get { return _a; }

        }
        public Class1 ()
        {

          
        }
        public void ac()
        {
            bc.Show("ok");

        }

     
    }
}
