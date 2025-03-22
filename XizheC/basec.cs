using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net;
using System.Data.OleDb;
using System.Xml;
namespace XizheC
{
    public class basec
    {
        #region nature
    
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
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private bool _IFExecutionSUCCESS;
        public bool IFExecution_SUCCESS
        {
            set { _IFExecutionSUCCESS = value; }
            get { return _IFExecutionSUCCESS; }

        }
        int i, j;
   
 
 
        #region RETURN_APPOINT_UNTIL_CHAR
        public string RETURN_APPOINT_UNTIL_CHAR(string HAVE_NAME_STRING, int START, char C1)
        {
            string v = "";
            if (HAVE_NAME_STRING.Length > 0 && HAVE_NAME_STRING.Length >= START)
            {
                int q = Convert.ToInt32(C1);
                for (int i = START - 1; i < HAVE_NAME_STRING.Length; i++)
                {
                    int p = Convert.ToInt32(HAVE_NAME_STRING[i]);

                    if (p != q)
                    {
                        v = v + HAVE_NAME_STRING[i];
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return v;
        }
        #endregion
        #region RETURN_SERVER_IP_OR_DOMAIN
        public string RETURN_SERVER_IP_OR_DOMAIN()
        {
            string v = "";
            if (File.Exists(System.IO.Path.GetFullPath("Configuration.config")))
            {
                //MessageBox.Show(GetSERVER_IP(System.IO.Path.GetFullPath("项目管理系统客户端.exe.config")));
                v = RETURN_APPOINT_UNTIL_CHAR(GetSERVER_IP(System.IO.Path.GetFullPath("Configuration.config")), 8, '/');
            }
            else
            {
                MessageBox.Show("不存在指定的配置文件");
            }
            return v;
        }
        #endregion
        #region GetSERVER_IP
        public string GetSERVER_IP(string Dir)
        {
            //获取客户端应用程序及服务器端升级程序的最近一次更新版本
            string LastUpdateVersion = "";
            string AutoUpdaterFileName = Dir;
            if (!File.Exists(AutoUpdaterFileName))
                return LastUpdateVersion;
            //打开xml文件  
            FileStream myFile = new FileStream(AutoUpdaterFileName, FileMode.Open);
            //xml文件阅读器  
            XmlTextReader xml = new XmlTextReader(myFile);
            while (xml.Read())
            {
                if (xml.Name == "endpoint")
                {  //获取升级文档的最后一次更新版本
                    LastUpdateVersion = xml.GetAttribute("address");
                    break;
                }
            }
            xml.Close();
            myFile.Close();
            return LastUpdateVersion;
        }
        #endregion
        #region  建立数据库连接
        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回SqlConnection对象</returns>
        /// 
        public SqlConnection getcon()
        {

            string M_str_sqlcon = ConfigurationManager.AppSettings["ConnectionDB"].ToString();
            SqlConnection myCon = new SqlConnection(M_str_sqlcon);
            return myCon;
        }
        #endregion
       
        #region  执行SqlCommand命令
        /// <summary>
        /// 执行SqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public void getcom(string M_str_sqlstr)
        {
            SqlConnection sqlcon = this.getcon();
            sqlcon.Open();
            SqlCommand sqlcom = new SqlCommand(M_str_sqlstr, sqlcon);
            sqlcom.ExecuteNonQuery();
            sqlcom.Dispose();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        #endregion
        #region getcoms
        public static void getcoms(string M_str_sqlstr)
        {
            basec bc=new basec ();
            SqlConnection sqlcon = bc.getcon();
            sqlcon.Open();
            SqlCommand sqlcom = new SqlCommand(M_str_sqlstr, sqlcon);
            sqlcom.ExecuteNonQuery();
            sqlcom.Dispose();
            sqlcon.Close();
            sqlcon.Dispose();
        }
        #endregion

        #region  创建DataSet对象
        /// <summary>
        /// 创建一个DataSet对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <param name="M_str_table">表名</param>
        /// <returns>返回DataSet对象</returns>
        public DataSet getds(string M_str_sqlstr, string M_str_table)
        {
            SqlConnection sqlcon = this.getcon();
            SqlDataAdapter sqlda = new SqlDataAdapter(M_str_sqlstr, sqlcon);
            DataSet myds = new DataSet();
            sqlda.Fill(myds, M_str_table);
            return myds;
        }
        #endregion

        #region  创建SqlDataReader对象
        /// <summary>
        /// 创建一个SqlDataReader对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <returns>返回SqlDataReader对象</returns>
        public SqlDataReader getread(string M_str_sqlstr)
        {
            SqlConnection sqlcon = this.getcon();
            SqlCommand sqlcom = new SqlCommand(M_str_sqlstr, sqlcon);
            sqlcon.Open();
            SqlDataReader sqlread = sqlcom.ExecuteReader(CommandBehavior.CloseConnection);
            return sqlread;
        }
        #endregion

        public DataTable table(string M_str_sql)
        {
            SqlConnection sqlcon = this.getcon();
            SqlCommand sqlcmd = new SqlCommand(M_str_sql, sqlcon);
            sqlcon.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            sqlcon.Close();
            GC.Collect();
            return dt;
        }
        public DataTable getdt(string M_str_sql)
        {
            SqlConnection sqlcon = this.getcon();
            SqlCommand sqlcom = new SqlCommand(M_str_sql, sqlcon);
            SqlDataAdapter da = new SqlDataAdapter(sqlcom);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }
        public static DataTable getdts(string M_str_sql)
        {
            basec bc=new basec ();
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(M_str_sql, sqlcon);
            SqlDataAdapter da = new SqlDataAdapter(sqlcom);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }
        public SqlDataAdapter getda(string M_str_sql)
        {
            SqlConnection sqlcon = this.getcon();
            SqlCommand sqlcom = new SqlCommand(M_str_sql, sqlcon);
            SqlDataAdapter da = new SqlDataAdapter(sqlcom);
            return da;


        }
        #region 编号YM
        public string numYM(int digit, int wcodedigit, string wcode, string sql, string tbColumns, string prifix)
        {
            string year, month;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            string P_str_Code, t, r, sql1, q = "";
            int P_int_Code, w, w1;

            sql1 = sql + " WHERE YEAR='" + year + "' AND  MONTH='" + month + "'";
            SqlDataReader sqlread = this.getread(sql1);
            DataTable dt = this.getdt(sql1);
            sqlread.Read();
            if (sqlread.HasRows)
            {
                P_str_Code = Convert.ToString(dt.Rows[(dt.Rows.Count - 1)][tbColumns]);
                w1 = digit - wcodedigit;
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(w1, wcodedigit)) + 1;
                t = Convert.ToString(P_int_Code);
                w = wcodedigit - t.Length;
                if (w >= 0)
                {
                    while (w >= 1)
                    {
                        q = q + "0";
                        w = w - 1;

                    }
                    r = prifix + year + month + q + P_int_Code;
                }
                else
                {
                    r = "Exceed Limited";

                }

            }
            else
            {
                r = prifix + year + month + wcode;
            }
            sqlread.Close();
            return r;
        }
        #endregion
        #region 编号YMD
        public string numYMD(int digit, int wcodedigit, string wcode, string sql, string tbColumns, string prifix)
        {
            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            string P_str_Code, t, r, sql1, q = "";
            int P_int_Code, w, w1;

            sql1 = sql + " WHERE YEAR='" + year + "' AND  MONTH='" + month + "' AND DAY='" + day + "'";
            SqlDataReader sqlread = this.getread(sql1);
            DataTable dt = this.getdt(sql1);
            sqlread.Read();
            if (sqlread.HasRows)
            {
                P_str_Code = Convert.ToString(dt.Rows[(dt.Rows.Count - 1)][tbColumns]);
                w1 = digit - wcodedigit;
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(w1, wcodedigit)) + 1;
                t = Convert.ToString(P_int_Code);
                w = wcodedigit - t.Length;
                if (w >= 0)
                {
                    while (w >= 1)
                    {
                        q = q + "0";
                        w = w - 1;

                    }
                    r = prifix + year + month + day + q + P_int_Code;
                }
                else
                {
                    r = "Exceed Limited";

                }

            }
            else
            {
                r = prifix + year + month + day + wcode;
            }
            sqlread.Close();
            return r;
        }
        #endregion
        #region 编号NOYMD
        public string numNOYMD(int digit, int wcodedigit, string wcode, string sql, string tbColumns, string prifix)
        {

            string P_str_Code, t, r, sql1, q = "";
            int P_int_Code, w, w1;

            sql1 = sql;
            SqlDataReader sqlread = this.getread(sql1);
            DataTable dt = this.getdt(sql1);
            sqlread.Read();
            if (sqlread.HasRows)
            {
                P_str_Code = Convert.ToString(dt.Rows[(dt.Rows.Count - 1)][tbColumns]);
                w1 = digit - wcodedigit;
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(w1, wcodedigit)) + 1;
                t = Convert.ToString(P_int_Code);
                w = wcodedigit - t.Length;
                if (w >= 0)
                {
                    while (w >= 1)
                    {
                        q = q + "0";
                        w = w - 1;

                    }
                    r = prifix + q + P_int_Code;
                }
                else
                {
                    r = "Exceed Limited";

                }


            }
            else
            {
                r = prifix + wcode;
            }
            sqlread.Close();
            return r;
        }
        #endregion
        #region 编号Restriction
        public string Restriction(int digit, int wcodedigit, string wcode, string sql, string tbColumns, string prifix)
        {
            string year, month, day;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            day = DateTime.Now.ToString("dd");
            string P_str_Code, t, r, sql1, q = "";
            int P_int_Code, w, w1;

            sql1 = sql;
            SqlDataReader sqlread = this.getread(sql1);
            DataTable dt = this.getdt(sql1);
            sqlread.Read();
            if (sqlread.HasRows)
            {
                P_str_Code = Convert.ToString(dt.Rows[(dt.Rows.Count - 1)][tbColumns]);
                w1 = digit - wcodedigit;
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(w1, wcodedigit)) + 1;
                t = Convert.ToString(P_int_Code);
                w = wcodedigit - t.Length;
                if (w >= 0)
                {
                    while (w >= 1)
                    {
                        q = q + "0";
                        w = w - 1;

                    }
                    r = prifix + year + month + day + q + P_int_Code;
                }
                else
                {
                    r = "Exceed Limited";

                }

            }
            else
            {
                r = prifix + year + month + day + wcode;
            }
            sqlread.Close();
            return r;
        }
        #endregion
        #region 编号YMCU
        public string numYMCU(int digit, int wcodedigit, string wcode, string sql, string tbColumns, string prifix)
        {
            string year, month;
            year = DateTime.Now.ToString("yy");
            month = DateTime.Now.ToString("MM");
            string P_str_Code, t, r, sql1, q = "";
            int P_int_Code, w, w1;

            sql1 = sql;
            SqlDataReader sqlread = this.getread(sql1);
            DataTable dt = this.getdt(sql1);
            sqlread.Read();
            if (sqlread.HasRows)
            {
                P_str_Code = Convert.ToString(dt.Rows[(dt.Rows.Count - 1)][tbColumns]);
                w1 = digit - wcodedigit;
                P_int_Code = Convert.ToInt32(P_str_Code.Substring(w1, wcodedigit)) + 1;
                t = Convert.ToString(P_int_Code);
                w = wcodedigit - t.Length;
                if (w >= 0)
                {
                    while (w >= 1)
                    {
                        q = q + "0";
                        w = w - 1;

                    }
                    r = prifix + q + P_int_Code;
                }
                else
                {
                    r = "Exceed Limited";

                }

            }
            else
            {
                r = prifix + wcode;
            }
            sqlread.Close();
            return r;
        }
        #endregion
        #region yesno
        public int yesno(string vars)
        {
            int k = 1;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57 || p == 46)
                {
                    k = 1;
                }
                else
                {
                    k = 0; break;
                }

            }

