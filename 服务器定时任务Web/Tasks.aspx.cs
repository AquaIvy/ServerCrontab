using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 服务器定时任务Web
{
    public partial class Tasks : System.Web.UI.Page
    {
        private string path = string.Empty;
        private List<OneTask> alltask = new List<OneTask>();

        protected void Page_Load(object sender, EventArgs e)
        {
            path = Server.MapPath("~/TasksConfig.xml");
            if (!File.Exists(path))
            {
                Response.Write("未找到本程序配置文件 TasksConfig.xml");
                return;
            }

            alltask = TasksConfigManager.Load(path);

            foreach (var task in alltask)
            {
                Button btn = new Button();
                btn.Text = task.Name;
                btn.Width = 120;
                btn.Height = 50;
                Panel1.Controls.Add(btn);
                btn.Command += Btn_Command;

                Label lbl = new Label();
                lbl.Text = task.Description;
                Panel1.Controls.Add(lbl);
            }

        }

        private void Btn_Command(object sender, CommandEventArgs e)
        {
            string text = ((Button)sender).Text;
            var find = alltask.FirstOrDefault(o => o.Name == text);
            if (find == null)
            {
                Label1.Text = "未找到 " + text;
                return;
            }

            Label1.Text = "正在处理 " + text + "  " + DateTime.Now.ToString();
            find.ModifyFlag = true;
            TasksConfigManager.Save(path, alltask);


            Response.Redirect("Redirect.aspx", false);
        }
    }
}