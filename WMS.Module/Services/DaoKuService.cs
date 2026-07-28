using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.CodeParser;
using DevExpress.ExpressApp;
using WMS.Module.BusinessObjects.JiChuDate;
using WMS.Module.BusinessObjects.KuCun;

namespace WMS.Module.Services
{
    public class DaoKuService{
        public static KuWei DaoKuQiShiHuoWei(IObjectSpace objecctSpace,KuWei kuwei)
        {
            if(kuwei.Lie=="002"||kuwei.Lie=="003")return null;
            if(kuwei.Lie=="001")
            {
                KuWei waice=objecctSpace.GetObjectsQuery<KuWei>()
                                        .Where(x=>x.Pai==kuwei.Pai&&
                                                  x.Ceng==kuwei.Ceng&&
                                                  x.Lie=="002"&&
                                                  x.XiangDaoNum==kuwei.XiangDaoNum)
                                        .FirstOrDefault();

                if (!waice.IsLock&&waice.IsEmpty)
                {
                    return null;
                }
                else
                {
                    return waice;
                }        
            }
            else if(kuwei.Lie=="004")
            {
                KuWei waice=objecctSpace.GetObjectsQuery<KuWei>()
                                        .Where(x=>x.Pai==kuwei.Pai&&
                                                x.Ceng==kuwei.Ceng&&
                                                x.Lie=="003"&&
                                                x.XiangDaoNum==kuwei.XiangDaoNum)
                                        .FirstOrDefault();

                if (!waice.IsLock&&waice.IsEmpty)
                {
                    return null;
                }
                else
                {
                    return waice;
                }
            }
            return null;
        }

        public static KuWei DaoKuMuDiHuoWei(IObjectSpace objectSpace,KuWei kuwei)
        {
            if (kuwei != null)
            {
                KuWei mudi=objectSpace.GetObjectsQuery<KuWei>()
                                      .Where(x=>x.XiangDaoNum==kuwei.XiangDaoNum&&
                                                x.IsEmpty==true&&
                                                x.IsLock==false&&
                                                x.QiYong==true)
                                      .FirstOrDefault();

                return mudi;
            }
            return null;
        }
    }
}
