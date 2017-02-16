using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 服务器定时任务Web
{
    public class OneTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public bool ModifyFlag { get; set; }
        public int ModifyCount { get; set; }
        public DateTime LastModifyTime { get; set; }
    }
}