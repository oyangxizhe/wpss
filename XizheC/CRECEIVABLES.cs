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
    public class CRECEIVABLES
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
    
        #endregion
        #region setsql
        string setsql = @"

SELECT 
A.RCID AS 应收单号,
A.INVOICE_NO AS 发票号码,
A.INVOICE_NOTAX_AMOUNT AS 发票未税,
A.INVOICE_TAX_AMOUNT AS 发票税额,
A.INVOICE_HAVETAX_AMOUNT AS 发票含税,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.RECEIVABLES_MAKERID) AS 应收人,
A.RECEIVABLES_DATE AS 应收日期,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.RECEIVABLES_MAKERID) AS 制单人,
A.DATE AS 制单日期
FROM RECEIVABLES_MST  A

";
        #endregion
        #region setsqlo
        string setsqlo = @"
INSERT INTO RECEIVABLES_DET
(
RCKEY,
RCID,
SSKEY,
YEAR,
MONTH,
DAY
)
VALUES
(
@RCKEY,
@RCID,
@SSKEY,
@YEAR,
@MONTH,
@DAY

)
";
        #endregion
        #region setsqlt
        string setsqlt = @"
INSERT INTO RECEIVABLES_MST
(
RCID,
INVOICE_NO,
INVOICE_NOTAX_AMOUNT,
INVOICE_TAX_AMOUNT,
INVOICE_HAVETAX_AMOUNT,
RECEIVABLES_DATE,
RECEIVABLES_MAKERID,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@RCID,
@INVOICE_NO,
@INVOICE_NOTAX_AMOUNT,
@INVOICE_TAX_AMOUNT,
@INVOICE_HAVETAX_AMOUNT,
@RECEIVABLES_DATE,
@RECEIVABLES_MAKERID,
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
B.RCKEY AS 应收索引,
A.RCID AS 应收单号,
A.INVOICE_NO AS 发票号码,
A.INVOICE_NOTAX_AMOUNT AS 发票未税金额,
A.INVOICE_TAX_AMOUNT AS 发票税额,
A.INVOICE_HAVETAX_AMOUNT AS 发票含税金额,
D.ORID AS 订单号,
G.SN AS 项次,
H.WareID AS ID,
H.CO_WAREID AS 料号,
H.WName AS 品名,
H.SPEC AS 规格,
G.SKU AS 销售单位,
D.OCOUNT AS 订单数量,
E.ORDERDATE AS 订单日期,
D.SELLUNITPRICE AS 销售单价,
D.TaxRate AS 税率,
B.SSKEY AS 索引,
C.SEID AS 销货销退单号,
G.MRCount AS 销货销退数量,
RTRIM(CONVERT(DECIMAL(18,2),G.MRCount * D.SELLUNITPRICE  )) AS 未税金额,
RTRIM(CONVERT(DECIMAL(18,2),G.MRCount * D.SELLUNITPRICE * D.TaxRate/100  )) AS 税额,
RTRIM(CONVERT(DECIMAL(18,2),G.MRCount * D.SELLUNITPRICE * (1+D.TaxRate/100) )) AS 含税金额,
F.CUID AS 客户代码,
F.CNAME AS 客户名称,
A.CUTPAYMENT_PROJECT AS 扣款项目,
A.CUTPAYMENT_AMOUNT AS 扣款金额,
A.RECEIVABLES_MAKERID 应收人工号,
(SELECT ENAME FROM EmployeeInfo WHERE EMID=A.RECEIVABLES_MAKERID ) AS 应收人,
A.RECEIVABLES_DATE AS 应收日期,
(SELECT ENAME FROM EmployeeInfo WHERE EMID=A.MakerID ) AS 制单人,
A.Date AS 制单日期
FROM RECEIVABLES_MST A 
LEFT JOIN RECEIVABLES_DET B ON A.RCID=B.RCID
LEFT JOIN SELLTABLE_DET C ON B.SSKEY =C.SEKEY 
LEFT JOIN ORDER_DET D ON D.ORID=C.ORID AND C.SN=D.SN 
LEFT JOIN ORDER_MST E ON D.ORID=E.ORID 
LEFT JOIN CUSTOMERINFO_MST F ON E.CUID =F.CUID 
LEFT JOIN MATERE G ON C.SEKEY =G.MRKEY 
LEFT JOIN WareInfo H ON G.WareID =H.WareID 

";
        #endregion
        #region setsqlf
        string setsqlf = @"

SELECT 
A.SEKEY AS 索引,
A.SEID AS 销货单号,
A.ORID AS 订单号, 
A.SN AS 项次,
E.WAREID AS ID,
B.CO_WAREID AS 料号,
B.WNAME AS 品名,
B.Spec AS 规格,
E.SKU AS 销售单位,
C.SELLUNITPRICE AS 销售单价,
C.OCOUNT AS 订单数量,
C.TaxRate as  税率,
E.MRCOUNT AS 销货数量 ,
RTRIM(CONVERT(DECIMAL(18,2),E.MRCount  * C.SELLUNITPRICE  )) AS 未税金额,
RTRIM(CONVERT(DECIMAL(18,2),E.MRCount  * C.SELLUNITPRICE * C.TaxRate/100  )) AS 税额,
RTRIM(CONVERT(DECIMAL(18,2),E.MRCount  * C.SELLUNITPRICE * (1+C.TaxRate/100) )) AS 含税金额,
C.CUID AS 客户代码,
D.CNAME AS 客户名称,
CASE WHEN A.IF_HAVE_INVOICE='Y' THEN '已开票'
ELSE '未开票'
END  AS 开票否,
G.ORDERDATE AS 订单日期
FROM SELLTABLE_DET A 
LEFT JOIN ORDER_DET C ON A.ORID=C.ORID AND A.SN=C.SN
LEFT JOIN CUSTOMERINFO_MST D ON C.CUID=D.CUID
LEFT JOIN MATERE E ON A.SEKEY=E.MRKEY
LEFT JOIN WAREINFO B ON E.WAREID=B.WAREID
LEFT JOIN SELLTABLE_MST F ON A.SEID=F.SEID
LEFT JOIN ORDER_MST G ON C.ORID=G.ORID WHERE A.IF_HAVE_INVOICE='N' ORDER BY C.CUID,A.SEKEY ASC
";
        #endregion
        #region setsqlfi
        string setsqlfi = @"
SELECT
B.RCKEY AS 应收索引,
A.RCID AS 应收单号,
A.INVOICE_NO AS 发票号码,
A.INVOICE_NOTAX_AMOUNT AS 发票未税金额,
A.INVOICE_TAX_AMOUNT AS 发票税额,
A.INVOICE_HAVETAX_AMOUNT AS 发票含税金额,
D.ORID AS 订单号,
G.SN AS 项次,
H.WareID AS ID,
H.CO_WAREID AS 料号,
H.WName AS 品名,
H.SPEC AS 规格,
G.SKU AS 销售单位,
D.OCount AS 订单数量,
E.ORDERDATE AS 订单日期,
D.SELLUNITPRICE AS 销售单价,
D.TaxRate AS 税率,
B.SSKEY AS 索引,
C.SRID AS 销货销退单号,
G.GECount AS 销货销退数量,
RTRIM(CONVERT(DECIMAL(18,2),G.GECount * D.SELLUNITPRICE  )) AS 未税金额,
RTRIM(CONVERT(DECIMAL(18,2),G.GECount * D.SELLUNITPRICE * D.TaxRate/100  )) AS 税额,
RTRIM(CONVERT(DECIMAL(18,2),G.GECount * D.SELLUNITPRICE * (1+D.TaxRate/100) )) AS 含税金额,
F.CUID AS 客户代码,
F.CNAME AS 客户名称,
A.CUTPAYMENT_PROJECT AS 扣款项目,
A.CUTPAYMENT_AMOUNT AS 扣款金额,
A.RECEIVABLES_MAKERID 应收人工号,
(SELECT ENAME FROM EmployeeInfo WHERE EMID=A.RECEIVABLES_MAKERID ) AS 应收人,
A.RECEIVABLES_DATE AS 应收日期,
(SELECT ENAME FROM EmployeeInfo WHERE EMID=A.MakerID ) AS 制单人,
A.Date AS 制单日期
FROM RECEIVABLES_MST A 
LEFT JOIN RECEIVABLES_DET B ON A.RCID=B.RCID
LEFT JOIN SELLRETURN_DET C ON B.SSKEY =C.SRKEY 
LEFT JOIN ORDER_DET D ON D.ORID=C.ORID AND C.SN=D.SN 
LEFT JOIN ORDER_MST E ON D.ORID=E.ORID 
LEFT JOIN CUSTOMERINFO_MST F ON E.CUID =F.CUID 
LEFT JOIN Gode G ON C.SRKEY =G.GEKEY 
LEFT JOIN WareInfo H ON G.WareID =H.WareID 

";
        #endregion
        DataTable dtx2 = new DataTable();
        DataTable dt4 = new DataTable();
        CORDER corder = new CORDER();
        public CRECEIVABLES()
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
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM RECEIVABLES_MST", "RCID", "RC");
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
            dtt.Columns.Add("应收单号", typeof(string));
            dtt.Columns.Add("订单号", typeof(string));
            dtt.Columns.Add("销货单号", typeof(string));
            dtt.Columns.Add("目录项次", typeof(Int32));
            dtt.Columns.Add("项次", typeof(string));
            dtt.Columns.Add("ID", typeof(string));
            dtt.Columns.Add("料号", typeof(string));
            dtt.Columns.Add("品名", typeof(string));
            dtt.Columns.Add("规格", typeof(string));
            dtt.Columns.Add("销售单位", typeof(string));
            dtt.Columns.Add("销售单价", typeof(decimal));
            dtt.Columns.Add("订单数量", typeof(decimal));
            dtt.Columns.Add("税率", typeof(decimal));
            dtt.Columns.Add("未税金额", typeof(decimal));
            dtt.Columns.Add("税额", typeof(decimal));
            dtt.Columns.Add("含税金额", typeof(decimal));
            dtt.Columns.Add("客户代码", typeof(string));
            dtt.Columns.Add("客户名称", typeof(string));
            dtt.Columns.Add("订单日期", typeof(string));
            dtt.Columns.Add("发票号码", typeof(string));
            dtt.Columns.Add("发票未税金额", typeof(decimal));
            dtt.Columns.Add("发票税额", typeof(decimal));
            dtt.Columns.Add("发票含税金额", typeof(decimal));
            dtt.Columns.Add("应收索引", typeof(string));
            dtt.Columns.Add("销货(销退)单号", typeof(string));
            dtt.Columns.Add("销货(销退)数量", typeof(string));
            dtt.Columns.Add("累计销货数量", typeof(decimal));
            dtt.Columns.Add("应收人工号", typeof(string));
            dtt.Columns.Add("应收人", typeof(string));
            dtt.Columns.Add("应收日期", typeof(string));
            dtt.Columns.Add("制单日期", typeof(string));
            dtt.Columns.Add("制单人", typeof(string));
            dtt.Columns.Add("合计未税金额", typeof(decimal));
            dtt.Columns.Add("合计税额", typeof(decimal));
            dtt.Columns.Add("合计含税金额", typeof(decimal));
            dtt.Columns.Add("预收款单号", typeof(string));
            dtt.Columns.Add("预收款金额", typeof(string));
            dtt.Columns.Add("扣款项目", typeof(string));
            dtt.Columns.Add("扣款金额", typeof(string));
            dtt.Columns.Add("实际应收金额", typeof(decimal));
            dtt.Columns.Add("累计收款金额", typeof(string));
            dtt.Columns.Add("未收款金额", typeof(decimal));
            dtt.Columns.Add("收款单号", typeof(string));
            dtt.Columns.Add("收款金额", typeof(decimal));
            dtt.Columns.Add("收款人工号", typeof(string));
            dtt.Columns.Add("收款人", typeof(string));
            dtt.Columns.Add("收款日期", typeof(string));
            dtt.Columns.Add("收款制单人", typeof(string));
            dtt.Columns.Add("收款制单日期", typeof(string));
            dtt.Columns.Add("备注", typeof(string));
     
            return dtt;
        }
   
        #region dtx
        public  DataTable dtx()
        {

            DataTable dtt = DT_EMPTY();
            DataTable dt4 = basec.getdts(corder.sqlo+" WHERE C.IF_HAVE_INVOICE='N' AND F.ORDERSTATUS_MST='CLOSE' "+corder.sqlt);
            int i = 0;
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt4.Rows)
                {   
                    DataRow dr = dtt.NewRow();
                    dr["选择"] = false;
                   
                    dr["订单号"] = dr1["订单号"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["目录项次"] = i + 1;
                    dr["ID"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["销售单位"] = dr1["销售单位"].ToString();
                    dr["销售单价"] = dr1["销售单价"].ToString();
                    dr["订单数量"] = dr1["订单数量"].ToString();
                    dr["累计销货数量"] = dr1["累计销货数量"].ToString();
                    dr["税率"] = dr1["税率"].ToString();
                    dr["未税金额"] = dr1["未税金额"].ToString();
                    dr["税额"] = dr1["税额"].ToString();
                    dr["含税金额"] = dr1["含税金额"].ToString();
                    dr["客户代码"] = dr1["客户代码"].ToString();
                    dr["客户名称"] = dr1["客户名称"].ToString();
                    dr["订单日期"] = dr1["订单日期"].ToString();
                    dtt.Rows.Add(dr);
                    i = i + 1;
                }
         
            }
       
            return dtt;
        }
        #endregion
        #region RETURN_PG_AND_RETURN_DT
        public DataTable RETURN_PG_AND_RETURN_DT()
        {

            DataTable dtt = DT_EMPTY();
            DataTable dt4 = basec.getdts(sqlth + " WHERE SUBSTRING (B.SSKEY,1,2)='SE' ");
            int i = 0;
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt4.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    dr["索引"] = dr1["索引"].ToString();
                    dr["订单号"] = dr1["订单号"].ToString();
                    dr["销货(销退)单号"] = dr1["销货销退单号"].ToString();
                    dr["销货(销退)数量"] = dr1["销货销退数量"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["目录项次"] = i + 1;
                    dr["ID"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["销售单位"] = dr1["销售单位"].ToString();
                    dr["销售单价"] = dr1["销售单价"].ToString();
                    dr["订单数量"] = dr1["订单数量"].ToString();
                    dr["税率"] = dr1["税率"].ToString();
                    dr["未税金额"] = dr1["未税金额"].ToString();
                    dr["税额"] = dr1["税额"].ToString();
                    dr["含税金额"] = dr1["含税金额"].ToString();
                    dr["扣款项目"] = dr1["扣款项目"].ToString();
                    dr["扣款金额"] = dr1["扣款金额"].ToString();
                    dr["客户代码"] = dr1["客户代码"].ToString();
                    dr["客户名称"] = dr1["客户名称"].ToString();
                    dr["订单日期"] = dr1["订单日期"].ToString();
                    dr["应收索引"] = dr1["应收索引"].ToString();
                    dr["应收人工号"] = dr1["应收人工号"].ToString();
                    dr["应收人"] = dr1["应收人"].ToString();
                    dr["制单人"] = dr1["制单人"].ToString();
                    dr["应收日期"] = dr1["应收日期"].ToString();
                    dr["发票号码"] = dr1["发票号码"].ToString();
                    dr["发票未税金额"] = dr1["发票未税金额"].ToString();
                    dr["发票税额"] = dr1["发票税额"].ToString();
                    dr["发票含税金额"] = dr1["发票含税金额"].ToString();
                    dr["应收单号"] = dr1["应收单号"].ToString();
                    dtt.Rows.Add(dr);
                    i = i + 1;
                }

            }
            dt4 = basec.getdts(sqlfi + " WHERE SUBSTRING (B.SSKEY,1,2)='SR' ");
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
                    dr["订单号"] = dr1["订单号"].ToString();
                    dr["销货(销退)单号"] = dr1["销货销退单号"].ToString();
                    dr["销货(销退)数量"] = dr1["销货销退数量"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["目录项次"] = i + 1;
                    dr["ID"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["销售单位"] = dr1["销售单位"].ToString();
                    dr["销售单价"] = dr1["销售单价"].ToString();
                    dr["订单数量"] = dr1["订单数量"].ToString();
                    dr["税率"] = dr1["税率"].ToString();
                    dr["未税金额"] = d2;
                    dr["税额"] = d4;
                    dr["含税金额"] = d6;
                    dr["客户代码"] = dr1["客户代码"].ToString();
                    dr["客户名称"] = dr1["客户名称"].ToString();
                    dr["扣款项目"] = dr1["扣款项目"].ToString();
                    dr["扣款金额"] = dr1["扣款金额"].ToString();
                    dr["订单日期"] = dr1["订单日期"].ToString();
                    dr["应收索引"] = dr1["应收索引"].ToString();
                    dr["应收人工号"] = dr1["应收人工号"].ToString();
                    dr["应收人"] = dr1["应收人"].ToString();
                    dr["制单人"] = dr1["制单人"].ToString();
                    dr["应收日期"] = dr1["应收日期"].ToString();
                    dr["发票号码"] = dr1["发票号码"].ToString();
                    dr["发票未税金额"] = dr1["发票未税金额"].ToString();
                    dr["发票税额"] = dr1["发票税额"].ToString();
                    dr["发票含税金额"] = dr1["发票含税金额"].ToString();
                    dr["应收单号"] = dr1["应收单号"].ToString();
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
        #region RETURN_PG_AND_RETURN_DT
        public DataTable RETURN_PG_AND_RETURN_DT(string RCID)
        {

            DataTable dtt = DT_EMPTY();
            DataTable dt4 = basec.getdts(sqlth + " WHERE SUBSTRING (B.SSKEY,1,2)='SE' AND A.RCID='" + RCID + "'");
            int i = 0;
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt4.Rows)
                {
                    DataRow dr = dtt.NewRow();
                    dr["索引"] = dr1["索引"].ToString();
                    dr["订单号"] = dr1["订单号"].ToString();
                    dr["销货(销退)单号"] = dr1["销货销退单号"].ToString();
                    dr["销货(销退)数量"] = dr1["销货销退数量"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["目录项次"] = i + 1;
                    dr["ID"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["销售单位"] = dr1["销售单位"].ToString();
                    dr["销售单价"] = dr1["销售单价"].ToString();
                    dr["订单数量"] = dr1["订单数量"].ToString();
                    dr["税率"] = dr1["税率"].ToString();
                    dr["未税金额"] = dr1["未税金额"].ToString();
                    dr["税额"] = dr1["税额"].ToString();
                    dr["含税金额"] = dr1["含税金额"].ToString();
                    dr["扣款项目"] = dr1["扣款项目"].ToString();
                    dr["扣款金额"] = dr1["扣款金额"].ToString();
                    dr["客户代码"] = dr1["客户代码"].ToString();
                    dr["客户名称"] = dr1["客户名称"].ToString();
                    dr["订单日期"] = dr1["订单日期"].ToString();
                    dr["应收索引"] = dr1["应收索引"].ToString();
                    dr["应收人工号"] = dr1["应收人工号"].ToString();
                    dr["应收人"] = dr1["应收人"].ToString();
                    dr["制单人"] = dr1["制单人"].ToString();
                    dr["应收日期"] = dr1["应收日期"].ToString();
                    dr["发票号码"] = dr1["发票号码"].ToString();
                    dr["发票未税金额"] = dr1["发票未税金额"].ToString();
                    dr["发票税额"] = dr1["发票税额"].ToString();
                    dr["发票含税金额"] = dr1["发票含税金额"].ToString();
                    dr["应收单号"] = dr1["应收单号"].ToString();
                    dtt.Rows.Add(dr);
                    i = i + 1;
                }

            }
            dt4 = basec.getdts(sqlfi + " WHERE SUBSTRING (B.SSKEY,1,2)='SR' AND A.RCID='" + RCID + "'");
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
                    dr["订单号"] = dr1["订单号"].ToString();
                    dr["销货(销退)单号"] = dr1["销货销退单号"].ToString();
                    dr["销货(销退)数量"] = dr1["销货销退数量"].ToString();
                    dr["项次"] = dr1["项次"].ToString();
                    dr["目录项次"] = i + 1;
                    dr["ID"] = dr1["ID"].ToString();
                    dr["料号"] = dr1["料号"].ToString();
                    dr["品名"] = dr1["品名"].ToString();
                    dr["规格"] = dr1["规格"].ToString();
                    dr["销售单位"] = dr1["销售单位"].ToString();
                    dr["销售单价"] = dr1["销售单价"].ToString();
                    dr["订单数量"] = dr1["订单数量"].ToString();
                    dr["税率"] = dr1["税率"].ToString();
                    dr["未税金额"] = d2;
                    dr["税额"] = d4;
                    dr["含税金额"] = d6;
                    dr["客户代码"] = dr1["客户代码"].ToString();
                    dr["客户名称"] = dr1["客户名称"].ToString();
                    dr["扣款项目"] = dr1["扣款项目"].ToString();
                    dr["扣款金额"] = dr1["扣款金额"].ToString();
                    dr["订单日期"] = dr1["订单日期"].ToString();
                    dr["应收索引"] = dr1["应收索引"].ToString();
                    dr["应收人工号"] = dr1["应收人工号"].ToString();
                    dr["应收人"] = dr1["应收人"].ToString();
                    dr["制单人"] = dr1["制单人"].ToString();
                    dr["应收日期"] = dr1["应收日期"].ToString();
                    dr["发票号码"] = dr1["发票号码"].ToString();
                    dr["发票未税金额"] = dr1["发票未税金额"].ToString();
                    dr["发票税额"] = dr1["发票税额"].ToString();
                    dr["发票含税金额"] = dr1["发票含税金额"].ToString();
                    dr["应收单号"] = dr1["应收单号"].ToString();
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

        #region TOTAL_RETURN_PG_AND return_DT
        public DataTable TOTAL_RETURN_PG_AND_RETURN_DT(string RCID)
        {
            DataTable dtt = DT_EMPTY();
            decimal d1 = 0, d2 = 0, d3 = 0;
            DataTable dt = RETURN_PG_AND_RETURN_DT(RCID);
            DataRow dr = dtt.NewRow();
            dr["合计未税金额"] = dt.Compute("SUM(未税金额)", "");
            dr["合计税额"] = dt.Compute("SUM(税额)", "");
            dr["合计含税金额"] = dt.Compute("SUM(含税金额)", "");
            dr["客户名称"] = dt.Rows[0]["客户名称"].ToString();
            dr["应收人"] = dt.Rows[0]["应收人"].ToString();
            dr["应收日期"] = dt.Rows[0]["应收日期"].ToString();
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
A.ARID AS ARID,
C.ADVANCE_RECEIVABLES AS ADVANCE_RECEIVABLES
FROM RECEIVABLES_MST A 
LEFT JOIN ADVANCE_RECEIVABLES  B ON A.ARID=B.ARID 
LEFT JOIN GODE C ON B.ARKEY=C.GEKEY 
WHERE RCID='" + RCID + "'";

                dt = bc.getdt(sqlx);
                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0]["ADVANCE_RECEIVABLES"].ToString()))
                    {
                        d2 = decimal.Parse(dt.Rows[0]["ADVANCE_RECEIVABLES"].ToString());
                    }
                   

                }
          
                dtt.Rows[0]["预收款单号"] = dt.Rows[0]["ARID"].ToString();
                dtt.Rows[0]["预收款金额"] = dt.Rows[0]["ADVANCE_RECEIVABLES"].ToString();
                dtt.Rows[0]["实际应收金额"] = d1 - d2 - d3;
 
            return dtt;
        }
        #endregion
        #region RETURN_DT
        public DataTable RETURN_DT()
        {
            DataTable dtt = DT_EMPTY();
            string sqlx = "SELECT * FROM RECEIVABLES_MST ";
            DataTable  dt = bc.getdt(sqlx);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt.Rows)
                {

                    DataRow dr = dtt.NewRow();
                    dr["应收单号"] = dr1["RCID"].ToString();
                    dr["发票号码"] = dr1["INVOICE_NO"].ToString();
                    dr["发票未税金额"] = dr1["INVOICE_NOTAX_AMOUNT"].ToString();
                    dr["发票税额"] = dr1["INVOICE_TAX_AMOUNT"].ToString();
                    dr["发票含税金额"] = dr1["INVOICE_HAVETAX_AMOUNT"].ToString();
                    dr["制单日期"] = dr1["DATE"].ToString();
                    dr["扣款项目"] = dr1["CUTPAYMENT_PROJECT"].ToString();
                    dr["扣款金额"] = dr1["CUTPAYMENT_AMOUNT"].ToString();
                    DataTable dtx = TOTAL_RETURN_PG_AND_RETURN_DT(dr1["RCID"].ToString());
                    dr["客户名称"] = dtx.Rows[0]["客户名称"].ToString();
                    dr["合计未税金额"] = dtx.Rows[0]["合计未税金额"].ToString();
                    dr["合计税额"] = dtx.Rows[0]["合计税额"].ToString();
                    dr["预收款单号"] = dtx.Rows[0]["预收款单号"].ToString();
                    dr["预收款金额"] = dtx.Rows[0]["预收款金额"].ToString();
                    dr["合计含税金额"] = dtx.Rows[0]["合计含税金额"].ToString();
                    dr["实际应收金额"] = dtx.Rows[0]["实际应收金额"].ToString();
                    dr["应收人"] = dtx.Rows[0]["应收人"].ToString();
                    dr["应收日期"] = dtx.Rows[0]["应收日期"].ToString();
                    dr["制单人"] = dtx.Rows[0]["制单人"].ToString();
                    DataTable dtx1 = bc.getdt(@"
SELECT 
RCID AS RCID,
SUM(RECEIVABLES_ORDER_AMOUNT) AS RECEIVABLES_ORDER_AMOUNT 
FROM RECEIVABLES_ORDER 
WHERE
RCID='" + dr1["RCID"].ToString()+"' GROUP BY RCID");
                    if (dtx1.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dtx1.Rows[0]["RECEIVABLES_ORDER_AMOUNT"].ToString()))
                        {
                            dr["累计收款金额"] = dtx1.Rows[0]["RECEIVABLES_ORDER_AMOUNT"].ToString();
                            dr["未收款金额"] = decimal.Parse(dtx.Rows[0]["实际应收金额"].ToString()) - decimal.Parse(dtx1.Rows[0]["RECEIVABLES_ORDER_AMOUNT"].ToString());
                        }

                    }
                    else
                    {
                        dr["累计收款金额"] = "0.00";
                        dr["未收款金额"] = decimal.Parse(dtx.Rows[0]["实际应收金额"].ToString());
                    }
                    dtt.Rows.Add(dr);
                }

            }

            return dtt;
        }
        #endregion
        #region JUAGE_IF_EXISTS_SE_SERETURN()
        public bool  JUAGE_IF_EXISTS_SE_SERETURN(string SEID_OR_REID,string SEKEY_OR_SRKEY)
        {
         
            bool b = false;
            DataTable dt = RETURN_PG_AND_RETURN_DT();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["销货(销退)单号"].ToString() == SEID_OR_REID)
                    {
                        b = true;
                        break;
                    }
                    if (dr["索引"].ToString() == SEKEY_OR_SRKEY)
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
