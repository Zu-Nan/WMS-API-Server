using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Module.BusinessObjects.JiChuDate;

namespace WMS.Module.Controllers
{
    public class KuQvController : ViewController<ListView>
    {
        private SimpleAction ChuShiHuaAction{get;set;}
        private SimpleAction LockAction{get;set;}
        private SimpleAction UnLockAction{get;set;}

        public KuQvController()
        {
            TargetObjectType = typeof(KuQv);

            //初始化货位
            ChuShiHuaAction=new SimpleAction(this, "ChuShiHua", PredefinedCategory.Edit)
            {
                Caption="初始化货位"
            };
            ChuShiHuaAction.Execute+=ChuShiHuaAction_Execute;

            //锁定
            LockAction=new SimpleAction(this, "Lock", PredefinedCategory.Edit)
            {
                Caption="锁定"
            };
            LockAction.Execute+=LockAction_Execute;

            //解锁
            UnLockAction=new SimpleAction(this, "UnLock", PredefinedCategory.Edit)
            {
                Caption="解锁"
            };
            UnLockAction.Execute+=UnLockAction_Exxecute;
        }

        private void ChuShiHuaAction_Execute(object sender,SimpleActionExecuteEventArgs e)
        {
            IObjectSpace os=Application.CreateObjectSpace(typeof(KuQv));
            foreach(object obj in e.SelectedObjects)
            {
                var kuqv=os.GetObject(obj as KuQv);
                if(kuqv==null)continue;

                //按列排层生成货位
                for(int l = kuqv.QishiLie; l < kuqv.QishiLie + kuqv.LieCount; l++)
                {
                    for(int p = kuqv.QishiPai; p < kuqv.PaiCount + kuqv.QishiPai; p++)
                    {
                        for(int c = kuqv.QishiCeng; c < kuqv.CengCount + kuqv.QishiCeng; c++)
                        {
                            var kuwei=os.CreateObject<KuWei>();

                            kuwei.KuQv=kuqv;
                            kuwei.Lie=l.ToString("D3");
                            kuwei.Pai=p.ToString("D3");
                            kuwei.Ceng=c.ToString("D3");
                            kuwei.IsEmpty=true;
                            kuwei.QiYong=true;
                        }
                    }
                }
            }
            os.CommitChanges();
            View.ObjectSpace.Refresh();
        }

        private void LockAction_Execute(object sender,SimpleActionExecuteEventArgs e)
        {
            IObjectSpace os=Application.CreateObjectSpace(typeof(KuQv));
            foreach(var obj in e.SelectedObjects)
            {
                var kuqv=os.GetObject(obj as KuQv);
                if (kuqv != null)
                {
                    kuqv.Lock=true;

                    var kuweiList=os.GetObjectsQuery<KuWei>()
                                    .Where(x=>x.KuQvNum==kuqv.KuQvNum)
                                    .ToList();
                    
                    foreach(var kuwei in kuweiList)
                    {
                        kuwei.IsLock=true;
                    }
                }
            }
            os.CommitChanges();
            View.ObjectSpace.Refresh();
        }

        private void UnLockAction_Exxecute(object sender,SimpleActionExecuteEventArgs e)
        {
            IObjectSpace os=Application.CreateObjectSpace(typeof(KuQv));
            foreach(var obj in e.SelectedObjects)
            {
                var kuqv=os.GetObject(obj as KuQv);
                if (kuqv != null)
                {
                    kuqv.Lock=false;

                    var kuweiList=os.GetObjectsQuery<KuWei>()
                                    .Where(x=>x.KuQvNum==kuqv.KuQvNum)
                                    .ToList();
                    
                    foreach(var kuwei in kuweiList)
                    {
                        kuwei.IsLock=false;
                    }
                }
            }
            os.CommitChanges();
            View.ObjectSpace.Refresh();
        }
    }
}
