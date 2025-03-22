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
    public class CPAYMENT_ORDER
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
        private string _MAKERID;
        public string MAKERID
        {
            set { _MAKERID = value; }
            get { return _MAKERID; }

        }
        private string _PLID;
        public string PLID
        {
            set { _PLID = value; }
            get { return _PLID; }

        }
        private decimal  _PURCHASE_INVOICEUNITPRICE;
        public  decimal  PURCHASE_INVOICEUNITPRICE
        {
            set { _PURCHASE_INVOICEUNITPRICE = value; }
            get { return _PURCHASE_INVOICEUNITPRICE; }

        }
        private string _P_COUNT;
        public string P_COUNT
        {
            set { _P_COUNT = value; }
            get { return _P_COUNT; }

        }
        private string _XID;
        public string XID
        {
            set { _XID = value; }
            get { return _XID; }
        }
        private string _SUID;
        public string SUID
        {
            set { _SUID = value; }
            get { return _SUID; }
        }
        private string _NEEDDATE;
        public string NEEDDATE
        {
            set { _NEEDDATE = value; }
            get { return _NEEDDATE; }

        }
        #endregion
        #region setsql
        string setsql = @"

SELECT 
A.POID AS 付款单号,
A.PAYMENT_ORDER_MAKERID AS 经手人工号,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.PAYMENT_ORDER_MAKERID) AS 经手人,
A.PAYMENT_ORDER_DATE AS 付款日期,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.MAKERID) AS 制单人,
A.DATE AS 制单日期,
A.REMARK AS 备注,
A.PAYMENT_ORDER_AMOUNT AS 付款金额
FROM PAYMENT_ORDER A 
";


        #endregion
        #region setsqlo
        string setsqlo = @"
INSERT INTO PAYMENT_ORDER
(
POKEY,
POID,
RMID,
PAYMENT_ORDER_AMOUNT,
PAYMENT_ORDER_MAKERID,
PAYMENT_ORDER_DATE,
REMARK,
MAKERID,
DATE,
YEAR,
MONTH,
DAY
)
VALUES
(
@POKEY,
@POID,
@RMID,
@PAYMENT_ORDER_AMOUNT,
@PAYMENT_ORDER_MAKERID,
@PAYMENT_ORDER_DATE,
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
INSERT INTO GODE
(
GEKEY,
GODEID,
PAYMENT_ORDER_AMOUNT,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@GEKEY,
@GODEID,
@PAYMENT_ORDER_AMOUNT,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        #endregion
        #region setsqlth
        string setsqlth = @"
INSERT INTO MATERRE
(
MRKEY,
MATEREID,
PAYMENT_ORDER_AMOUNT,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@MRKEY,
@MATEREID,
@PAYMENT_ORDER_AMOUNT,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";


        #endregion
        #region setsqlf
        string setsqlf = @"
UPDATE PAYMENT_ORDER SET 
RMID=@RMID,
PAYMENT_ORDER_MAKERID=@PAYMENT_ORDER_MAKERID,
PAYMENT_ORDER_DATE=@PAYMENT_ORDER_DATE,
PAYMENT_ORDER_AMOUNT=@PAYMENT_ORDER_AMOUNT,
REMARK=@REMARK,
MAKERID=@MAKERID,
DATE=@DATE

";
        #endregion
        #region setsqlfi
        string setsqlfi = @"
UPDATE GODE SET 
PAYMENT_ORDER_AMOUNT=@PAYMENT_ORDER_AMOUNT,
MAKERID=@MAKERID,
DATE=@DATE
";
        #endregion
        DataTable dtx2 = new DataTable();
        DataTable dt4 = new DataTable();
        CREQUEST_MONEY crequest_money = new CREQUEST_MONEY();
        public CPAYMENT_ORDER()
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
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM PAYMENT_ORDER", "POID", "PO");
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
            DataTable dtt = crequest_money.DT_EMPTY();
            string sqlx = "SELECT * FROM PAYMENT_ORDER ";
            DataTable dt = bc.getdt(sqlx);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    dr["付款单号"] = dr1["POID"].ToString();
                    dr["付款金额"] = dr1["PAYMENT_ORDER_AMOUNT"].ToString();
                    dr["付款人工号"] = dr1["PAYMENT_ORDER_MAKERID"].ToString();
                    dr["付款人"] = bc.getOnlyString("SELECT ENAME FROM EMPLOYEEINFO WHERE EMID='"+dr1["PAYMENT_ORDER_MAKERID"].ToString()+"'");
                    dr["付款日期"] = dr1["PAYMENT_ORDER_DATE"].ToString();
                    dr["备注"] = dr1["REMARK"].ToString();
                    dr["请款单号"] = dr1["RMID"].ToString();
                    DataTable dtx = bc.GET_DT_TO_DV_TO_DT(crequest_money.RETURN_DT(), "", "请款单号='"+dr1["RMID"].ToString ()+"'");
                    if (dtx.Rows.Count > 0)
                    {
                        dr["发票号码"] = dtx.Rows[0]["发票号码"].ToString();
                        dr["供应商名称"] = dtx.Rows[0]["供应商名称"].ToString();
                        dr["累计付款金额"] = dtx.Rows[0]["累计付款金额"].ToString();
                        dr["未付款金额"] = dtx.Rows[0]["未付款金额"].ToString();
                        dr["实际请款金额"] = dtx.Rows[0]["实际请款金额"].ToString();
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
