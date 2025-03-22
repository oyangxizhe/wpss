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
    public class CADVANCE_RECEIVABLES
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
A.ARID AS 预收款单号,
A.CUID AS 客户代码,
B.CNAME AS 客户名称,
A.ADVANCE_RECEIVABLES_MAKERID AS 经手人工号,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.ADVANCE_RECEIVABLES_MAKERID) AS 经手人,
A.ADVANCE_RECEIVABLES_DATE AS 预收日期,
CASE WHEN A.IF_ALREADY_USE='Y' THEN '已冲'
ELSE '未冲'
END  AS 冲减否,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A.MAKERID) AS 制单人,
A.DATE AS 制单日期,
A.REMARK AS 备注,
C.ADVANCE_RECEIVABLES AS 预收金额
FROM ADVANCE_RECEIVABLES A 
LEFT JOIN CUSTOMERINFO_MST B ON A.CUID=B.CUID
LEFT JOIN GODE C ON A.ARKEY=C.GEKEY
";


        #endregion
        #region setsqlo
        string setsqlo = @"
INSERT INTO ADVANCE_RECEIVABLES
(
ARKEY,
ARID,
CUID,
ADVANCE_RECEIVABLES_MAKERID,
ADVANCE_RECEIVABLES_DATE,
IF_ALREADY_USE,
REMARK,
MAKERID,
DATE,
YEAR,
MONTH,
DAY
)
VALUES
(
@ARKEY,
@ARID,
@CUID,
@ADVANCE_RECEIVABLES_MAKERID,
@ADVANCE_RECEIVABLES_DATE,
@IF_ALREADY_USE,
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
ADVANCE_RECEIVABLES,
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
@ADVANCE_RECEIVABLES,
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
ADVANCE_RECEIVABLES,
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
@ADVANCE_RECEIVABLES,
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
UPDATE ADVANCE_RECEIVABLES SET 
CUID=@CUID,
ADVANCE_RECEIVABLES_MAKERID=@ADVANCE_RECEIVABLES_MAKERID,
ADVANCE_RECEIVABLES_DATE=@ADVANCE_RECEIVABLES_DATE,
REMARK=@REMARK,
MAKERID=@MAKERID,
DATE=@DATE

";
        #endregion
        #region setsqlfi
        string setsqlfi = @"
UPDATE GODE SET 
ADVANCE_RECEIVABLES=@ADVANCE_RECEIVABLES,
MAKERID=@MAKERID,
DATE=@DATE
";
        #endregion
        DataTable dtx2 = new DataTable();
        DataTable dt4 = new DataTable();

        public CADVANCE_RECEIVABLES()
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
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM ADVANCE_RECEIVABLES", "ARID", "AR");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
    }
}
