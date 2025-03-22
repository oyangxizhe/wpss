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
    public class CORDER
    {
        basec bc = new basec();
        #region nature
        private string _getsql;
        public string getsql
        {
            set { _getsql = value; }
            get { return _getsql; }

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
        private  string _IDO;
        public  string IDO
        {
            set { _IDO = value; }
            get { return _IDO; }

        }
        private string _CO_COUNT;
        public string CO_COUNT
        {
            set { _CO_COUNT = value; }
            get { return _CO_COUNT; }

        }
        private string _WO_COUNT;
        public string WO_COUNT
        {
            set { _WO_COUNT = value; }
            get { return _WO_COUNT; }

        }
        private string _STORAGE_COUNT;
        public string STORAGE_COUNT
        {
            set { _STORAGE_COUNT = value; }
            get { return _STORAGE_COUNT; }

        }
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        #endregion
        string setsqlo = @"
SELECT
A.ORID AS 订单号,
A.SN AS 项次,
E.WAREID AS ID,
B.CO_WAREID AS 料号,
B.WNAME AS 品名,
B.CWAREID AS 客户料号,
B.SPEC AS 规格,
B.UNIT AS 销售单位,
RTRIM(CONVERT(DECIMAL(18,2),C.SELLUNITPRICE)) AS 销售单价,
RTRIM(CONVERT(DECIMAL(18,2),C.TAXRATE )) AS 税率,
RTRIM(CONVERT(DECIMAL(18,2),C.OCOUNT)) AS 订单数量 ,
RTRIM(CONVERT(DECIMAL(18,2),SUM(E.MRCOUNT))) AS 累计销货数量 ,
RTRIM(CONVERT(DECIMAL(18,2),SUM(E.MRCOUNT*C.SELLUNITPRICE))) AS 未税金额,
RTRIM(CONVERT(DECIMAL(18,2),SUM(E.MRCOUNT*C.SELLUNITPRICE*C.TAXRATE/100) )) AS 税额,
RTRIM(CONVERT(DECIMAL(18,2),SUM(E.MRCOUNT*C.SELLUNITPRICE*(1+C.TAXRATE/100)) )) AS 含税金额,
C.CUID AS 客户代码,
D.CNAME AS 客户名称 ,
F.ORDERDATE AS 订单日期,
F.DATE AS 制单日期,
F.ORDERSTATUS_MST AS 状态,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.SALEID )  AS 业务员
FROM SELLTABLE_DET A 
LEFT JOIN ORDER_DET C ON A.ORID=C.ORID AND A.SN=C.SN
LEFT JOIN CUSTOMERINFO_MST D ON C.CUID=D.CUID
LEFT JOIN MATERE E ON A.SEKEY=E.MRKEY
LEFT JOIN WAREINFO B ON E.WAREID=B.WAREID
LEFT JOIN ORDER_MST F ON C.ORID=F.ORID
";
        string setsqlt = @"
GROUP BY 
A.ORID,
A.SN,
E.WAREID,
B.CO_WAREID,
B.WNAME,
B.CWAREID,
B.SPEC,
B.UNIT,
C.SELLUNITPRICE,
C.TAXRATE,
C.OCOUNT,
C.CUID,
D.CNAME,
F.ORDERDATE,
F.DATE,
F.SALEID,
F.ORDERSTATUS_MST
ORDER BY 
A.ORID,
A.SN,
F.DATE
ASC
";
        DataTable dtx2 = new DataTable();
        DataTable dt4 = new DataTable();
        DataTable dt = new DataTable();
        int i,j;
        public CORDER()
        {
           
            sqlo = setsqlo;
            sqlt = setsqlt;
         

        }
        public bool IFNOALLOW_DELETE_ORID(string ORID)
        {
            bool b = false;
            if (bc.exists("SELECT * FROM CO_ORDER WHERE ORID='" + ORID + "'"))
            {
                b = true;
                ErrowInfo = "该订单号已经存在厂内订单中，不允许修改与删除！";
            }
            return b;
        }
       
     

        #region GET_TOTAL_ORDER
        public  DataTable GET_TOTAL_ORDER()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("索引", typeof(string));
            dtt.Columns.Add("订单号", typeof(string));
            dtt.Columns.Add("项次", typeof(string));
            dtt.Columns.Add("ID", typeof(string));
            dtt.Columns.Add("料号", typeof(string));
            dtt.Columns.Add("品名", typeof(string));
            dtt.Columns.Add("规格", typeof(string));
            dtt.Columns.Add("客户料号", typeof(string));
            dtt.Columns.Add("订单数量", typeof(decimal));
            dtt.Columns.Add("累计销货数量", typeof(decimal));
            dtt.Columns.Add("累计销退数量", typeof(decimal));
            dtt.Columns.Add("实际累计销货数量", typeof(decimal),"累计销货数量-累计销退数量");
            dtt.Columns.Add("订单未结数量", typeof(decimal), "订单数量-累计销货数量+累计销退数量");
            dtt.Columns.Add("状态", typeof(string));
            dtt.Columns.Add("交货日期", typeof(string));

            DataTable dtx1 = bc.getdt("SELECT * FROM ORDER_DET ");
            if (dtx1.Rows.Count > 0)
            {
                for (i = 0; i < dtx1.Rows.Count; i++)
                {
                    DataRow dr = dtt.NewRow();
                    dr["索引"] = dtx1.Rows[i]["ORKEY"].ToString();
                    dr["订单号"] = dtx1.Rows[i]["ORID"].ToString();
                    dr["项次"] = dtx1.Rows[i]["SN"].ToString();
                    dr["ID"] = dtx1.Rows[i]["WAREID"].ToString();
                    dtx2 = bc.getdt("select * from wareinfo where wareid='" + dtx1.Rows[i]["WAREID"].ToString() + "'");
                    dr["料号"] = dtx2.Rows[0]["CO_WAREID"].ToString();
                    dr["品名"] = dtx2.Rows[0]["WNAME"].ToString();
                    dr["规格"] = dtx2.Rows[0]["SPEC"].ToString();
                    dr["客户料号"] = dtx2.Rows[0]["CWAREID"].ToString();
                    dr["订单数量"] = dtx1.Rows[i]["OCOUNT"].ToString();
                    dr["累计销货数量"] = 0;
                    dr["累计销退数量"] = 0;
                    dr["交货日期"] = dtx1.Rows[i]["DELIVERYDATE"].ToString();
                    if (dtx1.Rows[i]["ORDERSTATUS_DET"].ToString() == "OPEN")
                    {
                        dr["状态"] = "OPEN";
                    }
                    else if (dtx1.Rows[i]["ORDERSTATUS_DET"].ToString() == "PROGRESS")
                    {
                        dr["状态"] = "部分出货";
                    }
                    else if (dtx1.Rows[i]["ORDERSTATUS_DET"].ToString() == "DELAY")
                    {
                        dr["状态"] = "DELAY";
                    }
                    else
                    {
                        dr["状态"] = "已出货";
                    }

                    dtt.Rows.Add(dr);
                }

            }

            DataTable dtx4 = bc.getdt(@"
SELECT
A.ORID AS ORID,
A.SN AS SN,
B.WAREID AS WAREID,
SUM(B.MRCOUNT) AS MRCOUNT 
FROM SELLTABLE_DET A 
LEFT JOIN MATERE B ON A.SEKEY=B.MRKEY 
GROUP BY A.ORID,A.SN,B.WAREID
");
            if (dtx4.Rows.Count > 0)
            {
                for (i = 0; i < dtx4.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["订单号"].ToString() == dtx4.Rows[i]["ORID"].ToString() && dtt.Rows[j]["项次"].ToString() == dtx4.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["累计销货数量"] = dtx4.Rows[i]["MRCOUNT"].ToString();
                            break;
                        }

                    }
                }

            }
            DataTable dtx6 = bc.getdt(@"
SELECT 
A.ORID AS ORID,
A.SN AS SN,
B.WAREID AS WAREID,
SUM(B.GECOUNT) AS GECOUNT
FROM SELLRETURN_DET A 
LEFT JOIN GODE B ON A.SRKEY=B.GEKEY  
GROUP BY 
A.ORID,
A.SN,
B.WAREID

");
            if (dtx6.Rows.Count > 0)
            {
                for (i = 0; i < dtx6.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["订单号"].ToString() == dtx6.Rows[i]["ORID"].ToString() && dtt.Rows[j]["项次"].ToString() == dtx6.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["累计销退数量"] = dtx6.Rows[i]["GECOUNT"].ToString();
                            break;
                        }

                    }
                }

            }

            return dtt;
        }
        #endregion
        #region GET_ORDER_PROGRESS_COUNT
        public string GET_ORDER_PROGRESS_COUNT(string WAREID,string ORKEY)
        {
            string v = "0";
            DataView dv = new DataView(GET_TOTAL_ORDER());
            dv.RowFilter = "状态 NOT IN ('已出货') AND ID='" + WAREID + "' AND 索引 NOT IN ('"+ORKEY +"')";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {

                v = dt.Compute("SUM(订单未结数量)", "").ToString();

            }
            return v;
        }
        #endregion

        #region UPDATE_ORDER_STATUS
        public void UPDATE_ORDER_STATUS(string ORID)
        {
            DataView dv = new DataView(GET_TOTAL_ORDER());
            dv.RowFilter = "订单号='" + ORID + "'";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    decimal d0 = decimal.Parse(dr["订单数量"].ToString());
                    decimal d1 = decimal.Parse(dr["累计销货数量"].ToString());
                    decimal d2 = decimal.Parse(dr["累计销退数量"].ToString());

                   if (decimal.Parse (dr["订单未结数量"].ToString()) ==0)
                    {
                        basec.getcoms("UPDATE ORDER_DET SET ORDERSTATUS_DET='CLOSE' WHERE ORID='" + ORID + "' AND SN='" +dr["项次"].ToString () + "'");
                    }
                    else if (bc.JuageCurrentDateIFAboveDeliveryDate(DateTime.Now.ToString(), dr["交货日期"].ToString()))
                    {
                        basec.getcoms("UPDATE ORDER_DET SET ORDERSTATUS_DET='DELAY' WHERE ORID='" + ORID + "' AND SN='" + dr["项次"].ToString() + "'");
                    }
                    else if (d1 - d2 > 0)
                    {
                        basec.getcoms("UPDATE ORDER_DET SET ORDERSTATUS_DET='PROGRESS' WHERE ORID='" + ORID + "' AND SN='" + dr["项次"].ToString() + "'");
                    }
                    else
                    {
                        basec.getcoms("UPDATE ORDER_DET SET ORDERSTATUS_DET='OPEN' WHERE ORID='" + ORID + "' AND SN='" + dr["项次"].ToString() + "'");
                    }
                }
                if (bc.JuageOrderOrPurchaseStatus(ORID, 0))
                {
                    basec.getcoms("UPDATE ORDER_MST SET ORDERSTATUS_MST='CLOSE' WHERE ORID='" + ORID + "'");

                }
                else if (bc.JuageCurrentDateIFAboveDeliveryDate(ORID, 0))
                {
                    basec.getcoms("UPDATE ORDER_MST SET ORDERSTATUS_MST='DELAY' WHERE ORID='" + ORID + "'");
                }
                else if (JUAGE_REALTY_IFHAVE_SELLCOUNT(ORID))
                {

                    basec.getcoms("UPDATE ORDER_MST SET ORDERSTATUS_MST='PROGRESS' WHERE ORID='" + ORID + "'");
                }
                else
                {
                    basec.getcoms("UPDATE ORDER_MST SET ORDERSTATUS_MST='OPEN' WHERE ORID='" + ORID + "'");

                }
            }
        }
        #endregion
        #region JUAGE_REALTY_IFHAVE_SELLCOUNT
        public bool  JUAGE_REALTY_IFHAVE_SELLCOUNT(string ORID)
        {
            bool b = false;
            DataView dv = new DataView(GET_TOTAL_ORDER());
            dv.RowFilter = "订单号='" + ORID + "'";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    decimal d1 = decimal.Parse(dr["累计销货数量"].ToString());
                    decimal d2 = decimal.Parse(dr["累计销退数量"].ToString());
                    if (d1 - d2 > 0)
                    {
                        b = true;
                        break;
                    }

                }
            }
            return b;
        }
        #endregion
        #region JUAGE_ORDER_IF_HAVE_NO_AUDIT
        public bool JUAGE_ORDER_IF_HAVE_NO_AUDIT(string ORID)
        {
            bool b = false;
            string s2 = bc.getOnlyString("SELECT IF_AUDIT FROM ORDER_MST WHERE ORID='" +ORID  + "'");
            if (s2 != "Y")
            {
                b = true;
                ErrowInfo = "此订单未审核，不能进行相关操作！";
            }
            return b;
        }
        #endregion
        
    }
}
