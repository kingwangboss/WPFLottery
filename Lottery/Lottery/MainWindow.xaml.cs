using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Configuration;
using System.IO;
using System.Xml;
using Lottery.OneJTools;
using Network;
using Newtonsoft.Json.Linq;
using Formatting = System.Xml.Formatting;


namespace Lottery
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// xml配置保存的路径
        /// </summary>
        public static readonly string Path = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/OneJSoft/user.xml";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string signStr = "1" + txtUsername.Text + "3YCW1.0.1ios" + OJSha256.SHA256(txtPassword.Password).ToUpper();
            string sign = OJSha256.SHA256(signStr).ToUpper();

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Action", "Login");
            dict.Add("SID", "1");
            dict.Add("Account", txtUsername.Text);
            dict.Add("AppType", "3");
            dict.Add("AppCode", "YCW");
            dict.Add("AppVersion", "1.0.1");
            dict.Add("ClientID", "ios");
            dict.Add("Sign", sign);
            string str = HttpTool.PostRequest(ConfigurationManager.AppSettings["URL"], dict, "UTF-8", "UTF-8");
            JToken jt = JToken.Parse(str);
            Console.WriteLine(jt);
            Console.WriteLine(jt["Code"]);
            Console.WriteLine(jt["Msg"]);
            if (jt["Code"].ToString().Equals("Suc"))
            {
                OJXmlConfigUtil xmlConfigUtil = new OJXmlConfigUtil(Path);
                xmlConfigUtil.Write(txtUsername.Text, "name");
                xmlConfigUtil.Write(OJSha256.SHA256(txtPassword.Password), "password");
                xmlConfigUtil.Write(jt["Data"]["LastLoginTime"].ToString(), "LastLoginTime");
                xmlConfigUtil.Write(jt["Data"]["SiteUrl"].ToString(), "SiteUrl");
                xmlConfigUtil.Write(jt["Data"]["Token"].ToString(), "Token");
                xmlConfigUtil.Write(jt["Data"]["UID"].ToString(), "UID");
                xmlConfigUtil.Write(jt["Data"]["AuthType"].ToString(), "AuthType");
                xmlConfigUtil.Write(jt["Data"]["AuthTypeName"].ToString(), "AuthTypeName");
                xmlConfigUtil.Write(jt["Data"]["PayType"].ToString(), "PayType");
                xmlConfigUtil.Write(jt["Data"]["QQUrl"].ToString(), "QQUrl");
                MessageBox.Show(jt["Code"].ToString());

                PlanWindow planWindow = new PlanWindow();
                Application.Current.MainWindow = planWindow;
                Close();
                planWindow.Show();

            }
            else
            {
                MessageBox.Show(jt["Msg"].ToString());
            }


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }

}
