using DevExpress.ExpressApp.DC;

//存储状态
public enum CunChuZhuangTai
{
    [XafDisplayName("等待入库")]
    DengDaiRuKu = 0,
    [XafDisplayName("正在入库")]
    ZhengZaiRuKu = 1,
    [XafDisplayName("入库中断")]
    RuKuZhongDuan = 2,
    [XafDisplayName("入库取消")]
    RuKuQuXiao = 3,
    [XafDisplayName("入库完成")]
    RuKuWanCheng = 4,
    [XafDisplayName("等待出库")]
    DengDaiChuKu = 5,
    [XafDisplayName("正在出库")]
    ZhengZaiChuKu = 6,
    [XafDisplayName("出库中断")]
    ChuKuZhongDuan = 7,
    [XafDisplayName("出库取消")]
    ChuKuQuXiao = 8,
    [XafDisplayName("出库完成")]
    ChuKuWanCheng = 9
}

//任务状态
public enum RenWuZhuangTai
{
    [XafDisplayName("等待发送")]
    DengDaiFaSong = 0,
    [XafDisplayName("等待执行")]
    DengDaiZhiXing = 1,
    [XafDisplayName("正在执行")]
    ZhengZaiZhiXing = 2,
    [XafDisplayName("故障中断")]
    GuZhangZhongDuan = 3,
    [XafDisplayName("任务撤销")]
    RenWuCheXiao = 4,
    [XafDisplayName("自动完成")]
    ZiDongWanCheng = 5,
    [XafDisplayName("人工完成")]
    RenGongWanCheng = 6
}

//任务类型
public enum RenWuLeiXing
{
    [XafDisplayName("入库任务")]
    RuKuRenWu = 0,
    [XafDisplayName("出库任务")]
    ChuKuRenWu = 1
}