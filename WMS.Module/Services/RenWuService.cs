using DevExpress.ExpressApp;
using WMS.Module.BusinessObjects.JiChuDate;
using WMS.Module.BusinessObjects.KuCun;
using WMS.Module.BusinessObjects.ZuoYe;
using WMS.Module.BusinessObjects.TongJi;

namespace WMS.Module.Services
{
    public class RenWuService
    {
        private readonly IObjectSpaceFactory objectSpaceFactory;
        
        
        public RenWuService(IObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory = objectSpaceFactory;
        }

        //任务完成
        public void WanCheng(RenWu renWu)
        {
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            renWu.WanChengShiJian=DateTime.Now;
            renWu.RenWuZhuangTai=RenWuZhuangTai.ZiDongWanCheng;

            LogHelper.WriteMessage(logSpace,"RenWuService.WanCheng",$"{renWu.RenWuLeiXing}已完成,Oid={renWu.Oid},包号={renWu.BaoHao}");
        }
        
        //任务撤销
        public void CheXiao(RenWu renWu)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(RenWu));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

           
            renWu.RenWuZhuangTai=RenWuZhuangTai.RenWuCheXiao;
            os.CommitChanges();

            LogHelper.WriteMessage(logSpace,"RenWuService.CheXiao",$"任务已撤销,Oid={renWu.Oid},包号={renWu.BaoHao}");
        }

        //新建任务
        public void XinJian(WuLiaoHelper rw)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(RenWu));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            RenWu renwu=os.CreateObject<RenWu>();

            renwu.BaoHao=rw.BaoHao;
            renwu.RenWuZhuangTai=RenWuZhuangTai.DengDaiFaSong;
            renwu.RenWuLeiXing=RenWuLeiXing.RuKuRenWu;
            renwu.RuKouName=rw.RuKouName;
            renwu.JiHuaShiJian=DateTime.Now.AddMinutes(10);
            renwu.ChuangJianShiJian=DateTime.Now;
            os.CommitChanges();

            LogHelper.WriteMessage(logSpace,"RenWuService.XinJian",$"新建任务,Oid={renwu.Oid},包号={renwu.BaoHao}");
        }

        //执行入库
        public void ZhiXing(RuKou ruKou,KuWei kuWei)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(RenWu));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            //根据包号倒序查任务
            RenWu renWu=os.GetObjectsQuery<RenWu>()
            .Where(x=>x.BaoHao==ruKou.BaoHao)
            .OrderByDescending(x=>x.Oid)
            .FirstOrDefault();

            if(renWu==null)
            {
                throw new Exception($"未找到任务,包号={ruKou.BaoHao}");
            }

            renWu.RenWuZhuangTai=RenWuZhuangTai.ZhengZaiZhiXing;
            renWu.ZhiXingShiJian=DateTime.Now;
            renWu.FaSongShiJian=DateTime.Now;
            renWu.KuWei=kuWei;
            os.CommitChanges();

            LogHelper.WriteMessage(logSpace,"RenWuService.ZhiXing",$"入库任务已执行,Oid={renWu.Oid},包号={renWu.BaoHao}");
        }

        //下发出库任务
        public static void XiaFaChuKuRenWu(IObjectSpace objectSpace,WuLiao wuLiao)
        {
            RenWu renwu=objectSpace.CreateObject<RenWu>();

            renwu.BaoHao=wuLiao.BaoHao;
            renwu.KuWei=wuLiao.KuWei;
            renwu.RenWuZhuangTai=RenWuZhuangTai.ZhengZaiZhiXing;
            renwu.RenWuLeiXing=RenWuLeiXing.ChuKuRenWu;
            renwu.ChuKouName=wuLiao.ChuKouName;
            renwu.JiHuaShiJian=DateTime.Now.AddMinutes(10);
            renwu.FaSongShiJian=DateTime.Now;
            renwu.ZhiXingShiJian=DateTime.Now;
            renwu.ChuangJianShiJian=DateTime.Now;
            objectSpace.CommitChanges();

            LogHelper.WriteMessage(objectSpace,"RenWuService.XiaFaChuKuRenWu",$"下发出库任务,Oid={renwu.Oid},包号={renwu.BaoHao}");
        }

    }
}
