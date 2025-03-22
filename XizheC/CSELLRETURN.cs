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
    public class CSELLRETURN
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

        string setsql = @"

SELECT 
A.SRKEY AS 索引,
A.SRID AS 销退单号,
A.ORID as 订单号,
A.SN as 项次,
E.WAREID as ID,
B.CO_WAREID AS 料号,
B.WNAME AS 品名,
B.CWAREID AS 客户料号,
B.SPEC as 规格,
B.UNIT as 单位,
C.OCOUNT AS 订单数量,
C.SELLUNITPRICE AS 销售单价,
C.TAXRATE AS 税率,
E.GECount as 销退数量 ,
A.NOTAX_AMOUNT AS 销退未税金额,
A.TAX_AMOUNT AS 销退税额,
A.AMOUNT AS 销退含税金额,
C.CUID as 客户代码,
D.CNAME as 客户名称 ,
F.SELLReturn_DATE AS 销退日期,
F.SELLReturn_ID AS 销退员工号,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.SELLReturn_ID )  AS 销退人,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=E.MAKERID )  AS 制单人,
E.DATE AS 制单日期,
A.REMARK AS 备注
from SELLReturn_DET A 
LEFT JOIN ORDER_DET C ON A.ORID=C.ORID AND A.SN=C.SN
LEFT JOIN CUSTOMERINFO_MST D ON C.CUID=D.CUID
LEFT JOIN GODE E ON A.SRKEY=E.GEKEY
LEFT JOIN WAREINFO B ON E.WAREID=B.WAREID
LEFT JOIN SELLReturn_MST F ON A.SRID=F.SRID
";
        string setsqlo = @"


"
;

        string setsqlt = @"

";
        string setsqlth = @"

";
        string setsqlf = @"

";
        DataTable dtx2 = new DataTable();
        DataTable dt4 = new DataTable();
        PrintSellTableBill printselltablebill = new PrintSellTableBill();
        public CSELLRETURN()
        {
            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
            sqlf = setsqlf;
        }
        public string GETID()
        {
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM SELLRETURN_MST", "SRID", "SR");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        #region asko
        public DataTable asko(string SRID)
        {
            DataTable dtt = printselltablebill .table ();
            DataTable dt = bc.getdt(sql+ " WHERE A.SRID='" +SRID+"'");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow dr1 = dtt.NewRow();
                    dr1["订单号"] = dr["订单号"].ToString();
                    dr1["客户名称"] = dr["客户名称"].ToString();
                 
         
                    dr1["料号"] = dr["料号"].ToString();
                    dr1["客户料号"] = dr["客户料号"].ToString();
                    dr1["品名"] = dr["品名"].ToString();
                    dr1["项次"] = dr["项次"].ToString();
                    
                  
                   
                    dr1["备注"] = dr["备注"].ToString();
 
                    dr1["合计销退数量"] = dt.Compute("SUM(销退数量)", "").ToString();
   

                    dr1["销退单号"] = dr["销退单号"].ToString();
                    dr1["销退日期"] = dr["销退日期"].ToString();
                    dr1["销退人"] = dr["销退人"].ToString();
                    dr1["销退数量"] = dr["销退数量"].ToString();
                    dtt.Rows.Add(dr1);
                    break;/*Only print rows one*/
                }
            }
            DataTable dt8 = bc.getdt(@"select  B.COKEY AS COKEY,A.COID AS COID,A.CONAME AS CONAME,B.PHONE AS PHONE,B.FAX AS FAX,
B.EMAIL AS MAIL,(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.MAKERID )  AS MAKER,
A.DATE AS DATE,B.ADDRESS AS ADDRESS,B.CONTACT AS CONTACT from 
COMPANYINFO_MST A LEFT JOIN COMPANYINFO_DET B ON A.COKEY=B.COKEY");
            if (dt8.Rows.Count > 0)
            {
                foreach (DataRow dr2 in dtt.Rows)
                {
                    dr2["公司名称"] = dt8.Rows[0]["CONAME"].ToString();
                    dr2["公司地址"] = dt8.Rows[0]["ADDRESS"].ToString();
                    dr2["公司电话"] = dt8.Rows[0]["PHONE"].ToString();
                    dr2["公司传真"] = dt8.Rows[0]["FAX"].ToString();
                }


            }
            return dtt;
        }
        #endregion
      
    }
}
