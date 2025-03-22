
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
    public class CPURCHASE
    {
        basec bc = new basec();
        #region nature
        public string _GETID;
        public string GETID
        {
            set { _GETID = value; }
            get { return _GETID; }

        }
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
        private string _MAKERID;
        public string MAKERID
        {
            set { _MAKERID = value; }
            get { return _MAKERID; }

        }
        private string _PUID;
        public string PUID
        {
            set { _PUID = value; }
            get { return _PUID; }

        }
        private decimal  _PURCHASEUNITPRICE;
        public  decimal  PURCHASEUNITPRICE
        {
            set { _PURCHASEUNITPRICE = value; }
            get { return _PURCHASEUNITPRICE; }

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
A.PUID AS PUID,
B.SUID AS SUID,
B.SNAME AS SNAME,
A.PDATE AS PDATE,
C.WAREID AS WAREID,
CASE WHEN A.IF_AUDIT='Y' THEN '已审核'
ELSE '未审核'
END
AS IF_AUDIT,
CASE WHEN A.PURCHASESTATUS_MST='CLOSE' THEN '已入库'
WHEN A.PURCHASESTATUS_MST='PROGRESS' THEN '部分入库'
WHEN A.PURCHASESTATUS_MST='DELAY' THEN 'DELAY'
ELSE 'OPEN'
END  AS PURCHASESTATUS_MST,
C.NEEDDATE AS NEEDDATE,
E.CNAME AS CNAME,
D.WNAME AS WNAME,
D.CWAREID AS CWAREID,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.PURID ) AS PUR,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.MAKERID ) AS MAKER,
A.DATE AS DATE FROM PURCHASE_MST A  
LEFT JOIN SUPPLIERINFO_MST B ON A.SUID=B.SUID
LEFT JOIN PURCHASE_DET C ON A.PUID =C.PUID 
LEFT JOIN WAREINFO D ON C.WAREID =D.WAREID 
LEFT JOIN CUSTOMERINFO_MST E ON D.CUID =E.CUID
";
        string setsqlo = @"
INSERT INTO PURCHASE_DET(
PUKEY,
PUID,
SN,
WAREID,
PCOUNT,
PURCHASEUNITPRICE,
CURRENCY,
TAXRATE,
SUID,
NEEDDATE,
PURCHASESTATUS_DET,
MPA_UNIT,
MPA_TO_STOCK,
STOCK_TO_BOM,
SKU,
BOM_UNIT,
REMARK,
YEAR,
MONTH,
DAY
) 
VALUES 
(
@PUKEY,
@PUID,
@SN,
@WAREID,
@PCOUNT,
@PURCHASEUNITPRICE,
@CURRENCY,
@TAXRATE,
@SUID,
@NEEDDATE,
@PURCHASESTATUS_DET,
@MPA_UNIT,
@MPA_TO_STOCK,
@STOCK_TO_BOM,
@SKU,
@BOM_UNIT,
@REMARK,
@YEAR,
@MONTH,
@DAY
)
";

        string setsqlt = @"
INSERT INTO PURCHASE_MST
(
PUID,
SUID,
PDATE,
PURID,
SOURCESTATUS,
PURCHASESTATUS_MST,
RDID,
COKEY,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@PUID,
@SUID,
@PDATE,
@PURID,
@SOURCESTATUS,
@PURCHASESTATUS_MST,
@RDID,
@COKEY,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        string setsqlth = @"
SELECT  
B.COKEY AS COKEY,
A.COID AS COID,
A.CONAME AS CONAME,
B.PHONE AS PHONE,
B.FAX AS FAX,
B.EMAIL AS MAIL,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.MAKERID )  AS MAKER,
A.DATE AS DATE,
B.ADDRESS AS ADDRESS,
B.CONTACT AS CONTACT
 FROM 
COMPANYINFO_MST A 
LEFT JOIN COMPANYINFO_DET B ON A.COKEY=B.COKEY
";
        string setsqlf = @"
