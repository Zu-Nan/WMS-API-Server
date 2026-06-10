using DevExpress.ExpressApp;
using WMS.Module.BusinessObjects.JiChuDate;
using WMS.Module.BusinessObjects.TongJi;

namespace WMS.Module.Services
{
    public class RuKouService
    {
        private readonly IObjectSpaceFactory objectSpaceFactory;

        public RuKouService(IObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory = objectSpaceFactory;
        }

        //新建物资
        public void XinJian(WuLiaoHelper wl)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(RuKou));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            RuKou ruKou=os.GetObjects<RuKou>().FirstOrDefault(x=>x.RuKouNum==wl.RuKouName);
            ruKou.BaoHao=wl.BaoHao;
            ruKou.Time=DateTime.Now;
            os.CommitChanges();
        }

        //执行入库
        public RuKou ZhiXing(string ruKouNum)
        {
            using var os=objectSpaceFactory.CreateObjectSpace(typeof(RuKou));
            using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();

            RuKou ruKou=os.GetObjects<RuKou>().FirstOrDefault(x=>x.RuKouNum==ruKouNum);
            if(ruKou==null)
            {
                throw new Exception($"入库口{ruKouNum}不存在");
            }
            return ruKou;
        }
    }
}
