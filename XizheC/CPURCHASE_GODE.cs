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

namespace XizheC
{
    public class CPURCHASE_GODE
    {
        basec bc = new basec();
    
        #region nature
        public  string _GETID;
        public  string GETID
        {
            set { _GETID =value ; }
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
        private string _MAKERID;
        public string MAKERID
        {
            set { _MAKERID = value; }
            get { return _MAKERID; }

        }
  
        #endregion
        private static bool _IFExecutionSUCCESS;
        public static bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private string _WP_COUNT;
        public string WP_COUNT
        {

            set { _WP_COUNT = value; }
            get { return _WP_COUNT; }

        }
  
        DataTable dtx2 = new DataTable();
        int i, j;
        CPURCHASE cpurchase = new CPURCHASE();
        #region sql
        string setsql = @"
SELECT
A.PGID AS 入库单号,
A.PUID AS 采购单号,
A.SN AS 项次,
E.WAREID AS ID,
B.WNAME AS 品名,
B.CO_WAREID AS 料号,
B.CWAREID AS 客户料号,
B.SPEC AS 规格,
C.PCOUNT AS 采购数量,
B.Unit AS 单位,
E.GECOUNT AS 入库数量 ,
E.FREECOUNT AS FREE数量,
C.SUID AS 供应商代码,
D.SNAME AS 供应商名称 ,
G.STORAGENAME AS 仓库,
E.BATCHID AS 批号,
F.GODEDATE AS 入库日期,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.GODERID )  AS 入库员,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=E.MAKERID )  AS 制单人,
E.DATE AS 制单日期,
A.REMARK AS 备注
FROM PURCHASEGODE_DET A 
LEFT JOIN PURCHASE_DET C ON A.PUID=C.PUID AND A.SN=C.SN
LEFT JOIN SUPPLIERINFO_MST D ON C.SUID=D.SUID
LEFT JOIN GODE E ON A.PGKEY=E.GEKEY
LEFT JOIN PURCHASEGODE_MST F ON A.PGID=F.PGID
LEFT JOIN WAREINFO B ON E.WAREID=B.WAREID
LEFT JOIN STORAGEINFO G ON E.STORAGEID=G.STORAGEID


";
        
        string setsqlo = @"
INSERT INTO PURCHASEGODE_DET
(
PGKEY,
PGID,
PUKEY,
PUID,
SN,
REMARK,
YEAR,
MONTH,
DAY
)
VALUES
(
@PGKEY,
@PGID,
@PUKEY,
@PUID,
@SN,
@REMARK,
@YEAR,
@MONTH,
@DAY
)
";

