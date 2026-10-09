using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Module.BusinessObjects.JiChuDate;

namespace WMS.Module.BusinessObjects.ZuoYe
{
    [DefaultClassOptions]
    [NavigationItem("作业管理")]
    //倒库对象
    public class DaoKu : XPObject
    {
        public DaoKu(Session session) : base(session) { }

        [Association("RenWu-DaoKus")]
        [DisplayName("所属任务")]
        public RenWu RenWu
        {
            get => GetPropertyValue<RenWu>(nameof(RenWu));
            set => SetPropertyValue(nameof(RenWu), value);
        }

        private string _baoHao;
        [DisplayName("系统包号")]
        public string BaoHao
        {
            get => _baoHao;
            set => SetPropertyValue(nameof(BaoHao), ref _baoHao, value);
        }

        private KuWei _qiShiKuWei;
        [DisplayName("起始库位")]
        public KuWei QiShiKuWei
        {
            get => _qiShiKuWei;
            set => SetPropertyValue(nameof(QiShiKuWei), ref _qiShiKuWei, value);
        }

        private KuWei _muDiKuWei;
        [DisplayName("目的库位")]
        public KuWei MuDiKuWei
        {
            get => _muDiKuWei;
            set => SetPropertyValue(nameof(MuDiKuWei), ref _muDiKuWei, value);
        }

        private RenWuZhuangTai _daoKuZhuangTai;
        [DisplayName("任务状态")]
        public RenWuZhuangTai RenWuZhuangTai
        {
            get => _daoKuZhuangTai;
            set => SetPropertyValue(nameof(RenWuZhuangTai), ref _daoKuZhuangTai, value);
        }

        private DateTime _wanChengShiJian;
        [DisplayName("完成时间")]
        public DateTime WanChengShiJian
        {
            get => _wanChengShiJian;
            set => SetPropertyValue(nameof(WanChengShiJian), ref _wanChengShiJian, value);
        }
    }
    
        
    
}
