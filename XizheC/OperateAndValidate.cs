using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace XizheC
{
    public class OperateAndValidate
    {
        BaseOperate boperate = new BaseOperate();//声明BaseOperate类的一个对象，以调用其方法


        public void Show(string MessageInfo)
        {
            HttpContext.Current.Response.Write("<script language=javascript>alert('" + MessageInfo + "')</script>");
        }


        public void ShowP(string values, string PageURL)
        {
            HttpContext.Current.Response.Write("<script>alert('" + values + "');window.location.href='" + PageURL + "'</script>");
            HttpContext.Current.Response.End();
        }


        #region  自动编号
        /// <summary>
        /// 自动编号
        /// </summary>
        /// <param name="P_str_sqlstr">SQL语句</param>
        /// <param name="P_str_table">数据表名</param>
        /// <param name="P_str_tbColumn">数据表字段</param>
        /// <param name="P_str_codeIndex">编号前的字符串</param>
        /// <param name="P_str_codeNum">编号后面的数字</param>
        /// <param name="txt">TextBox控件名</param>
        public void autoNum(string P_str_sqlstr, string P_str_table, string P_str_tbColumn, string P_str_codeIndex, string P_str_codeNum, TextBox txt)
        {
            string P_str_Code = "";
            int P_int_Code = 0;
            DataSet myds = boperate.getds(P_str_sqlstr, P_str_table);
            if (myds.Tables[0].Rows.Count == 0)
            {
                txt.Text = P_str_codeIndex + P_str_codeNum;
            }
            else
            {
                P_str_Code = Convert.ToString(myds.Tables[0].Rows[myds.Tables[0].Rows.Count - 1][P_str_tbColumn]);
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(2, 7)) + 1;
                P_str_Code = P_str_codeIndex + P_int_Code.ToString();
                txt.Text = P_str_Code;
            }
        }
        #endregion
        #region 编号
        public void num(string sql1, string sql2, string table1, string tbColumns, string prifix, string suffix, TextBox txt)
        {
            string year, month;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            DataSet ds1 = boperate.getds(sql1, table1);
            string P_str_Code, t, q = "";

            int P_int_Code, w;
            SqlDataReader sqlread = boperate.getread(sql2);
            sqlread.Read();
            if (sqlread.HasRows)
            {
                P_str_Code = Convert.ToString(ds1.Tables[0].Rows[(ds1.Tables[0].Rows.Count - 1)][tbColumns]);
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(6, 4)) + 1;
                t = Convert.ToString(P_int_Code);
                w = 4 - t.Length;
                while (w >= 1)
                {
                    q = q + "0";
                    w = w - 1;

                }
                txt.Text = prifix + year + month + q + P_int_Code;
            }
            else
            {
                txt.Text = prifix + year + month + suffix;
            }
            sqlread.Close();
        }
        #endregion
     

    }
}
