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
    public class CTRANSFERS
    {
        basec bc = new basec();

        #region nature
        private string _GETID;
        public string GETID
        {
            set { _GETID = value; }
            get { return _GETID; }

        }
        private string _WAREID;
        public string WAREID
        {
            set { _WAREID = value; }
            get { return _WAREID; }

        }
        private string _SKU;
        public string SKU
        {
            set { _SKU = value; }
            get { return _SKU; }

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
        private string _sqlse;
        public string sqlse
        {
            set { _sqlse = value; }
            get { return _sqlse; }

        }
        private string _MAKERID;
        public string MAKERID
        {
            set { _MAKERID = value; }
            get { return _MAKERID; }

        }
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
        #endregion

        #region sql
        string setsql = @"
SELECT 
A.TRID AS TRID,
A.TRANSFERS_DATE AS TRANSFERS_DATE,
A.TRANSFERS_MAKERID AS TRANSFERS_MAKERID,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.TRANSFERS_MAKERID )  AS TRANSFERS_MEMBER,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.MAKERID ) AS MAKER,
A.DATE AS DATE
FROM TRANSFERS_MST A
";
        #region setsqlo
        string setsqlo = @"
INSERT INTO 
TRANSFERS_DET
(
TRKEY,
TRID,
SN,
REMARK,
YEAR,
MONTH,
DAY
)
VALUES
(
@TRKEY,
@TRID,
@SN,
@REMARK,
@YEAR,
@MONTH,
@DAY

)
";
        #endregion
        #region setsqlt
        string setsqlt = @"
INSERT INTO 
TRANSFERS_MST
(
TRID,
TRANSFERS_DATE,
TRANSFERS_MAKERID,
DATE,
MAKERID,
YEAR,
MONTH,
DAY
)
VALUES
(
@TRID,
@TRANSFERS_DATE,
@TRANSFERS_MAKERID,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        #endregion
        string setsqlth = @"
UPDATE TRANSFERS_MST SET 
TRANSFERS_DATE=@TRANSFERS_DATE,
TRANSFERS_MAKERID=@TRANSFERS_MAKERID,
DATE=@DATE,
MAKERID=@MAKERID,
YEAR=@YEAR,
MONTH=@MONTH,
DAY=@DAY

";
        #region setsqlf
        string setsqlf = @"
INSERT INTO GODE
(
GEKEY,
GODEID,
SN,
WAREID,
GECOUNT,
SKU,
STORAGEID,
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
@GECOUNT,
@SKU,
@STORAGEID,
@BATCHID,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        #endregion
        string setsqlfi = @"

SELECT 
A.TRKEY AS 索引,
A.TRID AS 调拨单号, 
A.SN AS 项次,
C.WAREID AS ID,
D.CO_WAREID AS 料号,
D.WNAME AS 品名,
D.SPEC AS 规格,
D.CWAREID AS 客户料号,
C.GECOUNT AS 调拨数量,
F.TRANSFERS_DATE AS 调拨日期,
F.TRANSFERS_MAKERID AS 调拨员工号,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.TRANSFERS_MAKERID )  AS 调拨员,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.MAKERID )  AS 制单人,
F.DATE AS 制单日期,
H.STORAGENAME AS 仓库,
C.BATCHID AS 批号,
C.SKU AS 库存单位,
A.REMARK AS 备注
FROM TRANSFERS_DET A 
LEFT JOIN Gode   C ON A.TRKEY=C.GEKEY
LEFT JOIN WAREINFO D ON C.WAREID=D.WAREID
LEFT JOIN CUSTOMERINFO_MST E ON D.CUID=E.CUID
LEFT JOIN TRANSFERS_MST F ON A.TRID=F.TRID
LEFT JOIN STORAGEINFO H ON H.STORAGEID=C.STORAGEID
";
        string setsqlse = @"
INSERT INTO MATERE
(
MRKEY,
MATEREID,
SN,
WAREID,
MRCOUNT,
SKU,
STORAGEID,
BATCHID,
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
@SN,
@WAREID,
@MRCOUNT,
@SKU,
@STORAGEID,
@BATCHID,
@DATE,
@MAKERID,
@YEAR,
@MONTH,
@DAY
)
";
        #endregion
        public CTRANSFERS()
        {
            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            GETID = bc.numYM(10, 4, "0001", "SELECT * FROM TRANSFERS_MST", "TRID", "TR");
            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
            sqlf = setsqlf;
            sqlfi = setsqlfi;
            sqlse = setsqlse;
        }
        #region ask
        public DataTable ask(string TRID)
        {
            string sql1 = sqlo;
            DataTable dtt = bc.getdt(sqlfi + " WHERE A.TRID='" + TRID + "' ORDER BY A.TRKEY ASC");
            return dtt;
        }
        #endregion


    }
}