SELECT 
cast(0   as   bit)   as   选择,
A.PUID AS 采购单号,
A.SN AS 项次,
E.WAREID AS ID,
B.CO_WAREID AS 料号,
B.WNAME AS 品名,
B.CWAREID AS 客户料号,
RTRIM(CONVERT(DECIMAL(18,2),C.PURCHASEUNITPRICE))AS 采购单价,
B.UNIT AS 采购单位,
C.PCOUNT AS 采购数量,
C.TAXRATE AS 税率,
RTRIM(CONVERT(DECIMAL(18,2),SUM(E.GECOUNT)))AS 累计入库数量,
RTRIM(CONVERT(DECIMAL(18,2),SUM(C.PURCHASEUNITPRICE*E.GECOUNT) )) AS 未税金额,
RTRIM(CONVERT(DECIMAL(18,2),SUM(C.PURCHASEUNITPRICE*E.GECOUNT*C.TAXRATE/100) )) AS 税额,
RTRIM(CONVERT(DECIMAL(18,2),SUM(C.PURCHASEUNITPRICE*E.GECOUNT*(1+C.TAXRATE/100))))AS 含税金额,
C.SUID AS 供应商代码,
D.SNAME AS 供应商名称,
F.PDATE AS 采购日期,
F.DATE AS 采购制单日期,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.PURID ) AS 采购员
FROM PURCHASEGODE_DET A 
LEFT JOIN PURCHASE_DET C ON A.PUID=C.PUID AND A.SN=C.SN
LEFT JOIN SUPPLIERINFO_MST D ON C.SUID=D.SUID
LEFT JOIN GODE E ON A.PGKEY=E.GEKEY
LEFT JOIN WAREINFO B ON E.WAREID=B.WAREID
LEFT JOIN PURCHASE_MST F ON F.PUID=C.PUID 
";
        string setsqlfi= @"
