using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using WMS.Module.BusinessObjects.JiChuDate;

namespace WMS.Module.BusinessObjects.ZuoYe
{
    [DefaultClassOptions]
    [NavigationItem("作业管理")]
    //任务对象
    public class RenWu : XPObject
    {
        public RenWu(Session session) : base(session) { }

        private string _baoHao;
        [DisplayName("系统包号")]
        public string BaoHao
        {
            get => _baoHao;
            set => SetPropertyValue(nameof(BaoHao), ref _baoHao, value);
        }

        private KuWei _kuWei;
        [DisplayName("存储货位")]
        public KuWei KuWei
        {
            get => _kuWei;
            set => SetPropertyValue(nameof(KuWei), ref _kuWei, value);
        }

        private RenWuZhuangTai _renWuZhuangTai;
        [DisplayName("任务状态")]
        public RenWuZhuangTai RenWuZhuangTai
        {
            get => _renWuZhuangTai;
            set => SetPropertyValue(nameof(RenWuZhuangTai), ref _renWuZhuangTai, value);
        }

        private RenWuLeiXing _renWuLeiXing;
        [DisplayName("任务类型")]
        public RenWuLeiXing RenWuLeiXing
        {
            get => _renWuLeiXing;
            set => SetPropertyValue(nameof(RenWuLeiXing), ref _renWuLeiXing, value);
        }

        private string _ruKouName;
        [DisplayName("入口名称")]
        public string RuKouName
        {
            get => _ruKouName;
            set => SetPropertyValue(nameof(RuKouName), ref _ruKouName, value);
        }

        private string _chuKouName;
        [DisplayName("出口名称")]
        public string ChuKouName
        {
            get => _chuKouName;
            set => SetPropertyValue(nameof(ChuKouName), ref _chuKouName, value);
        }

        private bool _isDaoKu;
        [DisplayName("是否倒库")]
        public bool IsDaoKu
        {
            get => _isDaoKu;
            set => SetPropertyValue(nameof(IsDaoKu), ref _isDaoKu, value);
        }

        private string _daoKuBaohao;
        [DisplayName("倒库包号")]
        public string DaoKuBaohao
        {
            get => _daoKuBaohao;
            set => SetPropertyValue(nameof(DaoKuBaohao), ref _daoKuBaohao, value);
        }

        private KuWei _daoKuQiShiHuoWei;
        [DisplayName("倒库起始货位")]
        public KuWei DaoKuQiShiHuoWei
        {
            get => _daoKuQiShiHuoWei;
            set => SetPropertyValue(nameof(DaoKuQiShiHuoWei), ref _daoKuQiShiHuoWei, value);
        }

        private KuWei _daoKuMuDiHuoWei;
        [DisplayName("倒库目的货位")]
        public KuWei DaoKuMuDiHuoWei
        {
            get => _daoKuMuDiHuoWei;
            set => SetPropertyValue(nameof(DaoKuMuDiHuoWei), ref _daoKuMuDiHuoWei, value);
        }

        private DateTime _jiHuaShiJian;
        [DisplayName("计划时间")]
        [ModelDefault("DisplayFormat","{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime JiHuaShiJian
        {
            get => _jiHuaShiJian;
            set => SetPropertyValue(nameof(JiHuaShiJian), ref _jiHuaShiJian, value);
        }

        private DateTime _faSongShiJian;
        [DisplayName("发送时间")]
        [ModelDefault("DisplayFormat","{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime FaSongShiJian
        {
            get => _faSongShiJian;
            set => SetPropertyValue(nameof(FaSongShiJian), ref _faSongShiJian, value);
        }

        private DateTime _zhiXingShiJian;
        [DisplayName("执行时间")]
        [ModelDefault("DisplayFormat","{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime ZhiXingShiJian
        {
            get => _zhiXingShiJian;
            set => SetPropertyValue(nameof(ZhiXingShiJian), ref _zhiXingShiJian, value);
        }

        private DateTime _wanChengShiJian;
        [DisplayName("完成时间")]
        [ModelDefault("DisplayFormat","{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime WanChengShiJian
        {
            get => _wanChengShiJian;
            set => SetPropertyValue(nameof(WanChengShiJian), ref _wanChengShiJian, value);
        }

        private DateTime _chaungJianShiJian;
        [DisplayName("创建时间")]
        [ModelDefault("DisplayFormat","{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime ChuangJianShiJian
        {
            get => _chaungJianShiJian;
            set => SetPropertyValue(nameof(ChuangJianShiJian), ref _chaungJianShiJian, value);
        }

        private string _beiZhu;
        [DisplayName("备注")]
        [Size(100)]
        public string BeiZhu
        {
            get => _beiZhu;
            set => SetPropertyValue(nameof(BeiZhu), ref _beiZhu, value);
        }
    }
}
