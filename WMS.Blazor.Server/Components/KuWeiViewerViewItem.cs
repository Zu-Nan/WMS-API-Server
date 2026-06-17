using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using WMS.Module.BusinessObjects.JiChuDate;

namespace WMS.Blazor.Server.Components;

public class KuWeiViewerModel : ComponentModelBase
{
    public KuWei KuWei
    {
        get=>GetPropertyValue<KuWei>();
        set=>SetPropertyValue(value);
    }
    public override Type ComponentType => typeof(KuWeiViewer);
}

public interface IModelKuWeiViewerViewItem : IModelViewItem { }

[ViewItem(typeof(IModelKuWeiViewerViewItem))]
public class KuWeiViewerViewItem : ViewItem, IComponentContentHolder
{
    private RenderFragment _componentContent;
    public KuWeiViewerModel ComponentModel { get;private set; }
    public KuWeiViewerViewItem(IModelKuWeiViewerViewItem model,Type objectType) : base(objectType, model.Id) { }
    protected override object CreateControlCore()
    {
        ComponentModel = new KuWeiViewerModel { KuWei = View.CurrentObject as KuWei };
        return ComponentModel;
    }
    public RenderFragment ComponentContent
    {
        get
        {
            _componentContent ??= ComponentModelObserver.Create(ComponentModel, ComponentModel.GetComponentContent());
            return _componentContent;
        }
    }
    protected override void OnCurrentObjectChanged()
    {
        base.OnCurrentObjectChanged();
        if(ComponentModel is not null)
        {
            ComponentModel.KuWei = View.CurrentObject as KuWei;
        }
    }

}