GROUP BY
A.PUID,
A.SN,
E.WAREID,
B.CO_WAREID,
B.WNAME,
B.CWAREID,
B.UNIT,
C.PURCHASEUNITPRICE,
C.PCOUNT,
C.SUID,
D.SNAME,
F.PDATE,
F.PURID,
C.TAXRATE,
F.DATE 
ORDER BY 
A.PUID,
A.SN
";
        DataTable dtx2 = new DataTable();
        DataTable dt4 = new DataTable();
        int i, j;
        public CPURCHASE()
        {
            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            GETID = bc.numYM(10, 4, "0001", "SELECT * FROM PURCHASE_MST", "PUID", "PU");
            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
            sqlf = setsqlf;
            sqlfi = setsqlfi;
            
        }
        public void save(DataTable dt)
        {

          
        }
   

        #region GET_TOTAL_PURCHASE
        public DataTable GET_TOTAL_PURCHASE()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("采购单号", typeof(string));
            dtt.Columns.Add("项次", typeof(string));
            dtt.Columns.Add("ID", typeof(string));
            dtt.Columns.Add("料号", typeof(string));
            dtt.Columns.Add("品名", typeof(string));
            dtt.Columns.Add("规格", typeof(string));
            dtt.Columns.Add("客户料号", typeof(string));
            dtt.Columns.Add("采购数量", typeof(decimal));
            dtt.Columns.Add("采购单位", typeof(string));
            dtt.Columns.Add("采购库存换算", typeof(decimal));
            dtt.Columns.Add("采购折合库存", typeof(decimal), "采购数量*采购库存换算");
            dtt.Columns.Add("库存单位", typeof(string));
            dtt.Columns.Add("累计采购入库数量P", typeof(decimal));
            dtt.Columns.Add("累计采购入库数量S", typeof(decimal));
            dtt.Columns.Add("累计退货数量P", typeof(decimal));
            dtt.Columns.Add("累计退货数量S", typeof(decimal));
            dtt.Columns.Add("采购未结数量P", typeof(decimal), "采购数量-累计采购入库数量P+累计退货数量P");
            dtt.Columns.Add("采购未结数量S", typeof(decimal), "采购数量*采购库存换算-累计采购入库数量S+累计退货数量S");
            dtt.Columns.Add("状态", typeof(string));
            dtt.Columns.Add("需求日期", typeof(string));
            DataTable dtx1 = bc.getdt("SELECT * FROM PURCHASE_DET");
            if (dtx1.Rows.Count > 0)
            {
                for (i = 0; i < dtx1.Rows.Count; i++)
                {
                    DataRow dr = dtt.NewRow();
                    dr["采购单号"] = dtx1.Rows[i]["PUID"].ToString();
                    dr["项次"] = dtx1.Rows[i]["SN"].ToString();
                    dr["ID"] = dtx1.Rows[i]["WAREID"].ToString();
                    dtx2 = bc.getdt("SELECT * FROM WAREINFO WHERE WAREID='" + dtx1.Rows[i]["WAREID"].ToString() + "'");
                    dr["料号"] = dtx2.Rows[0]["CO_WAREID"].ToString();
                    dr["品名"] = dtx2.Rows[0]["WNAME"].ToString();
                    dr["规格"] = dtx2.Rows[0]["SPEC"].ToString();
                    dr["客户料号"] = dtx2.Rows[0]["CWAREID"].ToString();
                    dr["采购数量"] = dtx1.Rows[i]["PCOUNT"].ToString();
                    //dr["采购单位"] = dtx1.Rows[i]["MPA_UNIT"].ToString();
                    //dr["库存单位"] = dtx1.Rows[i]["SKU"].ToString();
                    //dr["采购库存换算"] = dtx1.Rows[i]["MPA_TO_STOCK"].ToString();
                    dr["累计采购入库数量P"] = 0;
                    dr["累计采购入库数量S"] = 0;
                    dr["累计退货数量P"] = 0;
                    dr["累计退货数量S"] = 0;
                    dr["需求日期"] = dtx1.Rows[i]["NEEDDATE"].ToString();
                    if (dtx1.Rows[i]["PURCHASESTATUS_DET"].ToString() == "OPEN")
                    {
                        dr["状态"] = "OPEN";
                    }
                    else if (dtx1.Rows[i]["PURCHASESTATUS_DET"].ToString() == "PROGRESS")
                    {
                        dr["状态"] = "部分入库";
                    }
                    else if (dtx1.Rows[i]["PURCHASESTATUS_DET"].ToString() == "DELAY")
                    {
                        dr["状态"] = "DELAY";
                    }
                    else
                    {
                        dr["状态"] = "已入库";
                    }
                    dtt.Rows.Add(dr);
                }
            }

            DataTable dtx4 = bc.getdt(@"
SELECT 
A.PUID AS PUID,
A.SN AS SN,
B.WAREID AS WAREID,
SUM(B.GECOUNT) AS P_GECOUNT,
SUM(B.GECOUNT) AS GECOUNT
FROM PURCHASEGODE_DET A 
LEFT JOIN GODE B ON A.PGKEY=B.GEKEY
GROUP BY
A.PUID,
A.SN,
B.WAREID
");
            if (dtx4.Rows.Count > 0)
            {
                for (i = 0; i < dtx4.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["采购单号"].ToString() == dtx4.Rows[i]["PUID"].ToString() &&
                            dtt.Rows[j]["项次"].ToString() == dtx4.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["累计采购入库数量P"] = dtx4.Rows[i]["P_GECOUNT"].ToString();
                            dtt.Rows[j]["累计采购入库数量S"] = dtx4.Rows[i]["GECOUNT"].ToString();
                            break;
                        }
                    }
                }
            }
            DataTable dtx6 = bc.getdt(@"
SELECT 
A.PUID AS PUID,
A.SN AS SN,
B.WAREID AS WAREID,
CASE WHEN SUM(B.MRCount) IS NULL THEN 0
ELSE SUM(B.MRCount)
END
AS P_MRCOUNT,
CASE WHEN SUM(B.MRCount) IS NULL THEN 0
ELSE SUM(B.MRCount)
END
AS MRCOUNT
FROM RETURN_DET A 
LEFT JOIN MateRe  B ON A.REKEY=B.MRKEY
LEFT JOIN Purchase_DET D ON D.PUID=A.PUID AND D.SN=A.SN 
GROUP BY 
A.PUID,
A.SN,
B.WAREID
");/*NORMAL P_MRCOUNT NOT NULL ,BUT DB EDIT*/
            if (dtx6.Rows.Count > 0)
            {
                for (i = 0; i < dtx6.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["采购单号"].ToString() == dtx6.Rows[i]["PUID"].ToString() &&
                            dtt.Rows[j]["项次"].ToString() == dtx6.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["累计退货数量P"] = dtx6.Rows[i]["P_MRCOUNT"].ToString();
                            dtt.Rows[j]["累计退货数量S"] = dtx6.Rows[i]["MRCOUNT"].ToString();
                            break;
                        }
                    }
                }
            }
            return dtt;
        }
        #endregion

        #region GET_PURCHASE_ON_THE_WAY_COUNT
        public string GET_PURCHASE_ON_THE_WAY_COUNT(string WAREID)
        {
            string v = "0";//USER FOR BOM UNIT SKU
            DataView dv = new DataView(GET_TOTAL_PURCHASE());
            string SKU = bc.getOnlyString("SELECT SKU FROM WAREINFO WHERE WAREID='" + WAREID + "'");
            dv.RowFilter = "状态 NOT IN ('已入库') AND ID='" + WAREID + "' AND 库存单位='" + SKU + "'";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {

                v = dt.Compute("SUM(采购未结数量S)", "").ToString();

            }
            return v;
        }
        #endregion

        #region UPDATE_PURCHASE_STATUS
        public void UPDATE_PURCHASE_STATUS(string PUID)
        {
            DataView dv = new DataView(GET_TOTAL_PURCHASE());
            dv.RowFilter = "采购单号='" + PUID + "'";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    decimal d0 = decimal.Parse(dr["采购数量"].ToString());
                    decimal d1 = decimal.Parse(dr["累计采购入库数量P"].ToString());
                    decimal d2 = decimal.Parse(dr["累计退货数量S"].ToString());

                    if (decimal.Parse(dr["采购未结数量P"].ToString()) == 0)
                    {
                        basec.getcoms("UPDATE PURCHASE_DET SET PURCHASESTATUS_DET='CLOSE' WHERE PUID='" + PUID + "' AND SN='" + dr["项次"].ToString() + "'");
                    }
                    else if (bc.JuageCurrentDateIFAboveDeliveryDate(DateTime.Now.ToString(), dr["需求日期"].ToString()))
                    {
                        basec.getcoms("UPDATE PURCHASE_DET SET PURCHASESTATUS_DET='DELAY' WHERE PUID='" + PUID + "' AND SN='" + dr["项次"].ToString() + "'");
                    }
                    else if (d1 - d2 > 0)
                    {
                        basec.getcoms("UPDATE PURCHASE_DET SET PURCHASESTATUS_DET='PROGRESS' WHERE PUID='" + PUID + "' AND SN='" + dr["项次"].ToString() + "'");
                    }
                    else
                    {
                        basec.getcoms("UPDATE PURCHASE_DET SET PURCHASESTATUS_DET='OPEN' WHERE PUID='" + PUID + "' AND SN='" + dr["项次"].ToString() + "'");
                    }
                }
                if (bc.JuageOrderOrPurchaseStatus(PUID, 1))
                {
                    basec.getcoms("UPDATE PURCHASE_MST SET PURCHASESTATUS_MST='CLOSE' WHERE PUID='" + PUID + "'");

                }
                else if (bc.JuageCurrentDateIFAboveDeliveryDate(PUID, 1))
                {
                    basec.getcoms("UPDATE PURCHASE_MST SET PURCHASESTATUS_MST='DELAY' WHERE PUID='" + PUID + "'");
                }
                else if (JUAGE_REALTY_IFHAVE_PURCHASE_GODE_COUNT(PUID))
                {

                    basec.getcoms("UPDATE PURCHASE_MST SET PURCHASESTATUS_MST='PROGRESS' WHERE PUID='" + PUID + "'");
                }
                else
                {
                    basec.getcoms("UPDATE PURCHASE_MST SET PURCHASESTATUS_MST='OPEN' WHERE PUID='" + PUID + "'");

                }
            }
        }
        #endregion

        #region JUAGE_REALTY_IFHAVE_PURCHASE_GODE_COUNT
        public bool JUAGE_REALTY_IFHAVE_PURCHASE_GODE_COUNT(string PUID)
        {
            bool b = false;
            DataView dv = new DataView(GET_TOTAL_PURCHASE());
            dv.RowFilter = "采购单号='" + PUID + "'";
            DataTable dt = dv.ToTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    decimal d1 = decimal.Parse(dr["累计采购入库数量P"].ToString());
                    decimal d2 = decimal.Parse(dr["累计退货数量P"].ToString());
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
        #region JUAGE_PURCHASE_IF_AREADY_AUDIT
        public bool JUAGE_PURCHASE_IF_AREADY_AUDIT(string PUID)
        {
            bool b = false;
            string s2 = bc.getOnlyString("SELECT IF_AUDIT FROM PURCHASE_MST WHERE PUID='" + PUID + "'");
            if (s2 == "Y")
            {
                b = true;
            }
            else
            {
                ErrowInfo = "此采购单未审核，不能进行相关操作！";

            }
            return b;
        }
        #endregion

  
    }
}
