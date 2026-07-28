using DevExpress.ExpressApp;
using WMS.Module.BusinessObjects.JiChuDate;
using WMS.Module.BusinessObjects.ZuoYe;
using WMS.Module.BusinessObjects.TongJi;
using WMS.Module.BusinessObjects.KuCun;

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

            KuWei wcKuWei=waice(os, kuWei);
            if (wcKuWei != null)
            {
                wcKuWei.IsLock=true;
            }
            
            os.CommitChanges();

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
            KuWei wcKuWei=waice(os, kuWei);
            if (wcKuWei != null)
            {
                wcKuWei.IsLock=false;
            }
            os.CommitChanges();
        }

        //出库完成
        public void ChuKuWanCheng(RenWu renWu)
        {
            using var kuweios=objectSpaceFactory.CreateObjectSpace(typeof(KuWei));
            using var waiceos=objectSpaceFactory.CreateObjectSpace(typeof(KuWei));

            KuWei kuWei=kuweios.GetObjectsQuery<KuWei>().FirstOrDefault(x=>x.KuWeiNum==renWu.KuWei.KuWeiNum);

            kuWei.IsLock=false;
            kuWei.IsEmpty=true;
            kuWei.BaoHao=null;
            if (renWu.IsDaoKu == true)
            {
                renWu.DaoKuMuDiHuoWei.IsLock=false;
                renWu.DaoKuQiShiHuoWei.IsLock=false;
                renWu.DaoKuMuDiHuoWei.BaoHao=renWu.DaoKuQiShiHuoWei.BaoHao;
                renWu.DaoKuQiShiHuoWei.BaoHao=null;
                renWu.DaoKuQiShiHuoWei.IsEmpty=true;
                renWu.DaoKuMuDiHuoWei.IsEmpty=false;

                KuWei wc=waice(waiceos, renWu.DaoKuMuDiHuoWei);
                if (wc != null)
                {
                    wc.IsLock=false;
                }
            }

            KuWei wcKuWei=waice(kuweios, kuWei);
            if (wcKuWei != null)
            {
                wcKuWei.IsLock=false;
            }
            kuweios.CommitChanges();
            waiceos.CommitChanges();
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
            KuWei wcKuWei=waice(os, kuWei);
            if (wcKuWei != null)
            {
                wcKuWei.IsLock=false;
            }
            os.CommitChanges();
        }

        //出库撤销
        public void ChuKuCheXiao(RenWu renWu)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(KuWei));

            KuWei kuWei=os.GetObjectsQuery<KuWei>().FirstOrDefault(x=>x.KuWeiNum==renWu.KuWei.KuWeiNum);

            kuWei.IsLock=false;
            KuWei wcKuWei=waice(os, kuWei);
            if (wcKuWei != null)
            {
                wcKuWei.IsLock=false;
            }
            os.CommitChanges();
        }
        
        //下发出库任务
        public static void XiaFaChuKuRenWu(IObjectSpace objectSpace,WuLiao wuLiao,KuWei DaoKuQiShiHuoWei,KuWei DaoKuMuDiHuoWei)
        {
            wuLiao.KuWei.IsLock=true;

            if (wuLiao.KuWei.Lie == "001")
            {
                KuWei wc=objectSpace.GetObjectsQuery<KuWei>()
                            .Where(x=>x.XiangDaoNum==wuLiao.KuWei.XiangDaoNum&&
                                      x.Lie=="002"&&
                                      x.Pai==wuLiao.KuWei.Pai&&
                                      x.Ceng==wuLiao.KuWei.Ceng)
                            .FirstOrDefault();                                                                                       
                
                wc.IsLock=true;
            }else if(wuLiao.KuWei.Lie=="004")
            {
                KuWei wc=objectSpace.GetObjectsQuery<KuWei>()
                            .Where(x=>x.XiangDaoNum==wuLiao.KuWei.XiangDaoNum&&
                                      x.Lie=="003"&&
                                      x.Pai==wuLiao.KuWei.Pai&&
                                      x.Ceng==wuLiao.KuWei.Ceng)
                            .FirstOrDefault();
                
                wc.IsLock=true;
            }

            if (DaoKuQiShiHuoWei != null)
            {
                DaoKuQiShiHuoWei.IsLock=true;
                DaoKuMuDiHuoWei.IsLock=true;
            }
            else
            {
                return;
            }

            if (DaoKuMuDiHuoWei.Lie == "001")
            {
                KuWei waice=objectSpace.GetObjectsQuery<KuWei>()
                                       .Where(x=>x.XiangDaoNum==DaoKuMuDiHuoWei.XiangDaoNum&&
                                                 x.Lie=="002"&&
                                                 x.Pai==DaoKuMuDiHuoWei.Pai&&
                                                 x.Ceng==DaoKuMuDiHuoWei.Ceng)
                                       .FirstOrDefault();
                
                waice.IsLock=true;
            }
            else if(DaoKuMuDiHuoWei.Lie=="004")
            {
                KuWei waice=objectSpace.GetObjectsQuery<KuWei>()
                                       .Where(x=>x.XiangDaoNum==DaoKuMuDiHuoWei.XiangDaoNum&&
                                                 x.Lie=="003"&&
                                                 x.Pai==DaoKuMuDiHuoWei.Pai&&
                                                 x.Ceng==DaoKuMuDiHuoWei.Ceng)
                                       .FirstOrDefault();
                
                waice.IsLock=true;
            }

            //objecctSpace.CommitChanges();
        } 

        private static KuWei waice(IObjectSpace os, KuWei kuwei)
        {  
            if(kuwei.Lie=="001")
            {
                KuWei waice=os.GetObjectsQuery<KuWei>()
                              .Where(x=>x.Pai==kuwei.Pai&&
                                        x.Ceng==kuwei.Ceng&&
                                        x.Lie=="002"&&
                                        x.XiangDaoNum==kuwei.XiangDaoNum)
                              .FirstOrDefault();

                return waice;
            }
            else if(kuwei.Lie=="004")
            {
                KuWei waice=os.GetObjectsQuery<KuWei>()
                              .Where(x=>x.Pai==kuwei.Pai&&
                                        x.Ceng==kuwei.Ceng&&
                                        x.Lie=="003"&&
                                        x.XiangDaoNum==kuwei.XiangDaoNum)
                              .FirstOrDefault();

                return waice;
            }
            return null;
        }

    }
}
