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
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace XizheC
{
    public  class CUSER
    {
        basec bc = new basec();
        #region nature
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private string _USID;
        public string USID
        {
            set { _USID = value; }
            get { return _USID; }

        }
        private string _UNAME;
        public string UNAME
        {
            set { _UNAME = value; }
            get { return _UNAME; }

        }
        private string _EMID;
        public string EMID
        {
            set { _EMID = value; }
            get { return _EMID; }

        }
        private string _GEID;
        public string GEID
        {
            set { _GEID = value; }
            get { return _GEID; }

        }
        private string _ENAME;
        public string ENAME
        {
            set { _ENAME = value; }
            get { return _ENAME; }

        }
        private string _DEPART;
        public string DEPART
        {
            set { _DEPART = value; }
            get { return _DEPART; }

        }
        private string _USER_GROUP;
        public string USER_GROUP
        {
            set { _USER_GROUP = value; }
            get { return _USER_GROUP; }

        }
        private string _MAKERID;
        public string MAKERID
        {
            set { _MAKERID = value; }
            get { return _MAKERID; }

        }
        private string _PWD;
        public string PWD
        {
            set { _PWD = value; }
            get { return _PWD; }

        }
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

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
        #endregion
        #region sql
        string setsql = @"
SELECT
A.USID AS 用户编号,
A.UNAME AS 用户名,
A.EMID AS 员工编号,
B.ENAME AS 姓名,
A.USER_GROUP AS 用户组,
(SELECT ENAME FROM EMPLOYEEINFO  WHERE EMID=A.MAKERID) AS 制单人,
A.DATE AS 制单日期 
FROM   USERINFO  A 
LEFT JOIN EMPLOYEEINFO B ON A.EMID=B.EMID

";
        string setsqlo = @"
INSERT INTO USERINFO(
USID, 
UNAME, 
PWD, 
EMID, 
USER_GROUP,
MAKERID,
DATE,
YEAR,
MONTH

) VALUES 

(
@USID, 
@UNAME, 
@PWD, 
@EMID, 
@USER_GROUP,
@MAKERID,
@DATE,
@YEAR,
@MONTH


)

";


        string setsqlt = @"
UPDATE USERINFO SET 
USID=@USID,
UNAME=@UNAME,
PWD=@PWD,
EMID=@EMID,
USER_GROUP=@USER_GROUP,
MAKERID=@MAKERID,
DATE=@DATE,
YEAR=@YEAR,
MONTH=@MONTH

";
        string setsqlth = @"
INSERT INTO 
AUTHORIZATION_USER
(
AUID,
USID,
STATUS,
LOGIN_DATE,
LEAVE_DATE,
YEAR,
MONTH
)  
VALUES 
(
@AUID,
@USID,
@STATUS,
@LOGIN_DATE,
@LEAVE_DATE,
@YEAR,
@MONTH
)";
       


        #endregion
        public CUSER()
        {
            sql = setsql;
            sqlo = setsqlo;
            sqlt = setsqlt;
            sqlth = setsqlth;
        }
     
    
        
        #region JUAGE_LOGIN_IF_SUCCESS
        public bool JUAGE_LOGIN_IF_SUCCESS(string UNAME, string PWD)
        {
            bool b = false;
            try
            {
                byte[] B = bc.GetMD5(PWD);
                SqlConnection sqlcon = bc.getcon();
                string sql1 = "SELECT * FROM USERINFO WHERE PWD=@PWD and UNAME=@UNAME";
                SqlCommand sqlcom = new SqlCommand(sql1, sqlcon);
                sqlcom.Parameters.Add("@PWD", SqlDbType.Binary, 50).Value = B;
                sqlcom.Parameters.Add("@UNAME", SqlDbType.VarChar, 50).Value = UNAME;
                sqlcon.Open();
                sqlcom.ExecuteNonQuery();
                if (sqlcom.ExecuteScalar().ToString() != "")
                {
                    string sql = @"SELECT B.DEPART,B.EMID,B.ENAME,A.USID AS USID,A.UNAME FROM USERINFO A 
LEFT JOIN EMPLOYEEINFO B ON A.EMID =B.EMID WHERE A.UNAME='" + UNAME + "'";
                    DataTable dt = basec.getdts(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DEPART = dt.Rows[0]["DEPART"].ToString();
                        ENAME = dt.Rows[0]["ENAME"].ToString();
                        EMID = dt.Rows[0]["EMID"].ToString();
                        USID = dt.Rows[0]["USID"].ToString();
                    }
                    b = true;
                }
                sqlcon.Close();
            }
            catch (Exception)
            {

            }
            return b;
        }
        #endregion

        public string GETID()
        {
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM USERINFO", "USID", "US");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        public string GETID_AUID()
        {
            string v1 = bc.numYM(10, 4, "0001", "SELECT * FROM AUTHORIZATION_USER", "AUID", "AU");
            string GETID = "";
            if (v1 != "Exceed Limited")
            {
                GETID = v1;
            }
            return GETID;
        }
        #region SQlcommandE
        public  void SQlcommandE(string sql)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss");
            string varMakerID = bc.getOnlyString("SELECT EMID FROM USERINFO WHERE USID='" +USID + "'");
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
            sqlcom.Parameters.Add("@AUID", SqlDbType.VarChar, 20).Value =GEID;
            sqlcom.Parameters.Add("@USID", SqlDbType.VarChar, 20).Value = USID;
            sqlcom.Parameters.Add("@STATUS", SqlDbType.VarChar, 20).Value = "Y";
            sqlcom.Parameters.Add("@LOGIN_DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcom.Parameters.Add("@LEAVE_DATE", SqlDbType.VarChar, 20).Value = "";
            sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
            sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
            sqlcon.Open();
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        #endregion

        #region save IDVALUE
        public void save(string TABLENAME, string COLUMNID, string COLUMNNAME, string IDVALUE,
            string NAMEVALUE, string INFOID, string INFONAME, string COLUMNID_o, string COLUMNNAME_o,
            string IDVALUE_o, string NAMEVALUE_o, string INFOID_o)
        {
         
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Replace("/", "-");
            string v1 = bc.getOnlyString("SELECT " + COLUMNNAME + " FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + IDVALUE + "'");
            string v2 = bc.getOnlyString("SELECT " + COLUMNID_o + " FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + IDVALUE + "'");
        
            //string varMakerID;
            if (!bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + IDVALUE + "'"))
            {
                if (bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNNAME + "='" + NAMEVALUE + "'"))
                {
                    ErrowInfo = INFONAME + "已经存在于系统！";
                    IFExecution_SUCCESS = false;
                }
                else if (bc.exists("SELECT " + COLUMNID_o + " FROM " + TABLENAME + " WHERE " + COLUMNID_o + "='" + IDVALUE_o + "'"))
                {
                    ErrowInfo = INFOID_o + "已经存在于系统！";
                    IFExecution_SUCCESS = false;
                }
                else
                {
                    SQlcommandE(sqlo, IDVALUE, NAMEVALUE);
                    IFExecution_SUCCESS = true;
                }

            }
            else if (v1 != NAMEVALUE && v2 == IDVALUE_o)
            {

                if (bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNNAME + "='" + NAMEVALUE + "'"))
                {
                    ErrowInfo = INFONAME + "已经存在于系统！";
                    IFExecution_SUCCESS = false;
                }
                else
                {
                    SQlcommandE(sqlt + " WHERE " + COLUMNID + "='" + IDVALUE + "'", IDVALUE, NAMEVALUE);
                    IFExecution_SUCCESS = true;
                }
            }
            else if (v1 == NAMEVALUE && v2 != IDVALUE_o)
            {
                
                if (bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNID_o + "='" + IDVALUE_o + "'"))
                {
                    ErrowInfo = INFOID_o + "已经存在于系统！";
                    IFExecution_SUCCESS = false;
                }
                else
                {
                    SQlcommandE(sqlt + " WHERE " + COLUMNID + "='" + IDVALUE + "'", IDVALUE, NAMEVALUE);
                    IFExecution_SUCCESS = true;
                }
            }
            else if (v1 != NAMEVALUE && v2 != IDVALUE_o)
            {

                if (bc.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNNAME + "='" + NAMEVALUE + "'"))
                {
                    ErrowInfo = INFONAME + "已经存在于系统！";
                    IFExecution_SUCCESS = false;

                }
                else if (bc.exists("SELECT " + COLUMNID_o + " FROM " + TABLENAME + " WHERE " + COLUMNID_o + "='" + IDVALUE_o + "'"))
                {
                    ErrowInfo = INFOID_o + "已经存在于系统！";
                    IFExecution_SUCCESS = false;
                }
                else
                {
                    SQlcommandE(sqlt + " WHERE " + COLUMNID + "='" + IDVALUE + "'", IDVALUE, NAMEVALUE);
                    IFExecution_SUCCESS = true;
                }

            }
            else
            {
                SQlcommandE(sqlt + " WHERE " + COLUMNID + "='" + IDVALUE + "'", IDVALUE, NAMEVALUE);
                IFExecution_SUCCESS = true;
            }

        }
        #endregion

        #region SQlcommandE
        protected void SQlcommandE(string sql, string IDVALUE, string NAMEVALUE)
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss").Replace("/", "-");
            Byte[] B = bc.GetMD5(PWD);
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
            sqlcom.Parameters.Add("@USID", SqlDbType.VarChar, 20).Value = IDVALUE;
            sqlcom.Parameters.Add("@UNAME", SqlDbType.VarChar, 20).Value = NAMEVALUE;
            sqlcom.Parameters.Add("@PWD", SqlDbType.Binary, 50).Value = B;
            sqlcom.Parameters.Add("@EMID", SqlDbType.VarChar, 20).Value = EMID;
            sqlcom.Parameters.Add("@USER_GROUP", SqlDbType.VarChar, 20).Value = USER_GROUP;
            sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = MAKERID;
            sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcom.Parameters.Add("@YEAR", SqlDbType.VarChar, 20).Value = year;
            sqlcom.Parameters.Add("@MONTH", SqlDbType.VarChar, 20).Value = month;
            sqlcon.Open();
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        #endregion
      
    }
}
