using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSSDemoForWinform
{
    public partial class Form1 : Form
    {

        private const string accessKeyId = "your id";

        private const string accessKeySecret = "your key";    

        private const string endpoint = "oss-cn-shanghai.aliyuncs.com";

        private const string bucketname = "bucketname";
        string key = "text";
        private OssClient client = null;
        public Form1()
        {
            InitializeComponent();
             client = new OssClient(endpoint, accessKeyId, accessKeySecret);//初始化OSS客户端
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = this.openFileDialog1.ShowDialog();
            if(result==DialogResult.OK)
            {
                this.textBox1.Text = this.openFileDialog1.FileName;
                
            }
            else
            {
                this.textBox1.Text = "打开文件错误";
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {                    
            try
            {
                string filename = this.textBox1.Text;
                var res = UploadFileToOSS(filename, key);                              
              
            }
            catch (Exception ex)
            {
               
            }
        }

      

        private void button3_Click(object sender, EventArgs e)
        {
            this.pictureBox1.ImageLocation = GetImgPath(key);
        }

        #region OSS文件操作

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="filename">文件的本地路径</param>
        /// <param name="keyname">存入OSS上的名字，后续用来获取图片地址用</param>
        /// <returns></returns>
        public PutObjectResult UploadFileToOSS(string filename, string keyname)
        {
            try
            {
                var reslut = client.PutObject(bucketname, keyname, filename);
                return reslut;
            }
            catch (Exception)
            {
                //MessageBox.Show("图片上传失败，文件名："+filename+",keyname:"+keyname);
                throw new Exception("图片上传失败，文件名：" + filename + ",keyname:" + keyname);              
            }
        }
        /// <summary>
        /// 获取图片uri
        /// </summary>
        /// <param name="keyname">上传图片的keyname</param>
        /// <param name="process">需要对图片进行的操作，可空，eg:process = "image/resize,m_fixed,w_100,h_100"</param>
        /// <returns></returns>
        public string GetImgPath(string keyname, string process = null)
        {
            try
            {
                var req = new GeneratePresignedUriRequest(bucketname, key, SignHttpMethod.Get);

                if (!string.IsNullOrWhiteSpace(process))
                {
                    req.Process = process;
                    req.Expiration = DateTime.Now.AddSeconds(30 * 60);
                }

                // 产生带有签名的URI
                var uri = client.GeneratePresignedUri(req);
                return uri.ToString();
            }
            catch (Exception)
            {

                throw new Exception("图片获取失败,key:"+keyname);
            }
        }
        #endregion
    }
}