            return k;

        }
        #endregion
        #region yesno1
        public int yesno1(string vars)
        {
            int k = 1;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57)
                {
                    k = 1;
                }
                else
                {
                    k = 0; break;
                }

            }

            return k;

        }
        #endregion
        #region checkphone
        public bool checkphone(string vars)
        {
            bool k = true;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57 || p == 46 || p == 45)
                {

                }
                else
                {
                    k = false;
                    break;
                }

            }

            return k;

        }
        #endregion
        #region checkEMAIL
        public bool checkEmail(string vars)
        {
            bool k = true;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57 || p == 46 || p >= 64 && p <= 90 || p >= 97 && p <= 122)
                {

                }
                else
                {
                    k = false;
                    break;
                }

            }
            return k;
        }
        #endregion
        #region checkNumber
        public bool checkNumber(string vars)
        {
            bool k = false;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 48 && p <= 57)
                {
                    k = true;
                    break;
                }
            }
            return k;
        }
        #endregion
        #region checkLetter
        public bool checkLetter(string vars)
        {
            bool k = false;
            int i;
            for (i = 0; i < vars.Length; i++)
            {
                int p = Convert.ToInt32(vars[i]);
                if (p >= 65 && p <= 90 || p >= 97 && p <= 122)
                {
                    k = true;
                    break;
                }
            }
            return k;
        }
        #endregion
        #region getstoragetable
        public DataTable getstoragetable()
        {
            DataTable dtk = new DataTable();
            dtk.Columns.Add("品号", typeof(string));
            dtk.Columns.Add("料号", typeof(string));
            dtk.Columns.Add("客户名称", typeof(string));
            dtk.Columns.Add("品名", typeof(string));
            dtk.Columns.Add("客户料号", typeof(string));
            dtk.Columns.Add("规格", typeof(string));
            dtk.Columns.Add("单位", typeof(string));
            dtk.Columns.Add("仓库", typeof(string));
            dtk.Columns.Add("批号", typeof(string));
            dtk.Columns.Add("库存数量", typeof(string));
            return dtk;
        }
        #endregion

        #region getstoragecount
        public DataTable getstoragecount()
        {
            int s1, s2;
            DataTable dtk = this.getstoragetable();
            DataTable dtk1 = new DataTable();
            DataTable dtk2 = new DataTable();
            string sqlk1 = @"
SELECT 
A.WAREID AS WAREID,
B.WNAME AS WNAME,
B.CWAREID AS CWAREID,
D.CNAME AS CNAME,
A.STORAGEID AS STORAGEID,
C.STORAGENAME AS STORAGENAME,
A.BATCHID AS BATCHID,
CAST(ROUND(SUM(A.GECOUNT),2) AS DECIMAL(18,2)) AS GECOUNT,
CASE WHEN SUM(A.FREECOUNT) IS NOT NULL THEN CAST(ROUND(SUM(A.FREECOUNT),2) AS DECIMAL(18,2))
ELSE 0
END 
AS FREECOUNT,
CASE WHEN SUM(A.FREECOUNT) IS NOT NULL THEN SUM(A.GECOUNT)+CAST(ROUND(SUM(A.FREECOUNT),2) AS DECIMAL(18,2))
ELSE SUM(A.GECOUNT)
END 
AS GECOUNTANDFREECOUNT
FROM GODE A
LEFT JOIN WAREINFO B ON A.WAREID=B.WAREID 
LEFT JOIN STORAGEINFO C ON C.STORAGEID=A.STORAGEID 
LEFT JOIN CUSTOMERINFO_MST D ON B.CUID =D.CUID 
WHERE A.WAREID IS NOT NULL
GROUP  BY 
A.WAREID,
B.WNAME,
A.STORAGEID,
A.BATCHID,
C.STORAGENAME,
B.CWAREID ,
D.CNAME 
ORDER BY
A.WAREID,
A.STORAGEID,
A.BATCHID

";

            string sqlk2 = @"
SELECT 
WAREID AS WAREID,
STORAGEID AS STORAGEID,
BATCHID AS BATCHID,
CAST(ROUND(SUM(MRCOUNT),2) AS DECIMAL(18,2)) AS MRCOUNT,
CASE WHEN SUM(FREECOUNT) IS NOT NULL THEN CAST(ROUND(SUM(FREECOUNT),2) AS DECIMAL(18,2))
ELSE 0
END 
AS FREECOUNT,
CASE WHEN SUM(FREECOUNT) IS NOT NULL THEN SUM(MRCOUNT)+CAST(ROUND(SUM(FREECOUNT),2) AS DECIMAL(18,2))
ELSE SUM(MRCOUNT)
END 
AS MRCOUNTANDFREECOUNT
FROM MATERE
WHERE 
WAREID IS NOT NULL
GROUP BY
WAREID,
STORAGEID,
BATCHID
ORDER BY
WAREID,
STORAGEID,
BATCHID

";

            dtk1 = this.getdt(sqlk1);
            dtk2 = this.getdt(sqlk2);

            for (s1 = 0; s1 < dtk1.Rows.Count; s1++)
            {
                decimal d1 = 0;
                string z = "";
                decimal dec1 = 0;
                for (s2 = 0; s2 < dtk2.Rows.Count; s2++)
                {
                    string v1 = dtk1.Rows[s1]["WAREID"].ToString();
                    string v2 = dtk1.Rows[s1]["STORAGEID"].ToString();
                    string v3 = dtk1.Rows[s1]["BATCHID"].ToString();
                    string v4 = dtk2.Rows[s2]["WAREID"].ToString();
                    string v5 = dtk2.Rows[s2]["STORAGEID"].ToString();
                    string v6 = dtk2.Rows[s2]["BATCHID"].ToString();
                    if (v1==v4 && v2==v5 && v3==v6)
                    {

                        dec1 = (decimal.Parse(dtk1.Rows[s1]["GECOUNTANDFREECOUNT"].ToString())) - (decimal.Parse(dtk2.Rows[s2]["MRCOUNTANDFREECOUNT"].ToString()));
                        z = Convert.ToString(dec1);
                        break;
                    }

                }

                if (z != "")
                {

                    d1 = decimal.Parse(z);
                  
                }
                else
                {

                    d1 = decimal.Parse(dtk1.Rows[s1]["GECOUNTANDFREECOUNT"].ToString());

                }
                if (d1 != 0)
                {
                    DataRow dr = dtk.NewRow();
                    dr["品号"] = dtk1.Rows[s1]["WAREID"].ToString();
                    dr["品名"] = dtk1.Rows[s1]["WNAME"].ToString();
                    DataTable dtx2 = this.getdt("select * from wareinfo where wareid='" + dtk1.Rows[s1]["WAREID"].ToString() + "'");
                    if (dtx2.Rows.Count > 0)//161109号发现客户有一个品号916050033在品号信息中不存在，导致程序出错
                    {
                        dr["规格"] = dtx2.Rows[0]["Spec"].ToString();
                        dr["单位"] = dtx2.Rows[0]["UNIT"].ToString();
                        dr["料号"] = dtx2.Rows[0]["CO_WAREID"].ToString();
                    }
                    dr["仓库"] = dtk1.Rows[s1]["STORAGENAME"].ToString();
                    dr["批号"] = dtk1.Rows[s1]["BATCHID"].ToString();
                    dr["库存数量"] = d1;
                    dr["客户名称"] = dtk1.Rows[s1]["CNAME"].ToString();
                    dr["客户料号"] = dtk1.Rows[s1]["CWAREID"].ToString();
                    dtk.Rows.Add(dr);

                }


            }

            return dtk;

        }
        #endregion


        #region JuageDeleteCount_MoreThanStorageCount
        public bool JuageDeleteCount_MoreThanStorageCount(string GodEID)
        {
            int i;
            bool z = false ;
            DataTable dt6 = this.getdt(@"
select A.WAREID AS WAREID,A.STORAGEID AS STORAGEID,B.STORAGENAME AS STORAGENAME,A.BATCHID AS BATCHID,
CASE WHEN SUM(A.FREECOUNT) IS NOT NULL THEN SUM(A.GECOUNT)+CAST(ROUND(SUM(A.FREECOUNT),2) AS DECIMAL(18,2))
ELSE SUM(A.GECOUNT)
END 
 AS GECOUNTANDFREECOUNT FROM GODE A LEFT JOIN STORAGEINFO B ON 
A.STORAGEID=B.STORAGEID  WHERE A.GODEID='" + GodEID  + "' GROUP BY A.WAREID,A.STORAGEID,B.STORAGENAME,"
           +" A.BATCHID ORDER BY A.WAREID,A.STORAGEID,A.BATCHID ASC");
            if (dt6.Rows.Count > 0)
            {
                for (i = 0; i < dt6.Rows.Count; i++)
                {
                    string c1, c2,c3;
                    c1 = dt6.Rows[i]["WAREID"].ToString();
                    c2 = dt6.Rows[i]["STORAGENAME"].ToString();
                    c3 = dt6.Rows[i]["BATCHID"].ToString();
                    DataRow[] dr = this.getstoragecount().Select("品号='" + c1 + "' and 仓库='" + c2 + "'AND 批号='"+c3+"'");
                    if (dr.Length > 0)
                    {
                        if (decimal.Parse(dr[0]["库存数量"].ToString()) < decimal.Parse(dt6.Rows[i]["GECOUNTANDFREECOUNT"].ToString()))
                        {
                           
                            ErrowInfo = "品号:" + dt6.Rows[i][0].ToString() + " 库存不足，不允许编辑或删除该单据";
                            z = true ;
                            break;
                        }
                    }
                    else
                    {
                       
                        ErrowInfo = "品号:" + dt6.Rows[i][0].ToString() + " 库存不足，不允许编辑或删除该单据";
                        z = true ;
                        break;
                    }

                }
            }
            return z;
        }
        #endregion
        #region JuageDeleteCount_MoreThanStorageCount
        public bool JuageDeleteCount_IFMoreThanTOTAL_ACTUAL_SELLCOUNT(string ORID,string SEID)
        {
            int i;
            bool b = false;
            CORDER corder = new CORDER();
            DataTable dt = GET_DT_TO_DV_TO_DT(corder.GET_TOTAL_ORDER(), "", "订单号='"+ORID +"'");
            DataTable dtx5 = this.getdt(@"
SELECT 
A.ORID AS ORID,
A.SEID AS SEID,
A.SN AS SN,
B.WAREID AS WAREID,
CAST(ROUND(SUM(B.MRCOUNT),2) AS DECIMAL(18,2)) AS MRCOUNT
FROM SELLTABLE_DET A 
LEFT JOIN  MATERE B
ON A.SEKEY=B.MRKEY 
WHERE  A.ORID='" + ORID  + "' AND A.SEID='" + SEID  + "' GROUP BY A.ORID,A.SEID,A.SN,B.WAREID");
            if (dt.Rows.Count > 0)
            {
                if (dtx5.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (b)
                         break;
                        for (j = 0; j < dtx5.Rows.Count; j++)
                        {
                            if (dt.Rows[i]["订单号"].ToString() == dtx5.Rows[j]["ORID"].ToString() && dt.Rows[i]["项次"].ToString() == dtx5.Rows[j]["SN"].ToString())
                            {
                                if (decimal.Parse(dtx5.Rows[j]["MRCOUNT"].ToString()) > decimal.Parse(dt.Rows[i]["实际累计销货数量"].ToString()))
                                {
                                    ErrowInfo = "项次：" + dt.Rows[i]["项次"].ToString() + " 本销货单累计删除数量："+dtx5.Rows[j]["MRCOUNT"].ToString() +
                                        " 大于订单该项次实际累计销货数量 " +  dt.Rows[i]["实际累计销货数量"].ToString()+" 不允许删除";
                                    b = true;
                                    break;

                                  
                                }
                            }
                        }
                    }

                }
            }
            return b;
        }
        #endregion

        #region juagedate
        public bool  juagedate(string StartDate, string EndDate)
        {
            bool b = true;
            if (StartDate  != "" && EndDate != "")
            {
                if (Convert.ToDateTime(StartDate ) <= Convert.ToDateTime(EndDate ))
                {
                    
                }
                else
                {
                    ErrowInfo = "截止日期需大于起始日期！";
                    return false;

                }
            }
            return b;
        }
        #endregion
        #region JuageCurrentDateIFAboveDeliveryDate
        public bool JuageCurrentDateIFAboveDeliveryDate(string CurrentDate, string DeliveryDate)
        {
            bool b = false ;
            if (CurrentDate != "" && DeliveryDate != "")
            {
                if (Convert.ToDateTime(CurrentDate)>=Convert.ToDateTime(DeliveryDate).AddDays (+1))
                {
                    return true;
                }
          
            }
            return b;
        }
        #endregion
        #region JuageCurrentDateIFAboveDeliveryDate
        public bool JuageCurrentDateIFAboveDeliveryDate(string ORIDorPUID, int OrderOrPurchase)
        {
            bool b = false;
            DataTable dt = new DataTable();
            if (OrderOrPurchase == 0)
            {
                dt = this.getdt("SELECT * FROM ORDER_DET WHERE ORID='" + ORIDorPUID + "'");
                foreach (DataRow dr in dt.Rows)
                {
                 
                    if (Convert.ToDateTime(DateTime.Now.ToString()) >= Convert.ToDateTime(dr["DELIVERYDATE"].ToString()).AddDays(+1))
                    {
                        b = true;
                        break;
                    }
                }

            }
            else
            {
                dt = this.getdt("SELECT * FROM PURCHASE_DET WHERE PUID='" + ORIDorPUID + "'");
                foreach (DataRow dr in dt.Rows)
                {

                    if (Convert.ToDateTime(DateTime.Now.ToString()) >= Convert.ToDateTime(dr["NEEDDATE"].ToString()).AddDays(+1))
                    {
                        b = true;
                        break;
                    }

                }

            }

            return b;

        }
        #endregion
        #region exists
        public bool exists(string sql)
        {
            DataTable dtx1 = this.getdt(sql);
            if (dtx1.Rows.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region maxstoragecount
        public DataTable getmaxstoragecount(string wareid)
        {
            DataTable dt = this.getstoragecount();
            DataTable dtu1 = new DataTable();
            DataRow[] dr = dt.Select("品号='" + wareid + "'");
            if (dr.Length > 0)
            {
                DataTable dtu = this.getstoragetable();
                for (i = 0; i < dr.Length; i++)
                {
                    DataRow dr1 = dtu.NewRow();
                    dr1["品号"] = dr[i]["品号"].ToString();
                    dr1["品名"] = dr[i]["品名"].ToString();
                    dr1["仓库"] = dr[i]["仓库"].ToString();
                    dr1["批号"] = dr[i]["批号"].ToString();
                    dr1["库存数量"] = dr[i]["库存数量"].ToString();
                    dtu.Rows.Add(dr1);

                }
                string s1 = "";
                string s2 = "";/*13111501*/
                decimal c1 = 0;
                decimal n = 0;
                string n1 = "";
                string n2 = "";/*13111501*/
                if (dtu.Rows.Count == 1)
                {

                    s1 = dtu.Rows[0]["仓库"].ToString();
                    s2 = dtu.Rows[0]["批号"].ToString();
                    c1 = decimal.Parse(dtu.Rows[0]["库存数量"].ToString());
                }
                else
                {
                    for (int j = 0; j < dtu.Rows.Count; j++)
                    {

                        decimal c2 = decimal.Parse(dtu.Rows[j]["库存数量"].ToString());

                        if (n > c2)
                        {

                        }
                        else if (n == c2)
                        {


                        }
                        else
                        {
                            n = c2;
                            n1 = dtu.Rows[j]["仓库"].ToString();
                            n2 = dtu.Rows[j]["批号"].ToString();

                        }
                    }
                    s1 = n1;
                    s2 = n2;/*13111501*/
                    c1 = n;
                }
                dtu1 = this.getstoragetable();
                DataRow dr2 = dtu1.NewRow();
                dr2["品号"] = dtu.Rows[0]["品号"].ToString();
                dr2["品名"] = dtu.Rows[0]["品名"].ToString();
                dr2["仓库"] = s1;
                dr2["批号"] = s2;
                dr2["库存数量"] = c1;
                dtu1.Rows.Add(dr2);
            }
            return dtu1;
        }
        #endregion
        #region getstorageid
        public string getstorageid(string storagetype)
        {
            string storageid = "";
            DataTable dtx3 = this.getdt("select * from tb_storageinfo where storagetype='" + storagetype + "'");
            if (dtx3.Rows.Count > 0)
            {
                storageid = dtx3.Rows[0][0].ToString();
            }

            return storageid;
        }
        #endregion
        #region checkingWareidAndstorage
        public string CheckingWareidAndStorage(string wareid, string storageType,string batchID)
        {
            string storagecount = "0";
            DataTable dt = this.getstoragecount();
            DataTable dtu1 = new DataTable();
            DataRow[] dr = dt.Select("品号= '" + wareid + "' and 仓库='" + storageType + "' and 批号='" + batchID  + "'");
            if (dr.Length > 0)
            {
                storagecount = dr[0]["库存数量"].ToString();
            }
            return storagecount;
        }
        #endregion
        #region getOnlyString
        public string getOnlyString(string sql)
        {
            string s2 = "";
            DataTable dtu2 = this.getdt(sql);
            if (dtu2.Rows.Count > 0)
            {
                s2 = dtu2.Rows[0][0].ToString();

            }

            return s2;
        }
        #endregion
        #region getprintinfo
        public DataTable getPrintInfo()
        {

            DataTable dtt = new DataTable();
            dtt.Columns.Add("销货单号", typeof(string));
            dtt.Columns.Add("订单号", typeof(string));
            dtt.Columns.Add("项次", typeof(string));
            dtt.Columns.Add("品号", typeof(string));
            dtt.Columns.Add("品名", typeof(string));
            dtt.Columns.Add("套件", typeof(string));
            dtt.Columns.Add("型号", typeof(string));
            dtt.Columns.Add("细节", typeof(string));
            dtt.Columns.Add("皮种", typeof(string));
            dtt.Columns.Add("颜色", typeof(string));
            dtt.Columns.Add("线色", typeof(string));
            dtt.Columns.Add("海棉厚度", typeof(string));
            dtt.Columns.Add("销售单价", typeof(decimal));
            dtt.Columns.Add("折扣率", typeof(decimal));
            dtt.Columns.Add("税率", typeof(decimal));
            dtt.Columns.Add("订单数量", typeof(decimal));
            dtt.Columns.Add("销货数量", typeof(decimal));
            dtt.Columns.Add("未税金额", typeof(decimal), "销售单价*折扣率*销货数量");
            dtt.Columns.Add("税额", typeof(decimal), "销售单价*折扣率*销货数量*税率/100");
            dtt.Columns.Add("含税金额", typeof(decimal), "销售单价*折扣率*销货数量*(1+税率/100)");
            dtt.Columns.Add("客户代码", typeof(string));
            dtt.Columns.Add("客户", typeof(string));
            dtt.Columns.Add("电话", typeof(string));
            dtt.Columns.Add("地址", typeof(string));
            dtt.Columns.Add("订货日期", typeof(string));
            dtt.Columns.Add("交货日期", typeof(string));
            dtt.Columns.Add("加急否", typeof(string));
            dtt.Columns.Add("制单人", typeof(string));
            dtt.Columns.Add("制单日期", typeof(string));
            return dtt;


        }
        #endregion
        #region ask
        public DataTable ask(string sqlcondition, int GROUP, int printselltable)
        {

            string M_str_sql1 = @"select ORID ,SN ,WareID ,OCOUNT ,ORDERDATE,DELIVERYDATE,URGENT,SELLUNITPRICE,DISCOUNTRATE,TAXRATE,CUID FROM TB_ORDER";

            DataTable dtt = this.getPrintInfo();
            DataTable dtx6 = this.getdt(sqlcondition);
            if (dtx6.Rows.Count > 0)
            {
                for (int i1 = 0; i1 < dtx6.Rows.Count; i1++)
                {
                    DataRow dr = dtt.NewRow();
                    dr["销货单号"] = dtx6.Rows[i1]["SEID"].ToString();
                    dr["订单号"] = dtx6.Rows[i1]["ORID"].ToString();
                    if (printselltable == 1)
                    {

                    }
                    else
                    {
                        dr["项次"] = dtx6.Rows[i1]["SN"].ToString();
                    }
                    dr["品号"] = dtx6.Rows[i1]["WAREID"].ToString();
                    DataTable dtx2 = this.getdt("select * from tb_wareinfo where wareid='" + dtx6.Rows[i1]["WAREID"].ToString() + "'");
                    dr["品名"] = dtx2.Rows[0]["WNAME"].ToString();
                    dr["套件"] = dtx2.Rows[0]["ExternalM"].ToString();
                    dr["型号"] = dtx2.Rows[0]["TYPE"].ToString();
                    dr["细节"] = dtx2.Rows[0]["DETAIL"].ToString();
                    dr["皮种"] = dtx2.Rows[0]["Leather"].ToString();
                    dr["颜色"] = dtx2.Rows[0]["COLOR"].ToString();
                    dr["线色"] = dtx2.Rows[0]["StitchingC"].ToString();
                    dr["海棉厚度"] = dtx2.Rows[0]["Thickness"].ToString();
                    if (GROUP == 0)
                    {
                        dr["销货数量"] = dtx6.Rows[i1]["SECOUNT"].ToString();
                        dr["制单人"] = dtx6.Rows[i1]["MAKER"].ToString();
                        dr["制单日期"] = dtx6.Rows[i1]["DATE"].ToString();
                    }
                    else
                    {
                        dr["销货数量"] = dtx6.Rows[i1][3].ToString();

                    }

                    dtt.Rows.Add(dr);

                }
            }

            DataTable dtx4 = this.getdt(M_str_sql1);
            if (dtx4.Rows.Count > 0)
            {
                for (i = 0; i < dtt.Rows.Count; i++)
                {
                    for (int j = 0; j < dtx4.Rows.Count; j++)
                    {
                        if (printselltable == 1)
                        {
                            if (dtt.Rows[i]["订单号"].ToString() == dtx4.Rows[j]["ORID"].ToString() && dtt.Rows[i]["品号"].ToString() == dtx4.Rows[j]["WAREID"].ToString())
                            {
                                dtt.Rows[i]["订单数量"] = dtx4.Rows[j]["OCOUNT"].ToString();
                                dtt.Rows[i]["订货日期"] = dtx4.Rows[j]["ORDERDATE"].ToString();
                                dtt.Rows[i]["交货日期"] = dtx4.Rows[j]["DELIVERYDATE"].ToString();
                                dtt.Rows[i]["加急否"] = dtx4.Rows[j]["URGENT"].ToString();
                                dtt.Rows[i]["销售单价"] = dtx4.Rows[j]["SELLUNITPRICE"].ToString();
                                dtt.Rows[i]["折扣率"] = dtx4.Rows[j]["DISCOUNTRATE"].ToString();
                                dtt.Rows[i]["税率"] = dtx4.Rows[j]["TAXRATE"].ToString();
                                dtt.Rows[i]["客户代码"] = dtx4.Rows[j]["CUID"].ToString();
                                DataTable dtx7 = this.getdt("select * from tb_customerinfo where cuid='" + dtx4.Rows[j]["CUID"].ToString() + "'");
                                dtt.Rows[i]["客户"] = dtx7.Rows[0]["CNAME"].ToString();
                                dtt.Rows[i]["电话"] = dtx7.Rows[0]["PHONE"].ToString();
                                dtt.Rows[i]["地址"] = dtx7.Rows[0]["ADDRESS"].ToString();
                                break;
                            }
                        }
                        else
                        {

                            if (dtt.Rows[i]["订单号"].ToString() == dtx4.Rows[j]["ORID"].ToString() && dtt.Rows[i]["项次"].ToString() == dtx4.Rows[j]["SN"].ToString())
                            {
                                dtt.Rows[i]["订单数量"] = dtx4.Rows[j]["OCOUNT"].ToString();
                                dtt.Rows[i]["订货日期"] = dtx4.Rows[j]["ORDERDATE"].ToString();
                                dtt.Rows[i]["交货日期"] = dtx4.Rows[j]["DELIVERYDATE"].ToString();
                                dtt.Rows[i]["加急否"] = dtx4.Rows[j]["URGENT"].ToString();
                                dtt.Rows[i]["销售单价"] = dtx4.Rows[j]["SELLUNITPRICE"].ToString();
                                dtt.Rows[i]["折扣率"] = dtx4.Rows[j]["DISCOUNTRATE"].ToString();
                                dtt.Rows[i]["税率"] = dtx4.Rows[j]["TAXRATE"].ToString();
                                dtt.Rows[i]["客户代码"] = dtx4.Rows[j]["CUID"].ToString();
                                DataTable dtx7 = this.getdt("select * from tb_customerinfo where cuid='" + dtx4.Rows[j]["CUID"].ToString() + "'");
                                dtt.Rows[i]["客户"] = dtx7.Rows[0]["CNAME"].ToString();
                                dtt.Rows[i]["电话"] = dtx7.Rows[0]["PHONE"].ToString();
                                dtt.Rows[i]["地址"] = dtx7.Rows[0]["ADDRESS"].ToString();
                                break;
                            }

                        }

                    }
                }
            }
            return dtt;
        }
        #endregion
        public DataTable asko(string sql,int Need)
        {
            DataTable dt = new DataTable();

            /*销货单打印数据含项次*/
            string s31 = @"
select A.SEID AS 销货单号,A.ORID AS 订单号,G.ORDERDATE AS 订货日期,C.DELIVERYDATE AS 交货日期,C.URGENT AS 加急否,
A.SN as 项次,E.WareID as 品号,
B.WNAME AS 品名,B.SPEC as 规格,B.UNIT as 单位,B.CWAREID AS 客户料号,
C.SELLUNITPRICE AS 销售单价,C.TAXRATE AS 税率,
SUM(E.MRCount) as 销货数量 ,SUM(E.MRCOUNT*C.SELLUNITPRICE) AS 未税金额,SUM(E.MRCOUNT*C.SELLUNITPRICE*C.TAXRATE/100) 
AS 税额,SUM(E.MRCOUNT*C.SELLUNITPRICE*(1+C.TAXRATE/100)) AS 含税金额,C.CUID as 客户代码,
D.CName as 客户 ,H.PHONE AS 电话,H.ADDRESS AS 地址,F.SELLDATE AS 销货日期,(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=F.SELLERID )  AS 销货员
from SELLTABLE_DET A 
LEFT JOIN ORDER_DET C ON A.ORID=C.ORID AND A.SN=C.SN
LEFT JOIN CUSTOMERINFO_MST D ON C.CUID=D.CUID
LEFT JOIN MATERE E ON A.SEKEY=E.MRKEY
LEFT JOIN WAREINFO B ON E.WAREID=B.WAREID
LEFT JOIN SELLTABLE_MST F ON A.SEID=F.SEID
LEFT JOIN ORDER_MST G ON A.ORID=G.ORID
LEFT JOIN CUSTOMERINFO_DET H ON D.CUKEY=H.CUKEY";

            string s32 = @" GROUP BY A.SEID,A.ORID,A.SN,E.WAREID,B.WNAME,B.SPEC,B.UNIT,B.CWAREID,
C.SELLUNITPRICE,C.TAXRATE,C.CUID,D.CNAME,F.SELLDATE,F.SELLERID,G.ORDERDATE,C.DELIVERYDATE,C.URGENT,H.PHONE,H.ADDRESS ORDER BY A.SEID,A.SN";

        if (Need == 2)
        {
            dt = this.getdt(s31 + sql + s32);


        }
            return dt;
        }
        #region PrintOrder
        public DataTable PrintOrder(string sqlcondition)
        {

            string M_str_sql1 = @"select * FROM TB_ORDER";
            DataTable dtt = this.getPrintInfo();
            DataTable dtx6 = this.getdt(M_str_sql1 + sqlcondition);
            if (dtx6.Rows.Count > 0)
            {
                for (i = 0; i < dtx6.Rows.Count; i++)
                {
                    DataRow dr = dtt.NewRow();

                    dr["订单号"] = dtx6.Rows[i]["ORID"].ToString();
                    dr["项次"] = dtx6.Rows[i]["SN"].ToString();
                    dr["品号"] = dtx6.Rows[i]["WAREID"].ToString();
                    DataTable dtx2 = this.getdt("select * from tb_wareinfo where wareid='" + dtx6.Rows[i]["WAREID"].ToString() + "'");
                    dr["品名"] = dtx2.Rows[0]["WNAME"].ToString();
                    dr["套件"] = dtx2.Rows[0]["ExternalM"].ToString();
                    dr["型号"] = dtx2.Rows[0]["TYPE"].ToString();
                    dr["细节"] = dtx2.Rows[0]["DETAIL"].ToString();
                    dr["皮种"] = dtx2.Rows[0]["Leather"].ToString();
                    dr["颜色"] = dtx2.Rows[0]["COLOR"].ToString();
                    dr["线色"] = dtx2.Rows[0]["StitchingC"].ToString();
                    dr["海棉厚度"] = dtx2.Rows[0]["Thickness"].ToString();
                    dr["订单数量"] = dtx6.Rows[i]["OCOUNT"].ToString();
                    dr["订货日期"] = dtx6.Rows[i]["ORDERDATE"].ToString();
                    dr["交货日期"] = dtx6.Rows[i]["DELIVERYDATE"].ToString();
                    dr["加急否"] = dtx6.Rows[i]["URGENT"].ToString();
                    dr["客户代码"] = dtx6.Rows[i]["CUID"].ToString();
                    DataTable dtx7 = this.getdt("select * from tb_customerinfo where cuid='" + dtx6.Rows[i]["CUID"].ToString() + "'");
                    dr["客户"] = dtx7.Rows[0]["CNAME"].ToString();
                    dr["电话"] = dtx7.Rows[0]["PHONE"].ToString();
                    dr["地址"] = dtx7.Rows[0]["ADDRESS"].ToString();
                    dtt.Rows.Add(dr);

                }
            }
            return dtt;
        }
        #endregion
        public bool juageValueLimits(string[] arr, string b)
        {
            DataTable dtzz = new DataTable();
            dtzz.Columns.Add("X", typeof(string));

            for (i = 0; i < arr.Length; i++)
            {
                DataRow dr = dtzz.NewRow();
                dr["X"] = arr[i];
                dtzz.Rows.Add(dr);
            }
            DataRow[] dr1 = dtzz.Select("X='" + b + "'");
            bool b1 = true;
            if (dr1.Length > 0)
            {

            }
            else
            {
                b1 = false;


            }
            return b1;
        }
        public bool juageValueLimits(string sql, string b)
        {
            DataTable dt = this.getdt(sql);
            DataTable dtzz = new DataTable();
            dtzz.Columns.Add("X", typeof(string));
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dtzz.NewRow();
                    dr["X"] = dt.Rows[i][0].ToString();
                    dtzz.Rows.Add(dr);
                }
            }
            DataRow dr1 = dtzz.NewRow();
            dr1["X"] = "";
            dtzz.Rows.Add(dr1);
            DataRow[] dr2 = dtzz.Select("X='" + b + "'");
            bool b1 = true;
            if (dr2.Length > 0)
            {

            }
            else
            {
                b1 = false;


            }
            return b1;
        }
        #region GetFileName
        public string GetFileName(string sql, string field)
        {
            string v2 = "";
            DataTable dt1 = this.getdt(sql);
            if (dt1.Rows.Count > 0)
            {
                string v1 = dt1.Rows[0][field].ToString();
                for (int j = v1.Length - 1; j >= 0; j--)
                {
                    if (v1[j] != '-')
                    {
                        v2 = v1[j] + v2;
                    }
                    else
                    {
                        break;

                    }
                }
            }
            return v2;
        }
        #endregion
        #region DelImagesFile
        public void DelImagesFile(string path, string sql, string field)
        {
            DataTable dt1 = this.getdt(sql);
            if (dt1.Rows.Count > 0)
            {
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    string v1 = dt1.Rows[i][field].ToString();
                    string v2 = "";
                    for (int j = v1.Length - 1; j >= 0; j--)
                    {
                        if (v1[j] != '-')
                        {
                            v2 = v1[j] + v2;
                        }
                        else
                        {
                            break;

                        }
                    }
                    string path2 = path + v2;
                    string path3 = path + "50x50-" + v2;
                    string path4 = path + "150x150-" + v2;
                    if (File.Exists(path2))
                    {
                        File.Delete(path2);
                    }
                    if (File.Exists(path3))
                    {
                        File.Delete(path3);
                    }
                    if (File.Exists(path4))
                    {
                        File.Delete(path4);
                    }
                }
            }
        }
        #endregion

        #region GetStorageCOID
        public DataTable GetStorageCOID(string v1)
        {
            DataTable dtk = new DataTable();
            dtk.Columns.Add("WAREID", typeof(string));
            dtk.Columns.Add("COID", typeof(string));
            dtk.Columns.Add("COLOR", typeof(string));
            dtk.Columns.Add("COLORIMAS", typeof(string));

            string sql = @"
SELECT A.WAREID,A.COID,B.COLOR,SUM(A.GECOUNT) ,SUM(A.MRCOUNT),
SUM(A.GECOUNT)-SUM(A.MRCOUNT)  FROM TB_SIZEMANAGE A
LEFT JOIN TB_COLOR B ON A.COID=B.COID
LEFT JOIN TB_SIZE C ON A.SIID=C.SIID
WHERE A.WAREID='" + v1 + "' GROUP BY A.WAREID,A.COID,B.COLOR HAVING SUM(A.GECOUNT)-SUM(A.MRCOUNT)>0 ORDER BY A.WAREID,A.COID,B.COLOR";

            string sql2 = @"SELECT A.WAREID,A.COID,B.COLOR,A.COLORIMAS FROM TB_SELECTCOLOR A
LEFT JOIN TB_COLOR B ON A.COID=B.COID
WHERE A.WAREID='" + v1 + "'";
            DataTable dt = this.getdt(sql);
            DataTable dt2 = this.getdt(sql2);
            if (dt.Rows.Count > 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {

                    DataRow dr = dtk.NewRow();
                    dr["Wareid"] = dt.Rows[i][0].ToString();
                    dr["COID"] = dt.Rows[i][1].ToString();
                    dr["COLOR"] = dt.Rows[i][2].ToString();
                    dtk.Rows.Add(dr);
                }
                for (int i1 = 0; i1 < dtk.Rows.Count; i1++)
                {

                    if (dt2.Rows.Count > 0)
                    {
                        for (j = 0; j < dt2.Rows.Count; j++)
                        {
                            if (dtk.Rows[i1]["wareid"].ToString() == dt2.Rows[j][0].ToString() &&
                                dtk.Rows[i1]["COID"].ToString() == dt2.Rows[j][1].ToString())
                            {

                                dtk.Rows[i1]["COLORIMAS"] = dt2.Rows[j][3].ToString();
                                break;
                            }


                        }

                    }

                }
            }
            return dtk;
        }
        #endregion



        #region addwater_nm
        /**/
        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        protected void AddWater(string Path, string Path_sy)
        {
            string addText = "1";
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 60);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Green);

            g.DrawString(addText, f, b, 35, 35);
            g.Dispose();

            image.Save(Path_sy);
            image.Dispose();
        }
        #endregion

        #region addwaterpic_nm
        /**/
        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        protected void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(Path_sypf);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(copyImage, new System.Drawing.Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }
        #endregion

        #region makethumbnail_nm
        /**/
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        /// 

        public void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        #endregion
        public void Show(string MessageInfo)
        {
            HttpContext.Current.Response.Write("<script language=javascript>alert('" + MessageInfo + "')</script>");
        }


        public void ShowP(string values, string PageURL)
        {
            HttpContext.Current.Response.Write("<script>alert('" + values + "');window.location.href='" + PageURL + "'</script>");
            HttpContext.Current.Response.End();
        }
        public bool juageOne(string sql)
        {
            bool b = false ;
            DataTable dt = this.getdt(sql);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count == 1)
                {
                    b =true ;
                }

            }
            return b;


        }
   
        #region csharpExcelPrint
        private  void csharpExcelPrint(string path)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = path;
            p.StartInfo.Verb = "print";
            p.Start();
        }
        #endregion
        public  bool JuageSourceStatus(string ORID)
        {
           
            bool b1 = false;
            string v11 = this.getOnlyString(@"SELECT A.SOURCESTATUS FROM PURCHASE_MST A LEFT JOIN ORDER_MST B 
            ON A.PUID=B.PUID WHERE B.ORID='" +ORID + "'");
            if (!string.IsNullOrEmpty(v11))
            {

                b1 = true;

            }
            return b1;

        }
        public byte[] GetMD5(string Password)
        {
            byte[] Encrypt=HashAlgorithm .Create ().ComputeHash (Encoding.Unicode .GetBytes (Password ));
            return Encrypt ;
        }
        #region JuageOrderPurchaseStatus
        public bool JuageOrderOrPurchaseStatus(string ORIDorPUID, int OrderOrPurchase)
        {
            bool b = true;
            DataTable dt = new DataTable();
            if (OrderOrPurchase == 0)
            {
                dt= this.getdt("SELECT * FROM ORDER_DET WHERE ORID='" + ORIDorPUID + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ORDERSTATUS_DET"].ToString() != "CLOSE")
                    {
                        b = false;
                        break;

                    }
                }
            
            }
            else
            {
                dt = this.getdt("SELECT * FROM PURCHASE_DET WHERE PUID='" + ORIDorPUID + "'");
                foreach (DataRow dr in dt.Rows)
                {
                  
                    if (dr["PURCHASESTATUS_DET"].ToString() != "CLOSE")
                    {
                        b = false;
                        break;

                    }
        
                }
                
            }
       
            return b;

        }
        #endregion

        #region JuageIfAllowDelete
        public bool JuageIfAllowDeleteEMID(string EMID)
        {
            bool b = false;
            if (this.exists("SELECT * FROM COMPANYINFO_MST WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在公司信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM COMPANYINFO_DET WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在公司信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM CUSTOMERINFO_MST WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在客户信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM CUSTOMERINFO_DET WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在客户信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM GODE WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在入库信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM MATERE WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在销货信息中使用了，不允许删除！";

            }

            else if (this.exists("SELECT * FROM ORDER_MST WHERE MAKERID='" + EMID + "' OR SALEID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在订单信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM PURCHASE_MST WHERE MAKERID='" + EMID + "' OR PURID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在采购信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM PURCHASEGODE_MST WHERE MAKERID='" + EMID + "' OR GODERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在入库信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM PURCHASEUNITPRICE WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在采购核价信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM RIGHTLIST WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在权限信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM SELLTABLE_MST WHERE MAKERID='" + EMID + "' OR SELLERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在销货信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM SELLUNITPRICE WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在销售核价信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM STORAGEINFO WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在仓库信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM SUPPLIERINFO_MST WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在供应商信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM SUPPLIERINFO_DET WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在供应商信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM USERINFO WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在用户信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM WAREFILE WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在品号相关文件信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM WAREINFO WHERE MAKERID='" + EMID + "'"))
            {
                b = true;
                ErrowInfo = "该工号已经在品号信息中使用了，不允许删除！";

            }


            return b;
        }
        #endregion

        #region JuageIfAllowDeleteWareID
        public bool JuageIfAllowDeleteWareID(string WareID)
        {
            bool b = false;
           if (this.exists("SELECT * FROM ORDER_DET WHERE WareID='" + WareID + "' "))
            {
                b = true;
                ErrowInfo = "该品号已经在订单信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM PURCHASE_DET WHERE WareID='" + WareID + "'"))
            {
                b = true;
                ErrowInfo = "该品号已经在采购信息中使用了，不允许删除！";

            }
            else if (this.exists("SELECT * FROM SELLUNITPRICE WHERE WareID='" + WareID + "'"))
            {
                b = true;
                ErrowInfo = "该品号已经在销售核价信息中使用了，不允许删除！";

            }
           else if (this.exists("SELECT * FROM PURCHASEUNITPRICE WHERE WareID='" + WareID  + "'"))
           {
               b = true;
               ErrowInfo = "该品号已经在采购核价信息中使用了，不允许删除！";

           }
            return b;
        }
        #endregion


       #region  GET_IFExecutionSUCCESS_HINT_INFO
       public string GET_IFExecutionSUCCESS_HINT_INFO(bool SET_IFExecutionSUCCESS)
       {
           string v = "";
           if (SET_IFExecutionSUCCESS == true)
           {

               v = "已保存成功!";
           }
           return v;
       }
       #endregion
       #region GET_DT_TO_DV_TO_DT
       public DataTable GET_DT_TO_DV_TO_DT(DataTable dt, string Sort, string RowFilter)
       {
           DataView dv = new DataView(dt);
           dv.RowFilter = RowFilter;
           dv.Sort = Sort;
           dt = dv.ToTable();
           return dt;
       }
       #endregion
       #region toexcel
       public void dgvtoExcel(DataGridView dataGridView1, string str1)
       {

           SaveFileDialog sfdg = new SaveFileDialog();
           sfdg.DefaultExt = "xls";
           sfdg.Filter = "Excel(*.xls)|*.xls";
           //sfdg.RestoreDirectory = true;
           sfdg.FileName = str1;
           //sfdg.CreatePrompt = true;
           sfdg.Title = "導出到EXCEL";
           int n, w;
           n = dataGridView1.RowCount;
           w = dataGridView1.ColumnCount;


           if (sfdg.ShowDialog() == DialogResult.OK)
           {
               try
               {
                   Excel.ApplicationClass excel = new Excel.ApplicationClass();
                   excel.Application.Workbooks.Add(true);

                   for (int j = 0; j < dataGridView1.ColumnCount; j++)
                   {

                       excel.Cells[1, j + 1] = dataGridView1.Columns[j].HeaderText;
                   }
                   for (int i = 0; i < dataGridView1.RowCount; i++)
                   {
                       for (int x = 0; x < dataGridView1.ColumnCount; x++)
                       {
                           if (dataGridView1[x, i].Value != null)
                           {
                               if (dataGridView1[x, i].ValueType == typeof(string))
                               {
                                   excel.Cells[i + 2, x + 1] = "'" + dataGridView1[x, i].Value.ToString();
                               }
                               else
                               {
                                   excel.Cells[i + 2, x + 1] = dataGridView1[x, i].Value.ToString();
                               }
                           }
                       }
                   }
                   excel.get_Range(excel.Cells[1, 1], excel.Cells[1, w]).HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;

                   //excel.get_Range(excel.Cells[2, 3], excel.Cells[n, 3]).HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlRight;
                   excel.get_Range(excel.Cells[1, 1], excel.Cells[n + 1, w]).Borders.LineStyle = 1;
                   //excel.get_Range(excel.Cells[1, 1], excel.Cells[n, w]).Select();
                   excel.get_Range(excel.Cells[1, 1], excel.Cells[n + 1, w]).Columns.AutoFit();
                   excel.Visible = false;
                   excel.ExtendList = false;
                   excel.DisplayAlerts = false;
                   excel.AlertBeforeOverwriting = false;
                   excel.ActiveWorkbook.SaveAs(sfdg.FileName, Excel.XlFileFormat.xlExcel7, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                   excel.Quit();
                   MessageBox.Show("成功导出！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   excel = null;
                   GC.Collect();
               }

               catch (Exception ex)
               {
                   MessageBox.Show(ex.Message);
               }
               finally
               {
                   GC.Collect();
               }

           }
       }

       #endregion
       #region SQlcommandE
       public void SQlcommandE(string sql)
       {
           string year = DateTime.Now.ToString("yy");
           string month = DateTime.Now.ToString("MM");
           string day = DateTime.Now.ToString("dd");
           string varDate = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss").Replace ("/","-");
           string varMakerID = this.getOnlyString("SELECT EMID FROM USERINFO WHERE USID='" + USID + "'");
           SqlConnection sqlcon = this.getcon();
           SqlCommand sqlcom = new SqlCommand(sql, sqlcon);
           sqlcom.Parameters.Add("@AUID", SqlDbType.VarChar, 20).Value = GEID;
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
           string v1 = this.getOnlyString("SELECT " + COLUMNNAME + " FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + IDVALUE + "'");
           string v2 = this.getOnlyString("SELECT " + COLUMNID_o + " FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + IDVALUE + "'");

           //string varMakerID;
           if (!this.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + IDVALUE + "'"))
           {
               if (this.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNNAME + "='" + NAMEVALUE + "'"))
               {
                   ErrowInfo = INFONAME + "已经存在于系统！";
                   IFExecution_SUCCESS = false;
               }
               else if (this.exists("SELECT " + COLUMNID_o + " FROM " + TABLENAME + " WHERE " + COLUMNID_o + "='" + IDVALUE_o + "'"))
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

               if (this.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNNAME + "='" + NAMEVALUE + "'"))
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

               if (this.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNID_o + "='" + IDVALUE_o + "'"))
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

               if (this.exists("SELECT " + COLUMNID + " FROM " + TABLENAME + " WHERE " + COLUMNNAME + "='" + NAMEVALUE + "'"))
               {
                   ErrowInfo = INFONAME + "已经存在于系统！";
                   IFExecution_SUCCESS = false;

               }
               else if (this.exists("SELECT " + COLUMNID_o + " FROM " + TABLENAME + " WHERE " + COLUMNID_o + "='" + IDVALUE_o + "'"))
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
           Byte[] B = this.GetMD5(PWD);
           SqlConnection sqlcon = this.getcon();
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
