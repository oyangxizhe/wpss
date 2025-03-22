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


namespace XizheC
{
    public partial class FrmUpdateClient :Form
    {
        CFileInfo cfileinfo = new CFileInfo();
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
     
        static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmUpdateClient());

        }
        public FrmUpdateClient()
        {
            InitializeComponent();
        }
        private void FrmUpdateClient_Load(object sender, EventArgs e)
        {
            
                bind();
        }
        #region bind
        private void bind()
        {
            //progressBar1.Visible = false;
            XizheC.CFileInfo cfileinfo = new XizheC.CFileInfo();
            List<XizheC.CFileInfo> listServer = cfileinfo.FindFile(GetServerUpdatePath ());
            foreach (XizheC.CFileInfo cfileinfoserver in listServer)
            {
                listBox1.Items.Add(cfileinfoserver.FileNameAndPath +" "+ cfileinfoserver.LastFileUpdateTime);

            }
            btnUpdate.Text = "更新";
            label1.Text = "";
        }
        #endregion

        private void btnUpdate_Click(object sender, EventArgs e)
        {
           
            try
            {
                progressBar1.Visible = true;
                showdata(GetClientIPListPath());
        
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

        private void showdata(string path)
        {

            DataSet ds = new DataSet();
            string tablename = GetExcelFirstTableName(path);
            ds = cfileinfo.importExcelToDataSet(path, tablename);
            DataTable dt = ds.Tables[0];
            progressBar1.Minimum = 0;
            progressBar1.Maximum = dt.Rows.Count;
            progressBar1.Value = 0;
            progressBar1.Step = 1;
            foreach (DataRow dr in dt.Rows)
            {
                XUpdate(dr[0].ToString());
                progressBar1.PerformStep();
                listBox2.Items.Add(dr[0].ToString());
         
           
            }
            //progressBar1.Visible = false;
            label1.Text = "客户端程序更新完闭";
        }
        #region XUpdate
        private void XUpdate(string ClientUpdatePath)
        {
            label1.Text = "";
            label1.Visible = false;
            btnUpdate.Enabled = false;
            XizheC.CFileInfo cfileinfo = new XizheC.CFileInfo();
            List<XizheC.CFileInfo> listServer = cfileinfo.FindFile(GetServerUpdatePath ());
            List<XizheC.CFileInfo> listClient = cfileinfo.FindFile(ClientUpdatePath);
           
        
            foreach (XizheC.CFileInfo cfileinfoserver in listServer)
            {

                foreach (XizheC.CFileInfo cfileinfoclient in listClient)
                {
                    if (cfileinfo.CExists(cfileinfoclient.Path, cfileinfoserver.FileName) == false)
                    {
                        File.Copy(cfileinfoserver.FileNameAndPath, cfileinfoclient.Path + cfileinfoserver.FileName);
                        //listBox2.Items.Add("COPY FILE " + cfileinfoserver .FileNameAndPath+ "TO CLIENT"+cfileinfoclient.Path+cfileinfoserver .FileName );
                        break;
                    }
                    if (cfileinfoserver.FileName == cfileinfoclient.FileName)
                    {
                        if (Convert.ToDateTime(cfileinfoclient.LastFileUpdateTime) < Convert.ToDateTime(cfileinfoserver.LastFileUpdateTime))
                        {

                            File.Delete(cfileinfoclient.FileNameAndPath);
                            File.Copy(cfileinfoserver.FileNameAndPath, cfileinfoclient.Path + cfileinfoserver.FileName);
                        }
                        break;

                    }
                    
                }
    
            }
            label1.Visible = true;
            btnUpdate.Enabled = true;
       
        }
        #endregion
        private string GetServerUpdatePath()
        {
            string UpdateUrl = @"\\127.0.0.1\xizhe\进销存管理系统\";
            return UpdateUrl;
        }
        private string GetClientIPListPath()
        {
            string ClientIPListPath = @"\\127.0.0.1\xizhe\ClientIPList.xls";
            return ClientIPListPath;
        }

        public static string GetExcelFirstTableName(string excelFileName)
        {
            string tableName = null;
            if (File.Exists(excelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet." +
                  "OLEDB.4.0;Extended Properties=\"Excel 8.0\";Data Source=" + excelFileName))
                {
                    conn.Open();
                    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    tableName = dt.Rows[0][2].ToString().Trim();

                }
            }
            return tableName;
        }
    }
}
