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
    public class   PrintOfferBill
    {
        basec bc = new basec();
        string sqlo = @"
 SELECT 
 A.WareID AS ID,
 A.WName AS 品名,
 A.CWareID  AS 客户料号,
 A.SET_LEN AS SET长,
 A.SET_WIDTH AS SET宽,
 A.SET_COMPOSING AS SET排版数,
 A.PCS_COUNT AS PCS内孔数,
 A.VCUT_SET AS VCUT_SET刀数,
 A.PCS_LEN AS PCS长,
 A.PCS_WIDTH AS PCS宽 ,
 A.PANEL_NEED AS 板材厂商要求,
 B.ASSIGN_STACKUP AS 指定叠构,
 B.BASEVALUE AS 指定叠构基准,
 C.BGA_DESIGN AS BGA设计,
 C.BASEVALUE AS BGA设计基准,
 D.BIGANDSMALL_PANEL AS 超大小板,
 D.BASEVALUE AS 超大小板基准, 
 E.Character_Color AS 文字颜色,
 E.BASEVALUE AS 文字颜色基准 , 
 F.Circuit_Spec AS 内外线路规格 ,
 F.BASEVALUE AS 内外线路规格基准,
 G.COPPER_NEED AS 孔铜要求,
 G.BASEVALUE AS 孔铜要求基准,
 H.Core_Copper AS 内层成品铜厚,
 H.BASEVALUE AS 内层成品铜厚基准 ,
 I.HYPOTENUSE_ANGLE AS 金手指斜边角度 ,
 I.BASEVALUE AS 金手指斜边角度基准,
 J.IF_HYPOTENUSE 金手指斜边,
 J.BASEVALUE AS 金手指斜边基准,
 K.IMPEDANCE AS 阻抗,
 K.BASEVALUE AS 阻抗基准,
 L.MINIMUM_HOLE  AS 最小孔,
 L.BASEVALUE AS 最小孔基准,
 M.MOLDING_STYLE AS 成型方式,
 M.BASEVALUE AS 成型方式基准,
 N.MOLDING_TOLERANCE AS 成型公差,
 N.BASEVALUE AS 成型公差基准,
 O.Panel AS 板材,
 O.BASEVALUE AS 板材基准,
 P.PLANK_THICKNESS AS 板厚,
 P.BASEVALUE AS 板厚基准,
 Q.PLANK_TOLERANCE AS 板厚公差,
 Q.BASEVALUE AS 板厚公差基准,
 R.PLANK_TYPE AS 板子类型,
 R.BASEVALUE AS 板子类型基准 ,
 S.Solder_Mask AS 防焊颜色,
 S.BASEVALUE AS 防焊颜色基准,
 T.SPEC AS 层数,
 T.BASEVALUE AS 层数基准 ,
 T.PROJECT_COST AS 工程费,
 U.Surface_Thickness AS 表面处理厚度,
 U.BASEVALUE AS 表面处理厚度基准,
 V.Surface_Treatment AS 表面处理 ,
 V.BASEVALUE AS 表面处理基准,
 W.THICKNESS_COPPER  AS 厚铜板,
 W.BASEVALUE AS 厚铜板基准,
 X.VCUT_ANGLE AS VCUT角度,
 X.BASEVALUE AS VCUT角度基准,
 Y.VCUT_DISABLED AS VCUT残厚,
 Y.BASEVALUE AS VCUT残厚基准,  
 Z.Out_Copper AS 外层成品铜厚,
 Z.BASEVALUE AS 外层成品铜厚基准,
A1.CUSTOMER_COEFFICIENT AS 客户系数,
A1.SAMPLE_COEFFICIENT AS SAMPLE系数,
A1.SMALLQUANTITY_COEFFICIENT AS 小量系数,  
A1.QUANTITY_COEFFICIENT AS 量产系数,  
A1.MOQ_AREA AS MOQ面积,
A1.SAMPLE_AREA AS SAMPLE面积,
A1.CName AS 客户名称,
A2.Contact AS 联系人,
A2.Phone AS 电话,
A2.Fax AS 传真,
A3.QUANTITY_COUNT AS 量产数量,
A3.QUANTITY_UNITPRICE AS 量产单价,
A3.SAMPLE_COUNT AS SAMPLE数量,
A3.SAMPLE_UNITPRICE AS SAMPLE单价,
A3.SMALLQUANTITY_UNITPRICE AS 小量单价,
A3.OFID AS 报价单号,
A4.OLDFILENAME AS 文件名,
(SELECT ENAME FROM EMPLOYEEINFO WHERE EMID=A3.MakerID )  AS 制单人,
A.REMARK AS 备注,  
SUBSTRING(A3.DATE,1,10) AS 制单日期,
A5.PHONE AS 个人电话
 FROM  WareInfo A 
 LEFT JOIN ASSIGN_STACKUP B ON A.ASSIGN_STACKUP =B.ASSIGN_STACKUP 
 LEFT JOIN BGA_DESIGN C ON A.BGA_DESIGN =C.BGA_DESIGN 
 LEFT JOIN BIGANDSMALL_PANEL D ON A.BIGANDSMALL_PANEL=D.BIGANDSMALL_PANEL 
 LEFT JOIN Character_Color E ON A.Character_Color =E.Character_Color 
 LEFT JOIN Circuit_Spec F ON A.Circuit_Spec =F.Circuit_Spec 
 LEFT JOIN COPPER_NEED G ON A.COPPER_NEED =G.COPPER_NEED 
 LEFT JOIN Core_Copper H ON A.Core_Copper =H.Core_Copper 
 LEFT JOIN HYPOTENUSE_ANGLE I ON A.HYPOTENUSE_ANGLE =I.HYPOTENUSE_ANGLE 
 LEFT JOIN IF_HYPOTENUSE J ON A.IF_HYPOTENUSE =J.IF_HYPOTENUSE 
 LEFT JOIN IMPEDANCE K ON A.IMPEDANCE =K.IMPEDANCE 
 LEFT JOIN MINIMUM_HOLE L ON A.MINIMUM_HOLE =L.MINIMUM_HOLE 
 LEFT JOIN MOLDING_STYLE M ON A.MOLDING_STYLE =M.MOLDING_STYLE 
 LEFT JOIN MOLDING_TOLERANCE N ON A.MOLDING_TOLERANCE =N.MOLDING_TOLERANCE
 LEFT JOIN Panel O ON A.Panel=O.Panel 
 LEFT JOIN PLANK_THICKNESS P ON A.PLANK_THICKNESS =P.PLANK_THICKNESS 
 LEFT JOIN PLANK_TOLERANCE Q ON A.PLANK_TOLERANCE =Q.PLANK_TOLERANCE 
 LEFT JOIN PLANK_TYPE R ON A.PLANK_TYPE=R.PLANK_TYPE 
 LEFT JOIN Solder_Mask S ON A.Solder_Mask =S.Solder_Mask 
 LEFT JOIN SPEC T ON A.Spec =T.SPEC 
 LEFT JOIN Surface_Thickness U ON A.Surface_Thickness =U.Surface_Thickness 
 LEFT JOIN Surface_Treatment V ON A.Surface_Treatment =V.Surface_Treatment 
 LEFT JOIN THICKNESS_COPPER W ON A.THICKNESS_COPPER =W.THICKNESS_COPPER 
 LEFT JOIN VCUT_ANGLE X ON A.VCUT_ANGLE =X.VCUT_ANGLE 
 LEFT JOIN VCUT_DISABLED Y ON A.VCUT_DISABLED =Y.VCUT_DISABLED 
 LEFT JOIN Out_Copper Z ON A.Out_Copper =Z.Out_Copper
 LEFT JOIN CUSTOMERINFO_MST A1 ON A.CUID=A1.CUID
 LEFT JOIN CustomerInfo_DET A2 ON A1.CUKEY =A2.CUKEY 
 LEFT JOIN OFFER A3 ON A.WareID=A3.WAREID
 LEFT JOIN WareFile A4 ON A.WareID =A4.WareID 
 LEFT JOIN EMPLOYEEINFO A5 ON A3.MAKERID=A5.EMID
";
        //string sqlt = @"    ORDER BY A.CAID,D.UCID ASC";

        public PrintOfferBill()
        {

        }
        #region askTOTALt
        public DataTable ask(string wareid)
        {
            string sql1 = sqlo;
            DataTable dtt = bc.getdt(sql1 + " WHERE A.WAREID='" + wareid  + "' ORDER BY A3.DATE DESC");
            return dtt;
        }
        #endregion
        #region gettable  /*crystalprint 1/2*/
        public DataTable gettable()
        {
            DataTable dt4 = new DataTable();
            dt4.Columns.Add("品名", typeof(string));
            dt4.Columns.Add("客户料号", typeof(string));
            dt4.Columns.Add("SET长", typeof(string));
            dt4.Columns.Add("SET宽", typeof(string));
            dt4.Columns.Add("SET排版数", typeof(string));
            dt4.Columns.Add("PCS内孔数", typeof(string));
            dt4.Columns.Add("VCUT_SET刀数", typeof(string));
            dt4.Columns.Add("PCS长", typeof(string));
            dt4.Columns.Add("PCS宽", typeof(string));
            dt4.Columns.Add("指定叠构", typeof(string));
            dt4.Columns.Add("指定叠构基准", typeof(string));
            dt4.Columns.Add("BGA设计", typeof(string));
            dt4.Columns.Add("BGA设计基准", typeof(string));
            dt4.Columns.Add("超大小板", typeof(string));
            dt4.Columns.Add("超大小板基准", typeof(string));
            dt4.Columns.Add("文字颜色", typeof(string));
            dt4.Columns.Add("文字颜色基准", typeof(string));
            dt4.Columns.Add("内外线路规格", typeof(string));
            dt4.Columns.Add("内外线路规格基准", typeof(string));
            dt4.Columns.Add("孔铜要求", typeof(string));
            dt4.Columns.Add("孔铜要求基准", typeof(string));
            dt4.Columns.Add("内层成品铜厚", typeof(string));
            dt4.Columns.Add("内层成品铜厚基准", typeof(string));
            dt4.Columns.Add("金手指斜边角度", typeof(string));
            dt4.Columns.Add("金手指斜边角度基准", typeof(string));
            dt4.Columns.Add("金手指斜边", typeof(string));
            dt4.Columns.Add("金手指斜边基准", typeof(string));
            dt4.Columns.Add("阻抗", typeof(string));
            dt4.Columns.Add("阻抗基准", typeof(string));
            dt4.Columns.Add("最小孔", typeof(string));
            dt4.Columns.Add("最小孔基准", typeof(string));
            dt4.Columns.Add("成型方式", typeof(string));
            dt4.Columns.Add("成型方式基准", typeof(string));
            dt4.Columns.Add("成型公差", typeof(string));
            dt4.Columns.Add("成型公差基准", typeof(string));
            dt4.Columns.Add("板材", typeof(string));
            dt4.Columns.Add("板材基准", typeof(string));
            dt4.Columns.Add("板厚", typeof(string));
            dt4.Columns.Add("板厚基准", typeof(string));
            dt4.Columns.Add("板厚公差", typeof(string));
            dt4.Columns.Add("板厚公差基准", typeof(string));
            dt4.Columns.Add("板子类型", typeof(string));
            dt4.Columns.Add("板子类型基准", typeof(string));
            dt4.Columns.Add("防焊颜色", typeof(string));
            dt4.Columns.Add("防焊颜色基准", typeof(string));
            dt4.Columns.Add("层数", typeof(string));
            dt4.Columns.Add("层数基准", typeof(string));
            dt4.Columns.Add("工程费", typeof(string));
            dt4.Columns.Add("表面处理厚度", typeof(string));
            dt4.Columns.Add("表面处理厚度基准", typeof(string));
            dt4.Columns.Add("表面处理", typeof(string));
            dt4.Columns.Add("表面处理基准", typeof(string));
            dt4.Columns.Add("厚铜板", typeof(string));
            dt4.Columns.Add("厚铜板基准", typeof(string));
            dt4.Columns.Add("VCUT角度", typeof(string));
            dt4.Columns.Add("VCUT角度基准", typeof(string));
            dt4.Columns.Add("VCUT残厚", typeof(string));
            dt4.Columns.Add("VCUT残厚基准", typeof(string));
            dt4.Columns.Add("外层成品铜厚", typeof(string));
            dt4.Columns.Add("外层成品铜厚基准", typeof(string));
            dt4.Columns.Add("客户系数", typeof(string));
            dt4.Columns.Add("SAMPLE系数", typeof(string));
            dt4.Columns.Add("小量系数", typeof(string));
            dt4.Columns.Add("量产系数", typeof(string));
            dt4.Columns.Add("MOQ面积", typeof(string));
            dt4.Columns.Add("SAMPLE面积", typeof(string));
            dt4.Columns.Add("客户名称", typeof(string));
            dt4.Columns.Add("联系人", typeof(string));
            dt4.Columns.Add("电话", typeof(string));
            dt4.Columns.Add("传真", typeof(string));
            dt4.Columns.Add("量产数量", typeof(string));
            dt4.Columns.Add("量产单价", typeof(string));
            dt4.Columns.Add("SAMPLE数量", typeof(string));
            dt4.Columns.Add("SAMPLE单价", typeof(string));
            dt4.Columns.Add("小量单价", typeof(string));
            dt4.Columns.Add("报价单号", typeof(string));
            dt4.Columns.Add("文件名", typeof(string));
            dt4.Columns.Add("制单人", typeof(string));
            dt4.Columns.Add("制单日期", typeof(string));
            dt4.Columns.Add("公司名称", typeof(string));
            dt4.Columns.Add("个人电话", typeof(string));
            dt4.Columns.Add("公司传真", typeof(string));
            dt4.Columns.Add("板材厂商要求", typeof(string));
            return dt4;
        }
        #endregion
        #region ask
        public  DataTable askt(string billid)
        {
            string sql1 = sqlo;
            DataTable dtt = this.gettable();
            DataTable dtto= bc.getdt(sql1+" WHERE A3.OFID='"+billid +"' ORDER BY A3.DATE DESC");
            if (dtto.Rows.Count > 0)
            {

            foreach (DataRow dr in dtto.Rows)
            {
                DataRow dr1 = dtt.NewRow();
                dr1["品名"] = dr["品名"].ToString();
                dr1["客户料号"] = dr["客户料号"].ToString();
                dr1["SET长"] = dr["SET长"].ToString();
                dr1["SET宽"] = dr["SET宽"].ToString();
                dr1["SET排版数"] = dr["SET排版数"].ToString();
                dr1["PCS内孔数"] = dr["PCS内孔数"].ToString();
                dr1["VCUT_SET刀数"] = dr["VCUT_SET刀数"].ToString();
                dr1["PCS长"] = dr["PCS长"].ToString();
                dr1["PCS宽"] = dr["PCS宽"].ToString();
                dr1["指定叠构"] = dr["指定叠构"].ToString();
                dr1["指定叠构基准"] = dr["指定叠构基准"].ToString();
                dr1["BGA设计"] = dr["BGA设计"].ToString();
                dr1["BGA设计基准"] = dr["BGA设计基准"].ToString();
                dr1["超大小板"] = dr["超大小板"].ToString();
                dr1["超大小板基准"] = dr["超大小板基准"].ToString();
                dr1["文字颜色"] = dr["文字颜色"].ToString();
                dr1["文字颜色基准"] = dr["文字颜色基准"].ToString();
                dr1["内外线路规格"] = dr["内外线路规格"].ToString();
                dr1["内外线路规格基准"] = dr["内外线路规格基准"].ToString();
                dr1["孔铜要求"] = dr["孔铜要求"].ToString();
                dr1["孔铜要求基准"] = dr["孔铜要求基准"].ToString();
                dr1["内层成品铜厚"] = dr["内层成品铜厚"].ToString();
                dr1["内层成品铜厚基准"] = dr["内层成品铜厚基准"].ToString();
                dr1["金手指斜边角度"] = dr["金手指斜边角度"].ToString();
                dr1["金手指斜边角度基准"] = dr["金手指斜边角度基准"].ToString();
                dr1["金手指斜边"] = dr["金手指斜边"].ToString();
                dr1["金手指斜边基准"] = dr["金手指斜边基准"].ToString();
                dr1["阻抗"] = dr["阻抗"].ToString();
                dr1["阻抗基准"] = dr["阻抗基准"].ToString();
                dr1["最小孔"] = dr["最小孔"].ToString();
                dr1["最小孔基准"] = dr["最小孔基准"].ToString();
                dr1["成型方式"] = dr["成型方式"].ToString();
                dr1["成型方式基准"] = dr["成型方式基准"].ToString();
                dr1["成型公差"] = dr["成型公差"].ToString();
                dr1["成型公差基准"] = dr["成型公差基准"].ToString();
                dr1["板材"] = dr["板材"].ToString();
                dr1["板材基准"] = dr["板材基准"].ToString();
                dr1["板厚"] = dr["板厚"].ToString();
                dr1["板厚基准"] = dr["板厚基准"].ToString();
                dr1["板厚公差"] = dr["板厚公差"].ToString();
                dr1["板厚公差基准"] = dr["板厚公差基准"].ToString();
                dr1["板子类型"] = dr["板子类型"].ToString();
                dr1["板子类型基准"] = dr["板子类型基准"].ToString();
                dr1["防焊颜色"] = dr["防焊颜色"].ToString();
                dr1["防焊颜色基准"] = dr["防焊颜色基准"].ToString();
                dr1["层数"] = dr["层数"].ToString();
                dr1["层数基准"] = dr["层数基准"].ToString();
                dr1["工程费"] = dr["工程费"].ToString();
                dr1["表面处理厚度"] = dr["表面处理厚度"].ToString();
                dr1["表面处理厚度基准"] = dr["表面处理厚度基准"].ToString();
                dr1["表面处理"] = dr["表面处理"].ToString();
                dr1["表面处理基准"] = dr["表面处理基准"].ToString();
                dr1["厚铜板"] = dr["厚铜板"].ToString();
                dr1["厚铜板基准"] = dr["厚铜板基准"].ToString();
                dr1["VCUT角度"] = dr["VCUT角度"].ToString();
                dr1["VCUT角度基准"] = dr["VCUT角度基准"].ToString();
                dr1["VCUT残厚"] = dr["VCUT残厚"].ToString();
                dr1["VCUT残厚基准"] = dr["VCUT残厚基准"].ToString();
                dr1["外层成品铜厚"] = dr["外层成品铜厚"].ToString();
                dr1["外层成品铜厚基准"] = dr["外层成品铜厚基准"].ToString();
                dr1["客户系数"] = dr["客户系数"].ToString();
                dr1["SAMPLE系数"] = dr["SAMPLE系数"].ToString();
                dr1["小量系数"] = dr["小量系数"].ToString();
                dr1["量产系数"] = dr["量产系数"].ToString();
                dr1["MOQ面积"] = dr["MOQ面积"].ToString();
                dr1["SAMPLE面积"] = dr["SAMPLE面积"].ToString();
                dr1["客户名称"] = dr["客户名称"].ToString();
                dr1["联系人"] = dr["联系人"].ToString();
                dr1["电话"] = dr["电话"].ToString();
                dr1["传真"] = dr["传真"].ToString();
                dr1["量产数量"] = dr["量产数量"].ToString();
                dr1["量产单价"] = dr["量产单价"].ToString();
                dr1["SAMPLE数量"] = dr["SAMPLE数量"].ToString();
                dr1["SAMPLE单价"] = dr["SAMPLE单价"].ToString();
                dr1["小量单价"] = dr["小量单价"].ToString();
                dr1["报价单号"] = dr["报价单号"].ToString();
                dr1["文件名"] = dr["文件名"].ToString();
                dr1["制单人"] = dr["制单人"].ToString();
                dr1["制单日期"] = dr["制单日期"].ToString();
                dr1["板材厂商要求"] = dr["板材厂商要求"].ToString();
                dr1["个人电话"] = dr["个人电话"].ToString();
                dtt.Rows.Add(dr1);

            }
            DataTable dt1 = bc.getdt(@"SELECT A.CONAME AS 公司名称,B.CONTACT AS CONTACT,B.PHONE AS 公司电话,B.FAX AS 公司传真 FROM COMPANYINFO_MST A
LEFT JOIN COMPANYINFO_DET B ON A.COKEY=B.COKEY");
            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr2 in dtt.Rows)
                {
                    dr2["公司名称"] = dt1.Rows[0]["公司名称"].ToString();
                    //dr2["公司电话"] = dt1.Rows[0]["公司电话"].ToString();
                    dr2["公司传真"] = dt1.Rows[0]["公司传真"].ToString();

                }


            }



            }
            return dtt;
        }
        #endregion

    }
}
