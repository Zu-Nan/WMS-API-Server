using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace WMS.Module.BusinessObjects.JiChuDate
{
    [DefaultClassOptions]
    [NavigationItem("基础数据")]
    //入口对象
    public class RuKou : XPObject
    {
        public RuKou(Session session) : base(session) { }

        private string _ruKouNum;
        [DisplayName("入口编号")]
        public string RuKouNum
        {
            get => _ruKouNum;
            set => SetPropertyValue(nameof(RuKouNum), ref _ruKouNum, value);
        }

        private string _ruKouName;
        [DisplayName("入口名称")]
        public string RuKouName
        {
            get => _ruKouName;
            set => SetPropertyValue(nameof(RuKouName), ref _ruKouName, value);
        }

        private string _baoHao;
        [DisplayName("系统包号")]
        public string BaoHao
        {
            get => _baoHao;
            set => SetPropertyValue(nameof(BaoHao), ref _baoHao, value);
        }

        private bool _qiYong;
        [DisplayName("启用")]
        public bool QiYong
        {
            get => _qiYong;
            set => SetPropertyValue(nameof(QiYong), ref _qiYong, value);
        }

        private DateTime _time;
        [DisplayName("更新时间")]
        [ModelDefault("DisplayFormat", "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime Time
        {
            get => _time;
            set => SetPropertyValue(nameof(Time), ref _time, value);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            Time = DateTime.Now;
        }
    }
}
