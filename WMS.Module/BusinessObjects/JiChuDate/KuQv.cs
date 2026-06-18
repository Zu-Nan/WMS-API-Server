using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;


namespace WMS.Module.BusinessObjects.JiChuDate
{
    [DefaultClassOptions]
    [NavigationItem("基础数据")]
    [Appearance("Locked_BlueBackground",Criteria =nameof(Lock)+"=True",TargetItems ="*",BackColor ="LightBlue")]
    //库区管理
    public class KuQv : XPObject
    {
        public KuQv(Session session) : base(session) { }

        [Association("CangKu-KuQvs")]
        [DisplayName("所属仓库")]
        public CangKu CangKu
        {
            get => GetPropertyValue<CangKu>(nameof(CangKu));
            set => SetPropertyValue(nameof(CangKu), value);
        }

        private string _kuqvName;
        [DisplayName("库区名称")]
        public string KuQvName
        {
            get => _kuqvName;
            set => SetPropertyValue(nameof(KuQvName), ref _kuqvName, value);
        }

        private string _kuqvNum;
        [DisplayName("库区编号")]
        public string KuQvNum
        {
            get => _kuqvNum;
            set => SetPropertyValue(nameof(KuQvNum), ref _kuqvNum, value);
        }

        private string _xiangdaoNum;
        [DisplayName("巷道编号")]
        public string XiangDaoNum
        {
            get => _xiangdaoNum;
            set => SetPropertyValue(nameof(XiangDaoNum), ref _xiangdaoNum, value);
        }

        private int _lieCount;
        [DisplayName("列数量")]
        public int LieCount
        {
            get => _lieCount;
            set => SetPropertyValue(nameof(LieCount), ref _lieCount, value);
        }

        private int _qishiLie;
        [DisplayName("起始列")]
        public int QishiLie
        {
            get => _qishiLie;
            set => SetPropertyValue(nameof(QishiLie), ref _qishiLie, value);
        }

        private int _paiCount;
        [DisplayName("排数量")]
        public int PaiCount
        {
            get => _paiCount;
            set => SetPropertyValue(nameof(PaiCount), ref _paiCount, value);
        }

        private int _qishiPai;
        [DisplayName("起始排")]
        public int QishiPai
        {
            get => _qishiPai;
            set => SetPropertyValue(nameof(QishiPai), ref _qishiPai, value);
        }

        private int _cengCount;
        [DisplayName("层数量")]
        public int CengCount
        {
            get => _cengCount;
            set => SetPropertyValue(nameof(CengCount), ref _cengCount, value);
        }

        private int _qishiCeng;
        [DisplayName("起始层")]
        public int QishiCeng
        {
            get => _qishiCeng;
            set => SetPropertyValue(nameof(QishiCeng), ref _qishiCeng, value);
        }

        private bool _zidongKuQv;
        [DisplayName("自动化库区")]
        public bool ZidongKuQv
        {
            get => _zidongKuQv;
            set => SetPropertyValue(nameof(ZidongKuQv), ref _zidongKuQv, value);
        }

        private string _chuKou;
        [DisplayName("出库站台")]
        public string ChuKou
        {
            get => _chuKou;
            set => SetPropertyValue(nameof(ChuKou), ref _chuKou, value);
        }

        private string _ruKou;
        [DisplayName("入库站台")]
        public string RuKou
        {
            get => _ruKou;
            set => SetPropertyValue(nameof(RuKou), ref _ruKou, value);
        }


        private DateTime _time;
        [DisplayName("创建日期")]
        [ModelDefault("DisplayFormat", "{0:yyyy/MM/dd}")]
        public DateTime Time
        {
            get => _time;
            set => SetPropertyValue(nameof(Time), ref _time, value);
        }

        private bool _lock;
        [DisplayName("锁定")]
        public bool Lock
        {
            get => _lock;
            set => SetPropertyValue(nameof(Lock), ref _lock, value);
        }

        private string _beizhu;
        [DisplayName("备注")]
        [Size(100)]
        public string BeiZhu
        {
            get => _beizhu;
            set => SetPropertyValue(nameof(BeiZhu), ref _beizhu, value);
        }

        //库区一对多库位
        [Association("KuQv-KuWeis")]
        public XPCollection<KuWei> KuWeis => GetCollection<KuWei>(nameof(KuWeis));

        [PersistentAlias("[KuWeis][].Count()")]
        [DisplayName("库位数量")]
        public int KuWeiCount => (int)EvaluateAlias(nameof(KuWeiCount));

        [PersistentAlias("[KuWeis][IsEmpty=True And QiYong=True].Count()")]
        [DisplayName("可用库位")]
        public int KeYongKuWeiCount => (int)EvaluateAlias(nameof(KeYongKuWeiCount));
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
