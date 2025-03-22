using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using XizheC;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace XizheC
{
    public  class PriceStyle
    {
        private string _UserID;
        public string UserID
        {
            set { _UserID = value; }
            get { return _UserID; }


        }      
      #region getpricestyle
        public DataTable GetPriceStyle()
        {
            string[] a = new string[] { "面积单价", "价格系数", "附加单价" };
            DataTable dtt = new DataTable();
            dtt.Columns.Add("价格方式", typeof(string));
            for (int i = 0; i <a.Length ; i++)
            {
                DataRow dr = dtt.NewRow();
                dr["价格方式"] = a[i];
                dtt.Rows.Add(dr);
            }
            return dtt;
        }
        #endregion
      
    }
}
