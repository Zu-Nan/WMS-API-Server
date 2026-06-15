using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using WMS.Module.BusinessObjects.KuCun;
using WMS.Module.Services;

namespace WMS.Module.Controllers
{
    //在库物资
    public class ZaiKuWuZiController : ViewController<ListView>
    {
        public SimpleAction XiaFaChuKuRenWuAction { get; set; }

        public ZaiKuWuZiController()
        {
            TargetObjectType=typeof(WuLiao);
            TargetViewId="WuLiao_ListView_ZaiKu";

            XiaFaChuKuRenWuAction=new SimpleAction(this, "XiaFaChuKuRenWu", PredefinedCategory.Edit)
            {
                Caption="下发出库任务"
            };

            XiaFaChuKuRenWuAction.Execute+=XiaFaChuKuRenWuAction_Execute;
        }

        private void XiaFaChuKuRenWuAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var selectedWuLiao=e.SelectedObjects.OfType<WuLiao>().ToList();

            if(!selectedWuLiao.Any())
            {
                Application.ShowViewStrategy.ShowMessage("请选择要下发的物资！",InformationType.Warning);
                return;
            }

            foreach(var wuLiao in selectedWuLiao)
            {
                
                //更新物料信息
                WuLiaoService.XiaFaChuKuRenWu(View.ObjectSpace, wuLiao);
                //创建出库任务
                RenWuService.XiaFaChuKuRenWu(View.ObjectSpace,wuLiao);
                //锁定货位
                KuWeiService.XiaFaChuKuRenWu(View.ObjectSpace, wuLiao);
                ObjectSpace.CommitChanges();
            }

            View.ObjectSpace.Refresh();
            Application.ShowViewStrategy.ShowMessage("出库任务已下发！",InformationType.Success);
        }
    }
}
