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
using System.Text;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop;
using System.Security.Cryptography;

namespace XizheC
{
    public class CRECEIVABLES_ORDER
    {
        basec bc = new basec();
        #region nature
    
        private string _sql;
        public string sql
        {
            set { _sql = value; }
            get { return _sql; }

        }
        private string _sqlo;
        public string sqlo
        {
            set { _sqlo = value; }
            get { return _sqlo; }

        }
        private string _sqlt;
        public string sqlt
        {
            set { _sqlt = value; }
            get { return _sqlt; }

        }
        private string _sqlth;
        public string sqlth
        {
            set { _sqlth = value; }
            get { return _sqlth; }

        }
        private string _sqlf;
        public string sqlf
        {
            set { _sqlf = value; }
            get { return _sqlf; }

        }
        private string _sqlfi;
        public string sqlfi
        {
            set { _sqlfi = value; }
            get { return _sqlfi; }

        }
        private string _IDO;
        public string IDO
        {
            set { _IDO = value; }
            get { return _IDO; }

        }
      
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }

        #endregion
        #region setsql
        string setsql = @"

SELECT 
A.ROID AS 收款单号,
A.RECEIVABLES_ORDER_MAKERID AS 经手人工号,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.RECEIVABLES_ORDER_MAKERID) AS 经手人,
A.RECEIVABLES_ORDER_DATE AS 收款日期,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.MAKERID) AS 制单人,
A.DATE AS 制单日期,
A.REMARK AS 备注,
A.RECEIVABLES_ORDER_AMOUNT AS 收款金额
FROM RECEIVABLES_ORDER A 
";


        #endregion
        #region setsqlo
        string setsqlo = @"
INSERT INTO RECEIVABLES_ORDER
(
ROKEY,
ROID,
RCID,
RECEIVABLES_ORDER_AMOUNT,
RECEIVABLES_ORDER_MAKERID,
RECEIVABLES_ORDER_DATE,
REMARK,
MAKERID,
DATE,
YEAR,
MONTH,
DAY
)
VALUES
(
@ROKEY,
@ROID,
@RCID,
@RECEIVABLES_ORDER_AMOUNT,
@RECEIVABLES_ORDER_MAKERID,
@RECEIVABLES_ORDER_DATE,
@REMARK,
@MAKERID,
@DATE,
@YEAR,
@MONTH,
@DAY


)
";
        #endregion
        #region setsqlt
        string setsqlt = @"

";
        #endregion
        #region setsqlth
        string setsqlth = @"

";


        #endregion
        #region setsqlf
        string setsqlf = @"
UPDATE RECEIVABLES_ORDER SET 
RCID=@RCID,
RECEIVABLES_ORDER_MAKERID=@RECEIVABLES_ORDER_MAKERID,
RECEIVABLES_ORDER_DATE=@RECEIVABLES_ORDER_DATE,
RECEIVABLES_ORDER_AMOUNT=@RECEIVABLES_ORDER_AMOUNT,
REMARK=@REMARK,
MAKERID=@MAKERID,
DATE=@DATE

";
        #endregion
        #region setsqlfi
        string setsqlfi = @"

";
        #endregion
        DataTable dtx2 = new DataTable();
        DataTable dt4 = new DataTable();
        CRECEIVABLES creceivables = new CRECEIVABLES();
        public CRECEIVABLES_ORDER()
        {
            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
            sqlf = setsqlf;
            sqlfi = setsqlfi;
        }
        public string GETID()
        {
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM RECEIVABLES_ORDER", "ROID", "RO");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        #region RETURN_DT
        public DataTable RETURN_DT()
        {
            DataTable dtt = creceivables.DT_EMPTY();
            string sqlx = "SELECT * FROM RECEIVABLES_ORDER ";
            DataTable dt = bc.getdt(sqlx);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    dr["收款单号"] = dr1["ROID"].ToString();
                    dr["收款金额"] = dr1["RECEIVABLES_ORDER_AMOUNT"].ToString();
                    dr["收款人工号"] = dr1["RECEIVABLES_ORDER_MAKERID"].ToString();
                    dr["收款人"] = bc.getOnlyString("SELECT ENAME FROM EMPLOYEEINFO WHERE EMID='"+dr1["RECEIVABLES_ORDER_MAKERID"].ToString()+"'");
                    dr["收款日期"] = dr1["RECEIVABLES_ORDER_DATE"].ToString();
                    dr["备注"] = dr1["REMARK"].ToString();
                    dr["应收单号"] = dr1["RCID"].ToString();
                    DataTable dtx = bc.GET_DT_TO_DV_TO_DT(creceivables.RETURN_DT(), "", "应收单号='" + dr1["RCID"].ToString() + "'");
                    if (dtx.Rows.Count > 0)
                    {
                        dr["发票号码"] = dtx.Rows[0]["发票号码"].ToString();
                        dr["客户名称"] = dtx.Rows[0]["客户名称"].ToString();
                        dr["累计收款金额"] = dtx.Rows[0]["累计收款金额"].ToString();
                        dr["未收款金额"] = dtx.Rows[0]["未收款金额"].ToString();
                        dr["实际应收金额"] = dtx.Rows[0]["实际应收金额"].ToString();
                    }
                    dr["制单人"] = bc.getOnlyString("SELECT ENAME FROM EMPLOYEEINFO WHERE EMID='" + dr1["MAKERID"].ToString() + "'");
                    dr["制单日期"] = dr1["DATE"].ToString();
                    dtt.Rows.Add(dr);
                }

            }

            return dtt;
        }
        #endregion
   


    }
}
