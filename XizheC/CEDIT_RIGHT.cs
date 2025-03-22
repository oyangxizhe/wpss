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
using System.Collections.Generic;

namespace XizheC
{
    public class CEDIT_RIGHT
    {
        basec bc = new basec();
        #region NATURE
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
        private string _EMID;
        public string EMID
        {
            set { _EMID = value; }
            get { return _EMID; }

        }
        private string _USID;
        public string USID
        {
            set { _USID = value; }
            get { return _USID; }

        }
        private string _URL;
        public string URL
        {
            set { _URL = value; }
            get { return _URL; }

        }
        private string _IMAGE_URL;
        public string IMAGE_URL
        {
            set { _IMAGE_URL = value; }
            get { return _IMAGE_URL; }

        }
        private string _NODEID;
        public string NODEID
        {
            set { _NODEID = value; }
            get { return _NODEID; }

        }
        private string _PARENT_NODEID;
        public string PARENT_NODEID
        {
            set { _PARENT_NODEID = value; }
            get { return _PARENT_NODEID; }

        }
        private string _NODE_NAME;
        public string NODE_NAME
        {
            set { _NODE_NAME = value; }
            get { return _NODE_NAME; }

        }
        private string _OPERATE;
        public string OPERATE
        {
            set { _OPERATE = value; }
            get { return _OPERATE; }

        }
        private string _SEARCH;
        public string SEARCH
        {
            set { _SEARCH = value; }
            get { return _SEARCH; }

        }
        private string _ADD_NEW;
        public string ADD_NEW
        {
            set { _ADD_NEW = value; }
            get { return _ADD_NEW; }

        }
        private string _EDIT;
        public string EDIT
        {
            set { _EDIT = value; }
            get { return _EDIT; }

        }
        private string _DEL;
        public string DEL
        {
            set { _DEL = value; }
            get { return _DEL; }

        }
        private string _MANAGE;
        public string MANAGE
        {
            set { _MANAGE = value; }
            get { return _MANAGE; }

        }
        private string _FINANCIAL;
        public string FINANCIAL
        {
            set { _FINANCIAL = value; }
            get { return _FINANCIAL; }

        }
        private string _GENERAL_MANAGE;
        public string GENERAL_MANAGE
        {
            set { _GENERAL_MANAGE = value; }
            get { return _GENERAL_MANAGE; }

        }
        #endregion
        string setsql = @"
SELECT
A.USID AS 用户编号,
A.UNAME AS 用户名,
B.ENAME AS 姓名,
C.NODE_NAME AS 作业名称,
CASE WHEN C.OPERATE='Y'  THEN '有权限'
WHEN C.OPERATE='N'  THEN '无权限'
ElSE ''
END AS 操作权限,
CASE WHEN C.SEARCH='Y' AND (C.NODE_NAME='采购单' OR C.NODE_NAME='采购入库' OR C.NODE_NAME='退货单' OR C.NODE_NAME='客户订单' OR 
C.NODE_NAME='销货单' OR C.NODE_NAME='销退单') THEN '有权限'
WHEN C.SEARCH='N' AND (C.NODE_NAME='采购单' OR C.NODE_NAME='采购入库' OR C.NODE_NAME='退货单' OR C.NODE_NAME='客户订单' OR 
C.NODE_NAME='销货单' OR C.NODE_NAME='销退单') THEN '无权限'
ELSE ''
END AS 查询权限,
CASE WHEN C.ADD_NEW ='Y' AND (C.NODE_NAME='采购单' OR C.NODE_NAME='采购入库' OR C.NODE_NAME='退货单' OR C.NODE_NAME='客户订单' OR 
C.NODE_NAME='销货单' OR C.NODE_NAME='销退单') THEN '有权限'
WHEN C.ADD_NEW ='N' AND (C.NODE_NAME='采购单' OR C.NODE_NAME='采购入库' OR C.NODE_NAME='退货单' OR C.NODE_NAME='客户订单' OR 
C.NODE_NAME='销货单' OR C.NODE_NAME='销退单') THEN '无权限'
ELSE ''
END AS 新增权限,
CASE WHEN C.EDIT='Y' AND (C.NODE_NAME='采购单' OR C.NODE_NAME='采购入库' OR C.NODE_NAME='退货单' OR C.NODE_NAME='客户订单' OR 
C.NODE_NAME='销货单' OR C.NODE_NAME='销退单') THEN '有权限'
WHEN C.EDIT='N' AND (C.NODE_NAME='采购单' OR C.NODE_NAME='采购入库' OR C.NODE_NAME='退货单' OR C.NODE_NAME='客户订单' OR 
C.NODE_NAME='销货单' OR C.NODE_NAME='销退单') THEN '无权限'
ELSE ''
END AS 修改权限,
CASE WHEN C.DEL='Y' AND (C.NODE_NAME='采购单' OR C.NODE_NAME='采购入库' OR C.NODE_NAME='退货单' OR C.NODE_NAME='客户订单' OR 
C.NODE_NAME='销货单' OR C.NODE_NAME='销退单') THEN '有权限'
WHEN C.DEL='N' AND (C.NODE_NAME='采购单' OR C.NODE_NAME='采购入库' OR C.NODE_NAME='退货单' OR C.NODE_NAME='客户订单' OR 
C.NODE_NAME='销货单' OR C.NODE_NAME='销退单') THEN '无权限'
ELSE ''
END AS 删除权限,
CASE WHEN D.SCOPE='Y' THEN '所有用户'
WHEN D.SCOPE='GROUP' THEN '本组用户'
ELSE '当前用户'
END AS '授权范围',
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=C.MAKERID) AS 制单人,
C.DATE AS 制单日期 
FROM  
USERINFO  A 
LEFT JOIN EMPLOYEEINFO B ON A.EMID=B.EMID 
LEFT JOIN RIGHTLIST C ON A.USID=C.USID
LEFT JOIN SCOPE_OF_AUTHORIZATION D ON A.USID=D.USID 

";

