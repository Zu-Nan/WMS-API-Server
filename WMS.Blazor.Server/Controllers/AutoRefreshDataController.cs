using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using System.Timers;

namespace WMS.Blazor.Server.Controllers
{
    public class AutoRefreshDataController:ViewController<ListView>
    {
        private System.Timers.Timer _refreshTimer;
        private const int RefreshIntervalMs = 3000;

        protected override void OnActivated()
        {
            //初始化定时器
            _refreshTimer = new System.Timers.Timer(RefreshIntervalMs);
            _refreshTimer.Elapsed += OnTImerElapsed;
            _refreshTimer.AutoReset = true;
            _refreshTimer.Start();
        }

        private void OnTImerElapsed(object sender, ElapsedEventArgs e)
        {
            if(Application is BlazorApplication blazorApp && View != null && !View.IsDisposed)
            {
                blazorApp.InvokeAsync(() =>
                {
                    //只有在没有选中对象时才刷新，避免丢失选择
                    if (View.SelectedObjects.Count == 0)
                    {
                        View.RefreshDataSource();
                    }
                });
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            //释放定时器
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
        }
    }
}