        string setsqlt = @"
INSERT INTO PURCHASEGODE_MST
(
PGID,
GODEDATE,
GODERID,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@PGID,
@GODEDATE,
@GODERID,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        string setsqlth = @"
UPDATE PURCHASEGODE_MST SET 
PGID=@PGID,
GODEDATE=@GODEDATE,
GODERID=@GODERID,
DATE=@DATE,
MAKERID=@MAKERID,
YEAR=@YEAR,
MONTH=@MONTH,
DAY=@DAY
";
             
        string setsqlf = @"
INSERT INTO GODE
(
GEKEY,
GODEID,
SN,
WAREID,
P_GECOUNT,
MPA_UNIT,
GECOUNT,
SKU,
BOM_GECOUNT,
BOM_UNIT,
FREECOUNT,
STORAGEID,
SLID,
BATCHID,
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
@SN,
@WAREID,
@P_GECOUNT,
@MPA_UNIT,
@GECOUNT,
@SKU,
@BOM_GECOUNT,
@BOM_UNIT,
@FREECOUNT,
@STORAGEID,
@SLID,
@BATCHID,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY

)
";
         string setsqlfi = @"

";
        #endregion
         public CPURCHASE_GODE()
        {
            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            //GETID =bc.numYM(10, 4, "0001", "SELECT * FROM WORKORDER_PICKING_MST", "WPID", "WP");
     
             sql= setsql;
            sqlo=setsqlo;
            sqlt=setsqlt;
            sqlth=setsqlth;
            sqlf= setsqlf;
            sqlfi=setsqlfi;
        }

        #region ask
        public DataTable ask(string wpid)
        {
            string sql1 = sqlo;
            DataTable dtt = bc.getdt(sqlfi + " WHERE A.WPID='" + wpid  + "' ORDER BY A.WPKEY ASC");
            return dtt;
        }
        #endregion

        #region purchasedt  /*crystalprint 1/2*/
        public DataTable emptydt()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("公司名称", typeof(string));
            dt.Columns.Add("公司联系人", typeof(string));
            dt.Columns.Add("公司电话", typeof(string));
            dt.Columns.Add("公司地址", typeof(string));
            dt.Columns.Add("供应商名称", typeof(string));
            dt.Columns.Add("联系人", typeof(string));
            dt.Columns.Add("电话", typeof(string));
            dt.Columns.Add("地址", typeof(string));
            dt.Columns.Add("入库单号", typeof(string));
            dt.Columns.Add("采购单号", typeof(string));
            dt.Columns.Add("项次", typeof(string));
            dt.Columns.Add("品号", typeof(string));
            dt.Columns.Add("料号", typeof(string));
            dt.Columns.Add("品名", typeof(string));
            dt.Columns.Add("客户料号", typeof(string));
            dt.Columns.Add("规格", typeof(string));
            dt.Columns.Add("采购数量", typeof(decimal));
            dt.Columns.Add("累计入库数量", typeof(decimal));
            dt.Columns.Add("累计退货数量", typeof(decimal));
            dt.Columns.Add("未入库数量", typeof(decimal), "采购数量-累计入库数量+累计退货数量");
            dt.Columns.Add("入库数量", typeof(decimal));
            dt.Columns.Add("退货数量", typeof(decimal));
            dt.Columns.Add("仓库", typeof(string));
            dt.Columns.Add("批号", typeof(string));
            dt.Columns.Add("本入库单累计入库数量", typeof(decimal));
            dt.Columns.Add("入库日期", typeof(string));
            dt.Columns.Add("制单人", typeof(string));
            dt.Columns.Add("制单日期", typeof(string));
            dt.Columns.Add("退货单号", typeof(string));
            dt.Columns.Add("退货日期", typeof(string));
            dt.Columns.Add("退货人", typeof(string));
        
            return dt;
        }
        #endregion
        #region ask
        public  DataTable ask(string v1, string v2)
        {
            DataTable dtt = emptydt();
          
            DataTable dtx1 = bc.getdt("SELECT * FROM PURCHASE_DET WHERE PUID='" + v2 + "'");
            if (dtx1.Rows.Count > 0)
            {
                for (i = 0; i < dtx1.Rows.Count; i++)
                {
                    DataRow dr = dtt.NewRow();
                    dr["采购单号"] = dtx1.Rows[i]["PUID"].ToString();
                    dr["项次"] = dtx1.Rows[i]["SN"].ToString();
                    dr["品号"] = dtx1.Rows[i]["WAREID"].ToString();
                    dtx2 = bc.getdt("select * from wareinfo where wareid='" + dtx1.Rows[i]["WAREID"].ToString() + "'");
                    dr["料号"] = dtx2.Rows[0]["CO_WAREID"].ToString();
                    dr["品名"] = dtx2.Rows[0]["WNAME"].ToString();
                    dr["客户料号"] = dtx2.Rows[0]["CWAREID"].ToString();
                    dr["采购数量"] = dtx1.Rows[i]["PCOUNT"].ToString();
                    dr["累计入库数量"] = 0;
                    dr["累计退货数量"] = 0;
                    dr["本入库单累计入库数量"] = 0;
                    dr["入库单号"] = v1;
                    dr["规格"] = dtx2.Rows[0]["SPEC"].ToString();
                    dtt.Rows.Add(dr);

                }

            }

            DataTable dtx4 = bc.getdt(@"
SELECT A.PUID AS PUID,A.SN AS SN,B.WAREID AS WAREID,CAST(ROUND(SUM(B.GECOUNT),2) AS DECIMAL(18,2)) AS GECOUNT FROM PURCHASEGODE_DET A 
LEFT JOIN GODE B ON A.PGKEY=B.GEKEY  WHERE  A.PUID='" + v2 + "' GROUP BY A.PUID,A.SN,B.WAREID");
            if (dtx4.Rows.Count > 0)
            {
                for (i = 0; i < dtx4.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["采购单号"].ToString() == dtx4.Rows[i]["PUID"].ToString() && dtt.Rows[j]["项次"].ToString() == dtx4.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["累计入库数量"] = dtx4.Rows[i]["GECOUNT"].ToString();
                            break;
                        }

                    }
                }

            }
            DataTable dtx7 = bc.getdt(@"
SELECT 
A.PUID AS PUID,
A.SN AS SN,
B.WAREID AS WAREID,
CAST(ROUND(SUM(B.MRCOUNT),2) AS DECIMAL(18,2)) AS P_MRCOUNT 
FROM RETURN_DET A 
LEFT JOIN MATERE B ON A.REKEY=B.MRKEY  
WHERE  A.PUID='" + v2 + "' GROUP BY A.PUID,A.SN,B.WAREID");
            if (dtx7.Rows.Count > 0)
            {
                for (i = 0; i < dtx7.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["采购单号"].ToString() == dtx7.Rows[i]["PUID"].ToString() &&
                            dtt.Rows[j]["项次"].ToString() == dtx7.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["累计退货数量"] = dtx7.Rows[i]["P_MRCOUNT"].ToString();
                            break;
                        }

                    }
                }

            }
            DataTable dtx5 = bc.getdt(@"SELECT A.PUID AS PUID,A.PGID AS PGID,A.SN AS SN,B.WAREID AS WAREID,
CAST(ROUND(SUM(B.GECOUNT),2) AS DECIMAL(18,2)) AS GECOUNT FROM PURCHASEGODE_DET A 
LEFT JOIN GODE B ON A.PGKEY=B.GEKEY  WHERE  A.PUID='" + v2 + "' AND A.PGID='" + v1 + "' GROUP BY A.PUID,A.PGID,A.SN,B.WAREID");
            if (dtx5.Rows.Count > 0)
            {
                for (i = 0; i < dtx5.Rows.Count; i++)
                {
                    for (j = 0; j < dtt.Rows.Count; j++)
                    {
                        if (dtt.Rows[j]["采购单号"].ToString() == dtx5.Rows[i]["PUID"].ToString() &&
                            dtt.Rows[j]["项次"].ToString() == dtx5.Rows[i]["SN"].ToString())
                        {
                            dtt.Rows[j]["本入库单累计入库数量"] = dtx5.Rows[i]["GECOUNT"].ToString();
                            break;
                        }

                    }
                }

            }
            return dtt;
        }
        #endregion
        #region asko
        public DataTable asko(string PGID)
        {
            DataTable dtt = this.emptydt();
            DataTable dt = bc.getdt(sql + " WHERE A.PGID='" + PGID + "' ");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    dr["入库单号"] = dr1["入库单号"].ToString();
                    dr["采购单号"] = dr1["采购单号"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["品号"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["客户料号"] = dr1["客户料号"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["采购数量"] = dr1["采购数量"].ToString();
                    dr["入库数量"] = dr1["入库数量"].ToString();
                    dr["仓库"] = dr1["仓库"].ToString();
                    dr["批号"] = dr1["批号"].ToString();
                    dr["供应商名称"] = dr1["供应商名称"].ToString();
                    dr["入库日期"] = dr1["入库日期"].ToString();
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
