using DevExpress.ExpressApp;
using WMS.Module.BusinessObjects.JiChuDate;
using WMS.Module.BusinessObjects.KuCun;
using WMS.Module.BusinessObjects.ZuoYe;
using WMS.Module.BusinessObjects.TongJi;

namespace WMS.Module.Services
{
    public class WuLiaoService
    {
        private readonly IObjectSpaceFactory objectSpaceFactory;

        public WuLiaoService(IObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory = objectSpaceFactory;
        }

        //新建物料
        public void XinJian(WuLiaoHelper wl)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(WuLiao));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            //系统包号不得重复
            var existsWuLiaoList=os.GetObjectsQuery<WuLiao>()
            .Where(x=>x.BaoHao==wl.BaoHao)
            .ToList();

            bool canCreate=false;

            //包号不存在，可以创建
            if(!existsWuLiaoList.Any())
            {
                canCreate=true;
            }
            else//包号均出库完成，可以创建
            {
                bool allChuKu=existsWuLiaoList.All(x=>x.CunChuZhuangTai==CunChuZhuangTai.ChuKuWanCheng);
                if(allChuKu)
                {
                    canCreate=true;
                }
            }

            //不满足条件抛出异常
            if(!canCreate)
            {
                throw new Exception($"系统包号{wl.BaoHao}已存在");
            }

            //新建物资
            WuLiao wuliaos=os.CreateObject<WuLiao>();
            wuliaos.BaoHao=wl.BaoHao;
            wuliaos.WuLiaoName=wl.WuLiaoName;
            wuliaos.MaKouName=wl.RuKouName;
            wuliaos.CunChuZhuangTai=CunChuZhuangTai.DengDaiRuKu;
            wuliaos.Time=DateTime.Now;
            os.CommitChanges();

            LogHelper.WriteMessage(logSpace,"KuCunService.XinJian",$"新建物资成功,包号={wl.BaoHao}");
        }

        //物料入库完成
        public void RuKuWanCheng(RenWu renWu)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(WuLiao));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            //根据包号倒序查物料
            WuLiao wuLiao=os.GetObjectsQuery<WuLiao>()
            .Where(x=>x.BaoHao==renWu.BaoHao)
            .OrderByDescending(x=>x.Oid)
            .FirstOrDefault();

            if(wuLiao==null)
            {
                throw new Exception($"未找到物料,包号={renWu.BaoHao}");
            }

            wuLiao.CunChuZhuangTai=CunChuZhuangTai.RuKuWanCheng;
            wuLiao.RuKuTime=DateTime.Now;
            wuLiao.KuCunCount=1;
            os.CommitChanges();
        }

        //物料出库完成
        public void ChuKuWanCheng(RenWu renWu)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(WuLiao));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            //根据包号倒序查物料
            WuLiao wuLiao=os.GetObjectsQuery<WuLiao>()
            .Where(x=>x.BaoHao==renWu.BaoHao)
            .OrderByDescending(x=>x.Oid)
            .FirstOrDefault();

            if(wuLiao==null)
            {
                throw new Exception($"未找到物料,包号={renWu.BaoHao}");
            }

            wuLiao.CunChuZhuangTai=CunChuZhuangTai.ChuKuWanCheng;
            wuLiao.ChuKuTime=DateTime.Now;
            wuLiao.KuCunCount=0;
            os.CommitChanges();
        }

        //物料入库撤销
        public void RuKuCheXiao(RenWu renWu)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(WuLiao));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            //根据包号倒序查物料
            WuLiao wuLiao=os.GetObjectsQuery<WuLiao>()
            .Where(x=>x.BaoHao==renWu.BaoHao)
            .OrderByDescending(x=>x.Oid)
            .FirstOrDefault();

            if(wuLiao==null)
            {
                throw new Exception($"未找到物料,包号={renWu.BaoHao}");
            }

            wuLiao.CunChuZhuangTai=CunChuZhuangTai.RuKuQuXiao;
            os.CommitChanges();
        }

        //物料出库撤销
        public void ChuKuCheXiao(RenWu renWu)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(WuLiao));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            //根据包号倒序查物料
            WuLiao wuLiao=os.GetObjectsQuery<WuLiao>()
            .Where(x=>x.BaoHao==renWu.BaoHao)
            .OrderByDescending(x=>x.Oid)
            .FirstOrDefault();

            if(wuLiao==null)
            {
                throw new Exception($"未找到物料,包号={renWu.BaoHao}");
            }

            wuLiao.CunChuZhuangTai=CunChuZhuangTai.RuKuWanCheng;
            wuLiao.ZhixingChuku=false;
            os.CommitChanges();
        }

        //执行入库
        public void ZhiXing(RuKou ruKou,KuWei kuWei)
        {
                using var os=objectSpaceFactory.CreateObjectSpace(typeof(WuLiao));
                using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();
    
                //根据包号倒序查物料
                WuLiao wuLiao=os.GetObjectsQuery<WuLiao>()
                .Where(x=>x.BaoHao==ruKou.BaoHao)
                .OrderByDescending(x=>x.Oid)
                .FirstOrDefault();

                if(wuLiao==null)
                {
                    throw new Exception($"未找到物料,包号={ruKou.BaoHao}");
                }

                wuLiao.CunChuZhuangTai=CunChuZhuangTai.ZhengZaiRuKu;
                wuLiao.KuWei=kuWei;

                os.CommitChanges();
        }

        //下发出库任务
        public static void XiaFaChuKuRenWu(IObjectSpace objectSpace, WuLiao wuLiao)
        {
            wuLiao.CunChuZhuangTai=CunChuZhuangTai.ZhengZaiChuKu;
            wuLiao.ChuKouName="1001";
            wuLiao.ZhixingChuku=true;
            objectSpace.CommitChanges();
        }
    }
}
