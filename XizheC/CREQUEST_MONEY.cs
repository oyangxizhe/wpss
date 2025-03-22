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
    public class CREQUEST_MONEY
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
A.RMID AS 请款单号,
A.INVOICE_NO AS 发票号码,
A.INVOICE_NOTAX_AMOUNT AS 发票未税,
A.INVOICE_TAX_AMOUNT AS 发票税额,
A.INVOICE_HAVETAX_AMOUNT AS 发票含税,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.REQUEST_MONEY_MAKERID) AS 请款人,
A.REQUEST_MONEY_DATE AS 请款日期,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.REQUEST_MONEY_MAKERID) AS 制单人,
A.DATE AS 制单日期
FROM REQUEST_MONEY_MST  A

";
        #endregion
        #region setsqlo
        string setsqlo = @"
INSERT INTO REQUEST_MONEY_DET
(
RMKEY,
RMID,
PRKEY,
YEAR,
MONTH,
DAY
)
VALUES
(
@RMKEY,
@RMID,
@PRKEY,
@YEAR,
@MONTH,
@DAY

)
";
        #endregion
        #region setsqlt
        string setsqlt = @"
INSERT INTO REQUEST_MONEY_MST
(
RMID,
INVOICE_NO,
INVOICE_NOTAX_AMOUNT,
INVOICE_TAX_AMOUNT,
INVOICE_HAVETAX_AMOUNT,
REQUEST_MONEY_DATE,
REQUEST_MONEY_MAKERID,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@RMID,
@INVOICE_NO,
@INVOICE_NOTAX_AMOUNT,
@INVOICE_TAX_AMOUNT,
@INVOICE_HAVETAX_AMOUNT,
@REQUEST_MONEY_DATE,
@REQUEST_MONEY_MAKERID,
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
SELECT 
B.RMKEY AS 请款索引,
A.RMID AS 请款单号,
A.INVOICE_NO AS 发票号码,
A.INVOICE_NOTAX_AMOUNT AS 发票未税金额,
A.INVOICE_TAX_AMOUNT AS 发票税额,
A.INVOICE_HAVETAX_AMOUNT AS 发票含税金额,
D.PUID AS 采购单号,
G.SN AS 项次,
H.WareID AS ID,
H.CO_WAREID AS 料号,
H.WName AS 品名,
H.SPEC AS 规格,
G.SKU AS 采购单位,
D.PCount AS 采购数量,
E.PDATE AS 采购日期,
D.PurchaseUnitPrice AS 采购单价,
D.TaxRate AS 税率,
B.PRKEY AS 索引,
C.PGID AS 入库退货单号,
G.GECount AS 入库退货数量,
RTRIM(CONVERT(DECIMAL(18,2),G.GECount * D.PurchaseUnitPrice  )) AS 未税金额,
RTRIM(CONVERT(DECIMAL(18,2),G.GECount * D.PurchaseUnitPrice * D.TaxRate/100  )) AS 税额,
RTRIM(CONVERT(DECIMAL(18,2),G.GECount * D.PurchaseUnitPrice * (1+D.TaxRate/100) )) AS 含税金额,
F.SUID AS 供应商代码,
F.SNAME AS 供应商名称,
A.CUTPAYMENT_PROJECT AS 扣款项目,
A.CUTPAYMENT_AMOUNT AS 扣款金额,
A.REQUEST_MONEY_MAKERID 请款人工号,
(SELECT ENAME FROM EmployeeInfo WHERE EMID=A.REQUEST_MONEY_MAKERID ) AS 请款人,
A.REQUEST_MONEY_DATE AS 请款日期,
(SELECT ENAME FROM EmployeeInfo WHERE EMID=A.MakerID ) AS 制单人,
A.Date AS 制单日期
FROM REQUEST_MONEY_MST A 
LEFT JOIN REQUEST_MONEY_DET B ON A.RMID=B.RMID
LEFT JOIN PurchaseGode_DET C ON B.PRKEY =C.PGKEY 
LEFT JOIN Purchase_DET D ON D.PUID=C.PUID AND C.SN=D.SN 
LEFT JOIN Purchase_MST E ON D.PUID=E.PUID 
LEFT JOIN SupplierInfo_MST F ON E.SUID =F.SUID 
LEFT JOIN Gode G ON C.PGKEY =G.GEKEY 
LEFT JOIN WareInfo H ON G.WareID =H.WareID 

";
        #endregion
        #region setsqlf
        string setsqlf = @"
SELECT 
A.PGKEY AS 索引,
A.PGID AS 入库单号,
A.PUID AS 采购单号, 
A.SN AS 项次,
E.WAREID AS ID,
B.CO_WAREID AS 料号,
B.WNAME AS 品名,
B.Spec AS 规格,
E.SKU AS 采购单位,
C.PurchaseUnitPrice AS 采购单价,
C.PCOUNT AS 采购数量,
C.TaxRate as  税率,
E.GECOUNT AS 入库数量 ,
RTRIM(CONVERT(DECIMAL(18,2),E.GECount * C.PurchaseUnitPrice  )) AS 未税金额,
RTRIM(CONVERT(DECIMAL(18,2),E.GECount * C.PurchaseUnitPrice * C.TaxRate/100  )) AS 税额,
RTRIM(CONVERT(DECIMAL(18,2),E.GECount * C.PurchaseUnitPrice * (1+C.TaxRate/100) )) AS 含税金额,
C.SUID AS 供应商代码,
D.SNAME AS 供应商名称,
CASE WHEN A.IF_HAVE_INVOICE='Y' THEN '已开票'
ELSE '未开票'
END  AS 开票否,
G.PDate AS 采购日期
FROM PURCHASEGODE_DET A 
LEFT JOIN PURCHASE_DET C ON A.PUID=C.PUID AND A.SN=C.SN
LEFT JOIN SUPPLIERINFO_MST D ON C.SUID=D.SUID
LEFT JOIN GODE E ON A.PGKEY=E.GEKEY
LEFT JOIN WAREINFO B ON E.WAREID=B.WAREID
LEFT JOIN PurchaseGode_MST F ON A.PGID=F.PGID
LEFT JOIN Purchase_MST G ON C.PUID=G.PUID 
WHERE A.IF_HAVE_INVOICE='N' AND G.PurchaseStatus_MST ='CLOSE' ORDER BY C.SUID,A.PGKEY ASC
";
        #endregion
        #region setsqlfi
        string setsqlfi = @"
SELECT
B.RMKEY AS 请款索引,
A.RMID AS 请款单号,
A.INVOICE_NO AS 发票号码,
A.INVOICE_NOTAX_AMOUNT AS 发票未税金额,
A.INVOICE_TAX_AMOUNT AS 发票税额,
A.INVOICE_HAVETAX_AMOUNT AS 发票含税金额,
D.PUID AS 采购单号,
G.SN AS 项次,
H.WareID AS ID,
H.CO_WAREID AS 料号,
H.WName AS 品名,
H.SPEC AS 规格,
G.SKU AS 采购单位,
D.PCount AS 采购数量,
E.PDATE AS 采购日期,
D.PurchaseUnitPrice AS 采购单价,
D.TaxRate AS 税率,
B.PRKEY AS 索引,
C.REID AS 入库退货单号,
G.GECount AS 入库退货数量,
RTRIM(CONVERT(DECIMAL(18,2),G.GECount * D.PurchaseUnitPrice  )) AS 未税金额,
RTRIM(CONVERT(DECIMAL(18,2),G.GECount * D.PurchaseUnitPrice * D.TaxRate/100  )) AS 税额,
RTRIM(CONVERT(DECIMAL(18,2),G.GECount * D.PurchaseUnitPrice * (1+D.TaxRate/100) )) AS 含税金额,
F.SUID AS 供应商代码,
F.SNAME AS 供应商名称,
A.CUTPAYMENT_PROJECT AS 扣款项目,
A.CUTPAYMENT_AMOUNT AS 扣款金额,
A.REQUEST_MONEY_MAKERID 请款人工号,
(SELECT ENAME FROM EmployeeInfo WHERE EMID=A.REQUEST_MONEY_MAKERID ) AS 请款人,
A.REQUEST_MONEY_DATE AS 请款日期,
(SELECT ENAME FROM EmployeeInfo WHERE EMID=A.MakerID ) AS 制单人,
A.Date AS 制单日期
FROM REQUEST_MONEY_MST A 
LEFT JOIN REQUEST_MONEY_DET B ON A.RMID=B.RMID
LEFT JOIN RETURN_DET C ON B.PRKEY =C.REKEY 
LEFT JOIN Purchase_DET D ON D.PUID=C.PUID AND C.SN=D.SN 
LEFT JOIN Purchase_MST E ON D.PUID=E.PUID 
LEFT JOIN SupplierInfo_MST F ON E.SUID =F.SUID 
LEFT JOIN Gode G ON C.REKEY =G.GEKEY 
LEFT JOIN WareInfo H ON G.WareID =H.WareID 

";
        #endregion
        DataTable dtx2 = new DataTable();
        DataTable dt4 = new DataTable();
        CPURCHASE cpurchase = new CPURCHASE();
        public CREQUEST_MONEY()
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
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM REQUEST_MONEY_MST", "RMID", "RM");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        public DataTable DT_EMPTY()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("选择", typeof(bool));
            dtt.Columns.Add("索引", typeof(string));
            dtt.Columns.Add("请款单号", typeof(string));
            dtt.Columns.Add("采购单号", typeof(string));
            dtt.Columns.Add("入库单号", typeof(string));
            dtt.Columns.Add("目录项次", typeof(Int32));
            dtt.Columns.Add("项次", typeof(string));
            dtt.Columns.Add("ID", typeof(string));
            dtt.Columns.Add("料号", typeof(string));
            dtt.Columns.Add("品名", typeof(string));
            dtt.Columns.Add("规格", typeof(string));
            dtt.Columns.Add("采购单位", typeof(string));
            dtt.Columns.Add("采购单价", typeof(decimal));
            dtt.Columns.Add("采购数量", typeof(decimal));
            dtt.Columns.Add("税率", typeof(decimal));
            dtt.Columns.Add("未税金额", typeof(decimal));
            dtt.Columns.Add("税额", typeof(decimal));
            dtt.Columns.Add("含税金额", typeof(decimal));
            dtt.Columns.Add("供应商代码", typeof(string));
            dtt.Columns.Add("供应商名称", typeof(string));
            dtt.Columns.Add("采购日期", typeof(string));
            dtt.Columns.Add("发票号码", typeof(string));
            dtt.Columns.Add("发票未税金额", typeof(decimal));
            dtt.Columns.Add("发票税额", typeof(decimal));
            dtt.Columns.Add("发票含税金额", typeof(decimal));
            dtt.Columns.Add("请款索引", typeof(string));
            dtt.Columns.Add("入库(退货)单号", typeof(string));
            dtt.Columns.Add("入库(退货)数量", typeof(string));
            dtt.Columns.Add("累计入库数量", typeof(decimal));
            dtt.Columns.Add("请款人工号", typeof(string));
            dtt.Columns.Add("请款人", typeof(string));
            dtt.Columns.Add("请款日期", typeof(string));
            dtt.Columns.Add("制单日期", typeof(string));
            dtt.Columns.Add("制单人", typeof(string));
            dtt.Columns.Add("合计未税金额", typeof(decimal));
            dtt.Columns.Add("合计税额", typeof(decimal));
            dtt.Columns.Add("合计含税金额", typeof(decimal));
            dtt.Columns.Add("预付款单号", typeof(string));
            dtt.Columns.Add("预付款金额", typeof(string));
            dtt.Columns.Add("扣款项目", typeof(string));
            dtt.Columns.Add("扣款金额", typeof(string));
            dtt.Columns.Add("实际请款金额", typeof(decimal));
            dtt.Columns.Add("累计付款金额", typeof(string));
            dtt.Columns.Add("未付款金额", typeof(decimal));
            dtt.Columns.Add("付款单号", typeof(string));
            dtt.Columns.Add("付款金额", typeof(decimal));
            dtt.Columns.Add("付款人工号", typeof(string));
            dtt.Columns.Add("付款人", typeof(string));
            dtt.Columns.Add("付款日期", typeof(string));
            dtt.Columns.Add("付款制单人", typeof(string));
            dtt.Columns.Add("付款制单日期", typeof(string));
            dtt.Columns.Add("备注", typeof(string));
            return dtt;
        }
   
 
        #region dtx
        public DataTable dtx()
        {

            DataTable dtt = DT_EMPTY();
            DataTable dt4 = basec.getdts(cpurchase .sqlf +" WHERE C.IF_HAVE_INVOICE='N' AND F.PURCHASESTATUS_MST='CLOSE' "+cpurchase.sqlfi);
            int i = 0;
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt4.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    dr["选择"] = false;
                    dr["采购单号"] = dr1["采购单号"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["目录项次"] = i + 1;
                    dr["ID"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["采购单位"] = dr1["采购单位"].ToString();
                    dr["采购单价"] = dr1["采购单价"].ToString();
                    dr["采购数量"] = dr1["采购数量"].ToString();
                    dr["累计入库数量"] = dr1["累计入库数量"].ToString();
                    dr["税率"] = dr1["税率"].ToString();
                    dr["未税金额"] = dr1["未税金额"].ToString();
                    dr["税额"] = dr1["税额"].ToString();
                    dr["含税金额"] = dr1["含税金额"].ToString();
                    dr["供应商代码"] = dr1["供应商代码"].ToString();
                    dr["供应商名称"] = dr1["供应商名称"].ToString();
                    dr["采购日期"] = dr1["采购日期"].ToString();
                    dtt.Rows.Add(dr);
                    i = i + 1;
                }

            }

            return dtt;
        }
        #endregion
        #region RETURN_PG_AND RETURN_DT
        public DataTable RETURN_PG_AND_RETURN_DT()
        {

            DataTable dtt = DT_EMPTY();
            DataTable dt4 = basec.getdts(sqlth + " WHERE SUBSTRING (B.PRKEY,1,2)='PG' ");
            int i = 0;
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt4.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    dr["索引"] = dr1["索引"].ToString();
                    dr["采购单号"] = dr1["采购单号"].ToString();
                    dr["入库(退货)单号"] = dr1["入库退货单号"].ToString();
                    dr["入库(退货)数量"] = dr1["入库退货数量"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["目录项次"] = i + 1;
                    dr["ID"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["采购单位"] = dr1["采购单位"].ToString();
                    dr["采购单价"] = dr1["采购单价"].ToString();
                    dr["采购数量"] = dr1["采购数量"].ToString();
                    dr["税率"] = dr1["税率"].ToString();
                    dr["未税金额"] = dr1["未税金额"].ToString();
                    dr["税额"] = dr1["税额"].ToString();
                    dr["含税金额"] = dr1["含税金额"].ToString();
                    dr["扣款项目"] = dr1["扣款项目"].ToString();
                    dr["扣款金额"] = dr1["扣款金额"].ToString();
                    dr["供应商代码"] = dr1["供应商代码"].ToString();
                    dr["供应商名称"] = dr1["供应商名称"].ToString();
                    dr["采购日期"] = dr1["采购日期"].ToString();
                    dr["请款索引"] = dr1["请款索引"].ToString();
                    dr["请款人工号"] = dr1["请款人工号"].ToString();
                    dr["请款人"] = dr1["请款人"].ToString();
                    dr["制单人"] = dr1["制单人"].ToString();
                    dr["请款日期"] = dr1["请款日期"].ToString();
                    dr["发票号码"] = dr1["发票号码"].ToString();
                    dr["发票未税金额"] = dr1["发票未税金额"].ToString();
                    dr["发票税额"] = dr1["发票税额"].ToString();
                    dr["发票含税金额"] = dr1["发票含税金额"].ToString();
                    dr["请款单号"] = dr1["请款单号"].ToString();
                    dtt.Rows.Add(dr);
                    i = i + 1;
                }

            }
            dt4 = basec.getdts(sqlfi + " WHERE SUBSTRING (B.PRKEY,1,2)='RE' ");
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt4.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    decimal d1 = decimal.Parse(dr1["未税金额"].ToString());
                    decimal d2 = -d1;
                    decimal d3 = decimal.Parse(dr1["税额"].ToString());
                    decimal d4 = -d3;
                    decimal d5 = decimal.Parse(dr1["含税金额"].ToString());
                    decimal d6 = -d5;
                    dr["选择"] = false;
                    dr["索引"] = dr1["索引"].ToString();
                    dr["采购单号"] = dr1["采购单号"].ToString();
                    dr["入库(退货)单号"] = dr1["入库退货单号"].ToString();
                    dr["入库(退货)数量"] = dr1["入库退货数量"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["目录项次"] = i + 1;
                    dr["ID"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["采购单位"] = dr1["采购单位"].ToString();
                    dr["采购单价"] = dr1["采购单价"].ToString();
                    dr["采购数量"] = dr1["采购数量"].ToString();
                    dr["税率"] = dr1["税率"].ToString();
                    dr["未税金额"] = d2;
                    dr["税额"] = d4;
                    dr["含税金额"] = d6;
                    dr["供应商代码"] = dr1["供应商代码"].ToString();
                    dr["供应商名称"] = dr1["供应商名称"].ToString();
                    dr["扣款项目"] = dr1["扣款项目"].ToString();
                    dr["扣款金额"] = dr1["扣款金额"].ToString();
                    dr["采购日期"] = dr1["采购日期"].ToString();
                    dr["请款索引"] = dr1["请款索引"].ToString();
                    dr["请款人工号"] = dr1["请款人工号"].ToString();
                    dr["请款人"] = dr1["请款人"].ToString();
                    dr["制单人"] = dr1["制单人"].ToString();
                    dr["请款日期"] = dr1["请款日期"].ToString();
                    dr["发票号码"] = dr1["发票号码"].ToString();
                    dr["发票未税金额"] = dr1["发票未税金额"].ToString();
                    dr["发票税额"] = dr1["发票税额"].ToString();
                    dr["发票含税金额"] = dr1["发票含税金额"].ToString();
                    dr["请款单号"] = dr1["请款单号"].ToString();
                    dtt.Rows.Add(dr);
                    i = i + 1;
                }

            }
            foreach (DataRow dr in dtt.Rows)
            {
                dr["合计未税金额"] = dtt.Compute("SUM(未税金额)", "");
                dr["合计税额"] = dtt.Compute("SUM(税额)", "");
                dr["合计含税金额"] = dtt.Compute("SUM(含税金额)", "");

            }
            return dtt;
        }
        #endregion
        #region RETURN_PG_AND RETURN_DT
        public DataTable RETURN_PG_AND_RETURN_DT(string RMID)
        {

            DataTable dtt = DT_EMPTY();
            DataTable dt4 = basec.getdts(sqlth + " WHERE SUBSTRING (B.PRKEY,1,2)='PG' AND A.RMID='" + RMID + "'");
            int i = 0;
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt4.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    dr["索引"] = dr1["索引"].ToString();
                    dr["采购单号"] = dr1["采购单号"].ToString();
                    dr["入库(退货)单号"] = dr1["入库退货单号"].ToString();
                    dr["入库(退货)数量"] = dr1["入库退货数量"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["目录项次"] = i + 1;
                    dr["ID"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["采购单位"] = dr1["采购单位"].ToString();
                    dr["采购单价"] = dr1["采购单价"].ToString();
                    dr["采购数量"] = dr1["采购数量"].ToString();
                    dr["税率"] = dr1["税率"].ToString();
                    dr["未税金额"] = dr1["未税金额"].ToString();
                    dr["税额"] = dr1["税额"].ToString();
                    dr["含税金额"] = dr1["含税金额"].ToString();
                    dr["扣款项目"] = dr1["扣款项目"].ToString();
                    dr["扣款金额"] = dr1["扣款金额"].ToString();
                    dr["供应商代码"] = dr1["供应商代码"].ToString();
                    dr["供应商名称"] = dr1["供应商名称"].ToString();
                    dr["采购日期"] = dr1["采购日期"].ToString();
                    dr["请款索引"] = dr1["请款索引"].ToString();
                    dr["请款人工号"] = dr1["请款人工号"].ToString();
                    dr["请款人"] = dr1["请款人"].ToString();
                    dr["制单人"] = dr1["制单人"].ToString();
                    dr["请款日期"] = dr1["请款日期"].ToString();
                    dr["发票号码"] = dr1["发票号码"].ToString();
                    dr["发票未税金额"] = dr1["发票未税金额"].ToString();
                    dr["发票税额"] = dr1["发票税额"].ToString();
                    dr["发票含税金额"] = dr1["发票含税金额"].ToString();
                    dr["请款单号"] = dr1["请款单号"].ToString();
                    dtt.Rows.Add(dr);
                    i = i + 1;
                }

            }
            dt4 = basec.getdts(sqlfi + " WHERE SUBSTRING (B.PRKEY,1,2)='RE' AND A.RMID='" + RMID + "'");
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt4.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    decimal d1 = decimal.Parse(dr1["未税金额"].ToString());
                    decimal d2 = -d1;
                    decimal d3 = decimal.Parse(dr1["税额"].ToString());
                    decimal d4 = -d3;
                    decimal d5 = decimal.Parse(dr1["含税金额"].ToString());
                    decimal d6 = -d5;
                    dr["选择"] = false;
                    dr["索引"] = dr1["索引"].ToString();
                    dr["采购单号"] = dr1["采购单号"].ToString();
                    dr["入库(退货)单号"] = dr1["入库退货单号"].ToString();
                    dr["入库(退货)数量"] = dr1["入库退货数量"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["目录项次"] = i + 1;
                    dr["ID"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["采购单位"] = dr1["采购单位"].ToString();
                    dr["采购单价"] = dr1["采购单价"].ToString();
                    dr["采购数量"] = dr1["采购数量"].ToString();
                    dr["税率"] = dr1["税率"].ToString();
                    dr["未税金额"] = d2;
                    dr["税额"] = d4;
                    dr["含税金额"] = d6;
                    dr["供应商代码"] = dr1["供应商代码"].ToString();
                    dr["供应商名称"] = dr1["供应商名称"].ToString();
                    dr["扣款项目"] = dr1["扣款项目"].ToString();
                    dr["扣款金额"] = dr1["扣款金额"].ToString();
                    dr["采购日期"] = dr1["采购日期"].ToString();
                    dr["请款索引"] = dr1["请款索引"].ToString();
                    dr["请款人工号"] = dr1["请款人工号"].ToString();
                    dr["请款人"] = dr1["请款人"].ToString();
                    dr["制单人"] = dr1["制单人"].ToString();
                    dr["请款日期"] = dr1["请款日期"].ToString();
                    dr["发票号码"] = dr1["发票号码"].ToString();
                    dr["发票未税金额"] = dr1["发票未税金额"].ToString();
                    dr["发票税额"] = dr1["发票税额"].ToString();
                    dr["发票含税金额"] = dr1["发票含税金额"].ToString();
                    dr["请款单号"] = dr1["请款单号"].ToString();
                    dtt.Rows.Add(dr);
                    i = i + 1;
                }

            }
            foreach (DataRow dr in dtt.Rows)
            {
                dr["合计未税金额"] = dtt.Compute("SUM(未税金额)", "");
                dr["合计税额"] = dtt.Compute("SUM(税额)", "");
                dr["合计含税金额"] = dtt.Compute("SUM(含税金额)", "");
            
            }
            return dtt;
        }
        #endregion

        #region TOTAL_RETURN_PG_AND RETURN_DT
        public DataTable TOTAL_RETURN_PG_AND_RETURN_DT(string RMID)
        {
            DataTable dtt = DT_EMPTY();
            decimal d1 = 0, d2 = 0, d3 = 0;
            DataTable dt = RETURN_PG_AND_RETURN_DT(RMID);
            DataRow dr = dtt.NewRow();
            dr["合计未税金额"] = dt.Compute("SUM(未税金额)", "");
            dr["合计税额"] = dt.Compute("SUM(税额)", "");
            dr["合计含税金额"] = dt.Compute("SUM(含税金额)", "");
            dr["供应商名称"] = dt.Rows[0]["供应商名称"].ToString();
            dr["请款人"] = dt.Rows[0]["请款人"].ToString();
            dr["请款日期"] = dt.Rows[0]["请款日期"].ToString();
            dr["制单人"] = dt.Rows[0]["制单人"].ToString();
            dr["扣款项目"] = dt.Rows[0]["扣款项目"].ToString();
            dr["扣款金额"] = dt.Rows[0]["扣款金额"].ToString();
            if (!string.IsNullOrEmpty(dt.Rows[0]["扣款金额"].ToString()))
            {
                d3 = decimal.Parse(dt.Rows[0]["扣款金额"].ToString());
            }

            dtt.Rows.Add(dr);
         
            d1 = decimal.Parse(dtt.Rows[0]["合计含税金额"].ToString());
            string sqlx = @"
SELECT 
A.APID AS APID,
C.ADVANCE_PAYMENT_AMOUNT AS ADVANCE_PAYMENT_AMOUNT 
FROM REQUEST_MONEY_MST A 
LEFT JOIN ADVANCE_PAYMENT B ON A.APID=B.APID 
LEFT JOIN GODE C ON B.APKEY=C.GEKEY WHERE RMID='" + RMID + "'";

                dt = bc.getdt(sqlx);
                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0]["ADVANCE_PAYMENT_AMOUNT"].ToString()))
                    {
                        d2 = decimal.Parse(dt.Rows[0]["ADVANCE_PAYMENT_AMOUNT"].ToString());
                    }
                   

                }
          
                dtt.Rows[0]["预付款单号"] = dt.Rows[0]["APID"].ToString();
                dtt.Rows[0]["预付款金额"] = dt.Rows[0]["ADVANCE_PAYMENT_AMOUNT"].ToString();
                dtt.Rows[0]["实际请款金额"] = d1 - d2 - d3;
 
            return dtt;
        }
        #endregion
        #region RETURN_DT
        public DataTable RETURN_DT()
        {
            DataTable dtt = DT_EMPTY();
            string sqlx = "SELECT * FROM REQUEST_MONEY_MST ";
            DataTable  dt = bc.getdt(sqlx);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt.Rows)
                {

                    DataRow dr = dtt.NewRow();
                    dr["请款单号"] = dr1["RMID"].ToString();
                    dr["发票号码"] = dr1["INVOICE_NO"].ToString();
                    dr["发票未税金额"] = dr1["INVOICE_NOTAX_AMOUNT"].ToString();
                    dr["发票税额"] = dr1["INVOICE_TAX_AMOUNT"].ToString();
                    dr["发票含税金额"] = dr1["INVOICE_HAVETAX_AMOUNT"].ToString();
                    dr["制单日期"] = dr1["DATE"].ToString();
                    dr["扣款项目"] = dr1["CUTPAYMENT_PROJECT"].ToString();
                    dr["扣款金额"] = dr1["CUTPAYMENT_AMOUNT"].ToString();
                    DataTable  dtx = TOTAL_RETURN_PG_AND_RETURN_DT(dr1["RMID"].ToString());
                    dr["供应商名称"] = dtx.Rows[0]["供应商名称"].ToString();
                    dr["合计未税金额"] = dtx.Rows[0]["合计未税金额"].ToString();
                    dr["合计税额"] = dtx.Rows[0]["合计税额"].ToString();
                    dr["预付款单号"] = dtx.Rows[0]["预付款单号"].ToString();
                    dr["预付款金额"] = dtx.Rows[0]["预付款金额"].ToString();
                    dr["合计含税金额"] = dtx.Rows[0]["合计含税金额"].ToString();
                    dr["实际请款金额"] = dtx.Rows[0]["实际请款金额"].ToString();
                    dr["请款人"] = dtx.Rows[0]["请款人"].ToString();
                    dr["请款日期"] = dtx.Rows[0]["请款日期"].ToString();
                    dr["制单人"] = dtx.Rows[0]["制单人"].ToString();
                    DataTable dtx1 = bc.getdt(@"
SELECT 
RMID AS RMID,
SUM(PAYMENT_ORDER_AMOUNT) AS PAYMNET_ORDER_AMOUNT 
FROM PAYMENT_ORDER 
WHERE
RMID='"+dr1["RMID"].ToString()+"' GROUP BY RMID");
                    if (dtx1.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dtx1.Rows[0]["PAYMNET_ORDER_AMOUNT"].ToString()))
                        {
                            dr["累计付款金额"] = dtx1.Rows[0]["PAYMNET_ORDER_AMOUNT"].ToString();
                            dr["未付款金额"] = decimal.Parse(dtx.Rows[0]["实际请款金额"].ToString()) - decimal.Parse(dtx1.Rows[0]["PAYMNET_ORDER_AMOUNT"].ToString());
                        }

                    }
                    else
                    {
                        dr["累计付款金额"] = "0.00";
                        dr["未付款金额"] = decimal.Parse(dtx.Rows[0]["实际请款金额"].ToString());
                    }
                    dtt.Rows.Add(dr);
                }

            }

            return dtt;
        }
        #endregion
        #region JUAGE_IF_EXISTS_PG_RETURN()
        public bool  JUAGE_IF_EXISTS_PG_RETURN(string PGID_OR_REID,string PGKEY_OR_REKEY)
        {

            bool b = false;
            DataTable dt = RETURN_PG_AND_RETURN_DT();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["入库(退货)单号"].ToString() == PGID_OR_REID)
                    {
                        b = true;
                        break;
                    }
                    if (dr["索引"].ToString() == PGKEY_OR_REKEY)
                    {
                        b = true;
                        break;
                    }
                }

            }
           
            return b;
        }
        #endregion
    }
}
