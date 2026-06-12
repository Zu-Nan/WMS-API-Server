using DevExpress.ExpressApp;
using WMS.Module.BusinessObjects.JiChuDate;
using WMS.Module.BusinessObjects.ZuoYe;
using WMS.Module.BusinessObjects.TongJi;

namespace WMS.Module.Services
{
    public class KuWeiService
    {
        private readonly IObjectSpaceFactory objectSpaceFactory;

        public KuWeiService(IObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory = objectSpaceFactory;
        }

        //执行入库,找空货位
        public KuWei ZhiXing()
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(KuWei));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            KuWei kuWei=os.GetObjects<KuWei>().FirstOrDefault(x=>x.QiYong==true&&x.IsEmpty==true&&x.IsLock==false);

            if(kuWei==null)
            {
                throw new Exception("无空货位");
            }

            kuWei.IsLock=true;

            LogHelper.WriteMessage(logSpace,"KuWeiService.ZhiXing",$"分配货位,货位编号={kuWei.KuWeiNum}");  
            return kuWei;
        }

        //入库完成
        public void RuKuWanCheng(RenWu renWu)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(KuWei));

            KuWei kuWei=os.GetObjectsQuery<KuWei>().FirstOrDefault(x=>x.KuWeiNum==renWu.KuWei.KuWeiNum);

            kuWei.IsLock=false;
            kuWei.IsEmpty=false;
            kuWei.BaoHao=renWu.BaoHao;
            os.CommitChanges();
        }

        //出库完成
        public void ChuKuWanCheng(RenWu renWu)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(KuWei));

            KuWei kuWei=os.GetObjectsQuery<KuWei>().FirstOrDefault(x=>x.KuWeiNum==renWu.KuWei.KuWeiNum);

            kuWei.IsLock=false;
            kuWei.IsEmpty=true;
            kuWei.BaoHao=null;
            os.CommitChanges();
        }

        //入库撤销
        public void RuKuCheXiao(RenWu renWu)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(KuWei));

            KuWei kuWei=os.GetObjectsQuery<KuWei>().FirstOrDefault(x=>x.KuWeiNum==renWu.KuWei.KuWeiNum);

            if(kuWei==null)
            {
                return;
            }

            kuWei.IsLock=false;
            os.CommitChanges();
        }

        //出库撤销
        public void ChuKuCheXiao(RenWu renWu)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(KuWei));

            KuWei kuWei=os.GetObjectsQuery<KuWei>().FirstOrDefault(x=>x.KuWeiNum==renWu.KuWei.KuWeiNum);

            kuWei.IsLock=false;
            os.CommitChanges();
        }

        //倒库逻辑
        public void DaoKu(KuWei kuWei)
        {
            
        }
    }
}
