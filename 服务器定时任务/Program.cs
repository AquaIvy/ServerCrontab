using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using 服务器定时任务Web;

namespace 服务器定时任务
{
    class Program
    {
        private static int interval = 2000;
        private static string path = @"";

        static void Main(string[] args)
        {
            try
            {
                //排异检查，只能启动1个
                RejectionCheck();

                //加载配置
                LoadSetting();

                //启动成功，托盘气泡提醒一下
                Notify();

                while (true)
                {
                    Thread.Sleep(interval);

                    var alltasks = TasksConfigManager.Load(path);
                    bool isModified = false;
                    foreach (var task in alltasks)
                    {
                        if (task.ModifyFlag)
                        {
                            isModified = true;
                            task.ModifyFlag = false;
                            task.LastModifyTime = DateTime.Now;
                            task.ModifyCount++;

                            Process.Start(task.Path);
                        }
                    }

                    if (isModified)
                    {
                        TasksConfigManager.Save(path, alltasks);
                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.ToString());
                Environment.Exit(0);

                throw;
            }

        }

        private static void Notify()
        {
            //http://www.crifan.com/show_balloon_tip_on_system_tray_in_csharp/
            //后来考虑了一下    还是算了
        }

        /// <summary>
        /// 排异检查，只能启动1个
        /// </summary>
        private static void RejectionCheck()
        {
            bool flag = false;
            Mutex mutex = new Mutex(true, "MutexExample", out flag);
            if (!flag)
            {
                MessageBox.Show("本程序只能同时启动一个");
                Environment.Exit(0);
            }
        }

        private static void LoadSetting()
        {
#if DEBUG
            if (!File.Exists(@"..\..\App.config"))
#else
            if (!File.Exists("App.config"))
#endif
            {
                MessageBox.Show("未找到本程序配置文件 App.config");
                Environment.Exit(0);
            }

            interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]);
            path = ConfigurationManager.AppSettings["TasksConfigPath"].ToString();
            //path = @"..\TasksConfig.xml";

            if (!File.Exists(path))
            {
                MessageBox.Show("未找到监控文件 " + path);
                Environment.Exit(0);
            }
        }
    }
}
