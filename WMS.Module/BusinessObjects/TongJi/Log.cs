using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Model;

namespace WMS.Module.BusinessObjects.TongJi
{
    [DefaultClassOptions]
    [NavigationItem("统计查询")]
    //系统日志
    public class Log : BaseObject
    {
        public Log(Session session) : base(session) { }

        [DisplayName("时间")]
        [ModelDefault("DisplayFormat", "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime LogTime { get; set; }

        [DisplayName("日志类别")]
        public string LogShyurui { get; set; }

        [DisplayName("模块名称")]
        public string Module { get; set; }

        [DisplayName("内容")]
        [Size(SizeAttribute.Unlimited)]
        public string Message { get; set; }

        [DisplayName("异常")]
        [Size(SizeAttribute.Unlimited)]
        public string Exception { get; set; }
    }
}
