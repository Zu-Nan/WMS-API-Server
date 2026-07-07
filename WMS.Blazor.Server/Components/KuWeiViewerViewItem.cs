using DevExpress.ExpressApp;
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
    public KuQv KuQv
    {
        get=>GetPropertyValue<KuQv>();
        set=>SetPropertyValue(value);
    }

    public KuWei KuWei
    {
        get=>GetPropertyValue<KuWei>();
        set=>SetPropertyValue(value);
    }

    public BlazorApplication Application { get;set; }
    public IObjectSpace ObjectSpace { get;set; }
    public override Type ComponentType => typeof(KuWeiViewer);
}

public interface IModelKuWeiViewerViewItem : IModelViewItem { }

[ViewItem(typeof(IModelKuWeiViewerViewItem))]
public class KuWeiViewerViewItem : ViewItem, IComponentContentHolder
{
    private RenderFragment _componentContent;
    public KuWeiViewerModel ComponentModel { get;private set; }

    public BlazorApplication Application { get; set; }
    public IObjectSpace ObjectSpace { get; set; }

    public void Setup(IObjectSpace objectSpace, XafApplication application)
    {
        Application = (BlazorApplication)application;
        ObjectSpace = objectSpace;
    }
    public KuWeiViewerViewItem(IModelKuWeiViewerViewItem model,Type objectType) : base(objectType, model.Id) { }
    protected override object CreateControlCore()
    {
        var currentKuQv = View.CurrentObject as KuQv;
        ComponentModel = new KuWeiViewerModel 
        { 
            KuQv = currentKuQv,
            Application=Application,
            ObjectSpace=ObjectSpace
        };
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
        if (ComponentModel == null) return;

        var currentKuQv = View.CurrentObject as KuQv;
        ComponentModel.KuQv = currentKuQv;

        ComponentModel.Application = Application;
        ComponentModel.ObjectSpace = ObjectSpace;

    }

}