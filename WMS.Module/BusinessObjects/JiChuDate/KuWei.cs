using DevExpress.CodeParser;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;


namespace WMS.Module.BusinessObjects.JiChuDate
{
    [DefaultClassOptions]
    [NavigationItem("基础数据")]
    [Appearance("Locked_BlueBackground", Criteria = nameof(IsLock) + "=True", TargetItems = "*", BackColor = "LightBlue")]
    [XafDefaultProperty(nameof(KuWeiNum))]
    //库位管理
    public class KuWei : XPObject
    {
        public KuWei(Session session) : base(session) { }

        [Association("KuQv-KuWeis")]
        [DisplayName("所属库区")]
        public KuQv KuQv
        {
            get => GetPropertyValue<KuQv>(nameof(KuQv));
            set => SetPropertyValue(nameof(KuQv), value);
        }

        [PersistentAlias("Concat(KuQv.CangKu.CangKuNum,'-',KuQv.XiangDaoNum,'-',Lie,'-',Pai,'-',Ceng)")]
        [DisplayName("库位编号")]
        public string KuWeiNum => EvaluateAlias(nameof(KuWeiNum))?.ToString();

        [PersistentAlias("Concat(KuQv.CangKu.CangKuNum,'仓库-',KuQv.XiangDaoNum,'巷道-',Lie,'列-',Pai,'排-',Ceng,'层')")]
        [DisplayName("库位说明")]
        public string KuWeiShuoMing => EvaluateAlias(nameof(KuWeiShuoMing))?.ToString();

        [PersistentAlias("KuQv.KuQvNum")]
        [DisplayName("库区编号")]
        public string KuQvNum => EvaluateAlias(nameof(KuQvNum))?.ToString();

        [PersistentAlias("KuQv.XiangDaoNum")]
        [DisplayName("巷道编号")]
        public string XiangDaoNum => EvaluateAlias(nameof(XiangDaoNum))?.ToString();

        private string _lie;
        [DisplayName("列号")]
        public string Lie
        {
            get => _lie;
            set => SetPropertyValue(nameof(Lie), ref _lie, value);
        }

        private string _pai;
        [DisplayName("排号")]
        public string Pai
        {
            get => _pai;
            set => SetPropertyValue(nameof(Pai), ref _pai, value);
        }

        private string _ceng;
        [DisplayName("层号")]
        public string Ceng
        {
            get => _ceng;
            set => SetPropertyValue(nameof(Ceng), ref _ceng, value);
        }

        private string _baoHao;
        [DisplayName("系统包号")]
        public string BaoHao
        {
            get => _baoHao;
            set => SetPropertyValue(nameof(BaoHao), ref _baoHao, value);
        }

        private bool _isEmpty;
        [DisplayName("空货位")]
        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetPropertyValue(nameof(IsEmpty), ref _isEmpty, value);
        }

        private bool _isLock;
        [DisplayName("任务锁定")]
        public bool IsLock
        {
            get => _isLock;
            set => SetPropertyValue(nameof(IsLock), ref _isLock, value);
        }

        private bool _qiYong;
        [DisplayName("启用")]
        public bool QiYong
        {
            get => _qiYong;
            set => SetPropertyValue(nameof(QiYong), ref _qiYong, value);
        }

        private DateTime _time;
        [DisplayName("创建日期")]
        [ModelDefault("DisplayFormat", "{0:yyyy/MM/dd}")]
        public DateTime Time
        {
            get => _time;
            set => SetPropertyValue(nameof(Time), ref _time, value);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                Time = DateTime.Now;
            }
        }
    }
}
