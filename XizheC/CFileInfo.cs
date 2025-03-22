using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Collections;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web;


namespace XizheC
{
   
    public class CFileInfo
    {
        private string _FileName;
        public string FileName
        {
            set { _FileName = value; }
            get { return _FileName; }

        }
        private string _LastFileUpdateTime;
        public string LastFileUpdateTime
        {

            set { _LastFileUpdateTime = value; }
            get { return _LastFileUpdateTime; }


        }
        private string _FileNameAndPath;
        public string FileNameAndPath
        {
            set { _FileNameAndPath = value; }
            get { return _FileNameAndPath; }

        }
        private string _Path;
        public string Path
        {

            set { _Path = value; }
            get { return _Path; }


        }
        int i;
        private string _ErrowInfo;
        public string ErrowInfo
        {

            set { _ErrowInfo = value; }
            get { return _ErrowInfo; }

        }
        private int _MaxFileSize;
        public  int  MaxFileSize
        {
            set { _MaxFileSize = value; }
            get { return _MaxFileSize; }

        }
        public CFileInfo()
        {
            _MaxFileSize = 20971520;
        }
        public CFileInfo(int j)
        {
            if (j > 0)
            {
                _MaxFileSize = j;

            }
            else
            {

                _MaxFileSize = 20971520;
            }

   
        }
       
        basec bc = new basec();
        public List<CFileInfo> FindFile(string dir)
        {
            //在指定目录及子目录下查找文件,在listBox1中列出子目录及文件
            List<CFileInfo> list1 = new List<CFileInfo>();
            DirectoryInfo Dir = new DirectoryInfo(dir);
            try
            {
               
                foreach (DirectoryInfo d in Dir.GetDirectories())
                {  
                    //查找子目录  
                    FindFile(Dir + d.ToString() + "\\");
                    //listBox1.Items.Add(Dir + d.ToString() + "\\");
              

                }
                //listBox1中填加目录名}    
                foreach (FileInfo f in Dir.GetFiles("*.*"))
                {  //查找文件
                    CFileInfo cfileinfo = new CFileInfo();
                    cfileinfo.FileName =f.ToString();
                    cfileinfo.Path = Dir.ToString();
                    cfileinfo.FileNameAndPath = Dir + f.ToString();
                    cfileinfo.LastFileUpdateTime = File.GetLastWriteTime(Dir + f.ToString()).ToString();
                    list1.Add(cfileinfo);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return list1;
        }
        public  bool CExists(string clientPath, string serverfilename)
        {
            bool a1 = true;

            if (File.Exists(clientPath + serverfilename) == true)
            {
                
            }
            else
            {
                a1 = false;

            }
            return a1;

        }
        public  string GetTheLastUpdateTime(string Dir)
        {
            //获取客户端应用程序及服务器端升级程序的最近一次更新日期
            string LastUpdateTime = "";
            string AutoUpdaterFileName = Dir + @"\AutoUpdater.xml";
            if (!File.Exists(AutoUpdaterFileName))
                return LastUpdateTime;
            //打开xml文件  
            FileStream myFile = new FileStream(AutoUpdaterFileName, FileMode.Open);
            //xml文件阅读器  
            XmlTextReader xml = new XmlTextReader(myFile);
            while (xml.Read())
            {
                if (xml.Name == "UpdateTime")
                {  //获取升级文档的最后一次更新日期 
                    LastUpdateTime = xml.GetAttribute("Date");
                    break;
                }
            }
            xml.Close();
            myFile.Close();
            return LastUpdateTime;
        }


        public  DataSet importExcelToDataSet(string FilePath, string tablename)
        {
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + FilePath + ";Extended Properties='Excel 8.0;HDR=No;IMEX=1'";
            OleDbConnection conn = new OleDbConnection(strConn);

            OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [" + tablename + "] ", strConn);
            DataSet myDataSet = new DataSet();
            try
            {
                myCommand.Fill(myDataSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show("error," + ex.Message);
            }
            return myDataSet;
        }
         public void   OnloadFile(string WareID)
        {


            string year = DateTime.Now.ToString("yy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string varDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            Random ro = new Random();
            System.Web.UI.Page page = new Page();
            HttpServerUtility hsu = page.Server;
            string dirpath =hsu.MapPath("../File/");
            for (i = 0; i < files.Count; i++)
            {
                System.Web.HttpPostedFile myFile = files[i];

                if (myFile.ContentLength >_MaxFileSize)
                {
                  _ErrowInfo ="文件超过20M";
                    return;
                }

                string FileName = "";
                string FileExtention = "";
                int name = 0;
                FileName = System.IO.Path.GetFileName(myFile.FileName);
                string stro = ro.Next(100, 100000000).ToString() + name.ToString();//产生一个随机数用于新命名的图片 
                string NewName = DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + stro;
                if (FileName.Length > 0)//有文件才执行上传操作再保存到数据库 
                {
                    FileExtention = System.IO.Path.GetExtension(myFile.FileName);
                    string noExtension = System.IO .Path.GetFileNameWithoutExtension(myFile.FileName);
                    string ppath = dirpath + "/" + noExtension + "_" + NewName + FileExtention;
                    myFile.SaveAs(ppath);
                    string FJname = FileName;
                    string Savepath = "../File/" + noExtension + "_" + NewName + FileExtention;
                    string v1 = bc.numYMD(20, 12, "000000000001", "SELECT * FROM WAREFILE", "FLKEY", "FL");
                    basec.getcoms(@"INSERT INTO WAREFILE(FLKEY,WAREID,OLDFILENAME,PATH,DATE,YEAR,MONTH,DAY) VALUES 
('" + v1 + "','" +WareID + "','" + FileName + "','" + Savepath + "','" + varDate + "','" + year + "','" + month + "','" + day + "')"); 
                }

            }
    
        }
   
    }
   
}
