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
    public class CRETURN
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
A.REKEY AS 索引,
A.REID AS 退货单号,
A.PUID as 采购单号,
A.SN as 项次,
E.WareID as ID,
B.CO_WAREID AS 料号,
B.WNAME AS 品名,
B.CWAREID AS 客户料号,
B.SPEC as 规格,
B.UNIT as 单位,
C.PCOUNT AS 采购数量,
C.PURCHASEUNITPRICE AS 采购单价,
C.TAXRATE AS 税率,
E.MRCount as 退货数量 ,
A.NOTAX_AMOUNT AS 退货未税金额,
A.TAX_AMOUNT AS 退货税额,
A.AMOUNT AS 退货含税金额,
C.SUID as 供应商代码,
D.SNAME as 供应商名称 ,
F.Return_DATE AS 退货日期,
F.Return_ID AS 退货员工号,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.Return_ID )  AS 退货人,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=E.MAKERID )  AS 制单人,
E.DATE AS 制单日期,
A.REMARK AS 备注
from Return_DET A 
LEFT JOIN PURCHASE_DET C ON A.PUID=C.PUID AND A.SN=C.SN
LEFT JOIN SUPPLIERINFO_MST D ON C.SUID=D.SUID
LEFT JOIN MateRe  E ON A.REKEY=E.MRKEY
LEFT JOIN WAREINFO B ON E.WAREID=B.WAREID
LEFT JOIN Return_MST F ON A.REID=F.REID   
";/*此时退货没有退定指定仓库，直接从库存中扣减*/
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
        CPURCHASE_GODE cpurchase_gode = new CPURCHASE_GODE();
        public CRETURN()
        {
            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
            sqlf = setsqlf;
        }
        public string GETID()
        {
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM RETURN_MST", "REID", "RE");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        #region asko
        public DataTable asko(string REID)
        {
            DataTable dtt = cpurchase_gode.emptydt();
            DataTable dt = bc.getdt(sql + " WHERE A.REID='" + REID + "' ");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    dr["退货单号"] = dr1["退货单号"].ToString();
                    dr["采购单号"] = dr1["采购单号"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["品号"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["客户料号"] = dr1["客户料号"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["采购数量"] = dr1["采购数量"].ToString();
                    dr["退货数量"] = dr1["退货数量"].ToString();
                    dr["供应商名称"] = dr1["供应商名称"].ToString();
                    dr["退货日期"] = dr1["退货日期"].ToString();
                    dr["制单人"] = dr1["制单人"].ToString();
                    dr["制单日期"] = dr1["制单日期"].ToString();
                    dtt.Rows.Add(dr);
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
                    //dr2["公司传真"] = dt8.Rows[0]["FAX"].ToString();
                }


            }
            return dtt;
        }
        #endregion
      
    }
}