        string setsqlo = @"
INSERT INTO 
RIGHTLIST
(
USID,
NODEID,
PARENT_NODEID,
NODE_NAME,
IMAGE_URL,
URL,
OPERATE,
SEARCH,
ADD_NEW,
EDIT,
DEL,
MANAGE,
FINANCIAL,
GENERAL_MANAGE,
MAKERID,
DATE
)
VALUES
(
@USID,
@NODEID,
@PARENT_NODEID,
@NODE_NAME,
@IMAGE_URL,
@URL,
@OPERATE,
@SEARCH,
@ADD_NEW,
@EDIT,
@DEL,
@MANAGE,
@FINANCIAL,
@GENERAL_MANAGE,
@MAKERID,
@DATE
)

";
        DataTable dt = new DataTable();
        public CEDIT_RIGHT()
        {
            sql = setsql; /*WAREINFO*/
            sqlo = setsqlo; /*ORDER*/
           
        }
        #region SQlcommandE
        public  void SQlcommandE()
        {
            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyy-MM-dd HH:mm:ss").Replace("/", "-");
            SqlConnection sqlcon = bc.getcon();
            SqlCommand sqlcom = new SqlCommand(setsqlo, sqlcon);
            sqlcom.Parameters.Add("@USID", SqlDbType.VarChar, 20).Value = USID;
            sqlcom.Parameters.Add("@NODEID", SqlDbType.VarChar, 20).Value = NODEID;
            sqlcom.Parameters.Add("@PARENT_NODEID", SqlDbType.VarChar, 20).Value = PARENT_NODEID;
            sqlcom.Parameters.Add("@NODE_NAME", SqlDbType.VarChar, 20).Value = NODE_NAME;
            sqlcom.Parameters.Add("@IMAGE_URL", SqlDbType.VarChar, 50).Value = IMAGE_URL;
            sqlcom.Parameters.Add("@URL", SqlDbType.VarChar, 50).Value = URL;
            sqlcom.Parameters.Add("@OPERATE", SqlDbType.VarChar, 20).Value = OPERATE;
            sqlcom.Parameters.Add("@SEARCH", SqlDbType.VarChar, 20).Value = SEARCH;
            sqlcom.Parameters.Add("@ADD_NEW", SqlDbType.VarChar, 20).Value = ADD_NEW;
            sqlcom.Parameters.Add("@EDIT", SqlDbType.VarChar, 20).Value = EDIT;
            sqlcom.Parameters.Add("@DEL", SqlDbType.VarChar, 20).Value = DEL;
            sqlcom.Parameters.Add("@MANAGE", SqlDbType.VarChar, 20).Value = MANAGE;
            sqlcom.Parameters.Add("@FINANCIAL", SqlDbType.VarChar, 20).Value = FINANCIAL;
            sqlcom.Parameters.Add("@GENERAL_MANAGE", SqlDbType.VarChar, 20).Value = GENERAL_MANAGE;
            sqlcom.Parameters.Add("@MAKERID", SqlDbType.VarChar, 20).Value = EMID;
            sqlcom.Parameters.Add("@DATE", SqlDbType.VarChar, 20).Value = varDate;
            sqlcon.Open();
            sqlcom.ExecuteNonQuery();
            sqlcon.Close();
        }
        #endregion

        public List<string> RETURN_TEMP()
        {

            string[] a = new string[] { "采购单", "采购入库", "退货单", "客户订单", "销货单", "销退单" };

            List<string> list1 = new List<string>();
            foreach (string v in a)
            {

                list1.Add(v);

            }

            return list1;
        }
        public bool JUAGE_IF_SAME(string TEST)
        {
            bool b = false;
            List<string> list1 = RETURN_TEMP();
            foreach (string a in list1)
            {

                if (a == TEST)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
        
    }
}
