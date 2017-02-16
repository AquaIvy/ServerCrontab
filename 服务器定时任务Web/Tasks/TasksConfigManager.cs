using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace 服务器定时任务Web
{
    public class TasksConfigManager
    {
        public static List<OneTask> alltasks = new List<OneTask>();

        public static List<OneTask> Load(string path)
        {
            alltasks.Clear();

            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            XmlNode root = xml.SelectSingleNode("root");
            foreach (XmlElement task in root.ChildNodes)
            {
                OneTask one = new OneTask();
                one.Name = task.GetAttribute("name");

                //这里可以考虑使用反射赋值
                foreach (XmlElement item in task.ChildNodes)
                {
                    if (item.Name == "path")
                    {
                        one.Path = item.InnerText;
                    }
                    else if (item.Name == "description")
                    {
                        one.Description = item.InnerText;
                    }
                    else if (item.Name == "modifyFlag")
                    {
                        one.ModifyFlag = Convert.ToBoolean(item.InnerText);
                    }
                    else if (item.Name == "modifyCount")
                    {
                        one.ModifyCount = Convert.ToInt32(item.InnerText);
                    }
                    else if (item.Name == "lastModifyTime")
                    {
                        one.LastModifyTime = Convert.ToDateTime(item.InnerText);
                    }

                }

                alltasks.Add(one);
            }

            return alltasks;
        }

        public static void Save(string path, List<OneTask> tasks)
        {
            alltasks = tasks;

            string str_task = string.Empty;
            foreach (var task in tasks)
            {
                str_task += string.Format(TASK_TEMPLATE, task.Name, task.Description,
                    task.Path, task.ModifyFlag, task.ModifyCount, task.LastModifyTime)
                    + lineend;
            }

            File.WriteAllText(path, string.Format(CONFIG_TEMPLATE, str_task));
        }

        private static string lineend = "\r\n";
        private static string CONFIG_TEMPLATE = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + lineend
            + "<root>" + lineend
            + "{0}" + lineend
            + "</root>";

        private static string TASK_TEMPLATE = "\t<task name=\"{0}\">" + lineend
            + "\t\t<description>{1}</description>" + lineend
            + "\t\t<path>{2}</path>" + lineend
            + "\t\t<modifyFlag>{3}</modifyFlag>" + lineend
            + "\t\t<modifyCount>{4}</modifyCount>" + lineend
            + "\t\t<lastModifyTime>{5}</lastModifyTime>" + lineend
            + "\t</task>" + lineend;
    }
}