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
        public static void XinJian(IObjectSpace os, WuLiaoHelper wl)
        {
           
            //系统包号不得重复
            bool cunzai=os.GetObjectsQuery<WuLiao>()
            .Any(x=>x.BaoHao==wl.BaoHao &&
                    x.CunChuZhuangTai!=CunChuZhuangTai.RuKuQuXiao &&
                    x.CunChuZhuangTai!=CunChuZhuangTai.ChuKuWanCheng);

            //不满足条件抛出异常
            if(!cunzai)
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

            using var logSpace = os.CreateNestedObjectSpace();
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
            using var dkos=objectSpaceFactory.CreateObjectSpace(typeof(WuLiao));
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

            if (renWu.IsDaoKu == true)
            {
                WuLiao dkWuLiao=os.GetObjectsQuery<WuLiao>()
                                    .Where(x=>x.BaoHao==renWu.DaoKuBaohao) 
                                    .OrderByDescending(x=>x.Oid)
                                    .FirstOrDefault();

                dkWuLiao.KuWei = os.GetObjectByKey<KuWei>(renWu.DaoKuMuDiHuoWei.Oid);
            }
            dkos.CommitChanges();
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
            wuLiao.ChuKouName=null;
            wuLiao.ZhixingChuku=false;
            os.CommitChanges();
        }

        //执行入库
        public void ZhiXing(RuKou ruKou,KuWei kuWei)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(WuLiao));

            var kuwei=os.GetObjectByKey<KuWei>(kuWei.Oid);
    
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
            wuLiao.KuWei=kuwei;

            os.CommitChanges();
        }

        //下发出库任务
        public static void XiaFaChuKuRenWu(IObjectSpace objectSpace, WuLiao wuLiao)
        {
            wuLiao.CunChuZhuangTai=CunChuZhuangTai.ZhengZaiChuKu;
            wuLiao.ChuKouName="9001";
            wuLiao.ZhixingChuku=true;
            //objectSpace.CommitChanges();
        }
    }
}
