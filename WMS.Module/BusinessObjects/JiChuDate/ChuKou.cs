using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace WMS.Module.BusinessObjects.JiChuDate
{
    [DefaultClassOptions]
    [NavigationItem("基础数据")]
    //出口对象
    public class ChuKou : XPObject
    {
        public ChuKou(Session session) : base(session) { }

        private string _chuKouNum;
        [DisplayName("出口编号")]
        public string ChuKouNum
        {
            get => _chuKouNum;
            set => SetPropertyValue(nameof(ChuKouNum), ref _chuKouNum, value);
        }

        private string _chuKouName;
        [DisplayName("出口名称")]
        public string ChuKouName
        {
            get => _chuKouName;
            set => SetPropertyValue(nameof(ChuKouName), ref _chuKouName, value);
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

        private string _beiZhu;
        [DisplayName("备注")]
        [Size(100)]
        public string BeiZhu
        {
            get => _beiZhu;
            set => SetPropertyValue(nameof(BeiZhu), ref _beiZhu, value);
        }

         protected override void OnSaving()
        {
            base.OnSaving();
            Time = DateTime.Now;
        }
    }
}
