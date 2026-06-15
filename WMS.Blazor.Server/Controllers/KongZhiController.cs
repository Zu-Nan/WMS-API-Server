using DevExpress.ExpressApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Module;
using WMS.Module.BusinessObjects.JiChuDate;
using WMS.Module.BusinessObjects.TongJi;
using WMS.Module.BusinessObjects.ZuoYe;
using WMS.Module.Services;

namespace WMS.Blazor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KongZhiController:ControllerBase
    {
        private readonly IObjectSpaceFactory objectSpaceFactory;

        public KongZhiController(IObjectSpaceFactory objectSpaceFactory)
        {
            this.objectSpaceFactory=objectSpaceFactory;
        }

        //完成作业
        [HttpPost("WanCheng")]
        public IActionResult WanCheng([FromBody] int renwuOid)
        {
            try
            {
                using var os=objectSpaceFactory.CreateObjectSpace(typeof(RenWu));

                RenWuService rwService=new RenWuService(objectSpaceFactory);
                WuLiaoService wlService=new WuLiaoService(objectSpaceFactory);
                KuWeiService kwService=new KuWeiService(objectSpaceFactory);

                //Oid查任务
                RenWu renwu=os.GetObjectByKey<RenWu>(renwuOid);
                if (renwu == null)
                {
                    return NotFound($"未找到任务,Oid={renwuOid}");
                }

                if(renwu.RenWuLeiXing==RenWuLeiXing.RuKuRenWu)//入库任务
                {
                    wlService.RuKuWanCheng(renwu);
                    kwService.RuKuWanCheng(renwu);
                }
                else//出库任务
                {
                    wlService.ChuKuWanCheng(renwu);
                    kwService.ChuKuWanCheng(renwu);
                }

                rwService.WanCheng(renwu);
                os.CommitChanges();

                return Ok(new{success=true,message="任务已完成"});
            }
            catch(Exception ex)
            {
                using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();
                LogHelper.WriteError(logSpace,"APIController.WanCheng",$"执行失败：{ex.Message}",ex);
                return BadRequest(new{success=false,message=ex.Message});
            }
        }

        //撤销作业
        [HttpPost("CheXiao")]
        public IActionResult CheXiao([FromBody] int renwuOid)
        {
            try
            {
                using var os=objectSpaceFactory.CreateObjectSpace(typeof(RenWu));

                RenWuService rwService=new RenWuService(objectSpaceFactory);
                WuLiaoService wlService=new WuLiaoService(objectSpaceFactory);
                KuWeiService kwService=new KuWeiService(objectSpaceFactory);

                //Oid查任务
                RenWu renwu=os.GetObjectByKey<RenWu>(renwuOid);
                if (renwu == null)
                {
                    return NotFound($"未找到任务,Oid={renwuOid}");
                }

                if (renwu.RenWuLeiXing == RenWuLeiXing.RuKuRenWu)//入库任务
                {
                    wlService.RuKuCheXiao(renwu);
                    kwService.RuKuCheXiao(renwu);
                }
                else//出库任务
                {
                    wlService.ChuKuCheXiao(renwu);
                    kwService.ChuKuCheXiao(renwu);
                }

                rwService.CheXiao(renwu);
                os.CommitChanges();

                return Ok(new{success=true,message="任务已撤销"});
            }
            catch(Exception ex)
            {
                using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();
                LogHelper.WriteError(logSpace,"APIController.CheXiao",$"执行失败：{ex.Message}",ex);
                return BadRequest(new{success=false,message=ex.Message});
            }
        }

        //新建物资
        [HttpPost("XinJianWuLiao")]
        public IActionResult XinJianWuLiao([FromBody] WuLiaoHelper wuliao)
        {
            try
            {
                WuLiaoService wlService=new WuLiaoService(objectSpaceFactory);
                RenWuService rwService=new RenWuService(objectSpaceFactory);
                RuKouService rkService=new RuKouService(objectSpaceFactory);

                wlService.XinJian(wuliao);
                rwService.XinJian(wuliao);
                rkService.XinJian(wuliao);

                return Ok(new{success=true,message=$"物资已创建,包号={wuliao.BaoHao}"});
            }
            catch(Exception ex)
            {
                using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();
                LogHelper.WriteError(logSpace,"APIController.XinJianWuLiao",$"执行失败：{ex.Message}",ex);
                return BadRequest(new{success=false,message=ex.Message});
            }
        }

        //执行入库
        [HttpPost("ZhiXingRuKu")]
        public IActionResult ZhiXingRuKu([FromBody] string rukouNum)
        {
            try
            {
                RuKouService rkService=new RuKouService(objectSpaceFactory);
                WuLiaoService wlService=new WuLiaoService(objectSpaceFactory);
                RenWuService rwService=new RenWuService(objectSpaceFactory);
                KuWeiService kwService=new KuWeiService(objectSpaceFactory);

                RuKou rukou=rkService.ZhiXing(rukouNum);
                KuWei kuwei=kwService.ZhiXing();
                wlService.ZhiXing(rukou,kuwei);
                rwService.ZhiXing(rukou,kuwei);

                return Ok(new{success=true,message="任务已执行"});
            }
            catch(Exception ex)
            {
                using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();
                LogHelper.WriteError(logSpace,"APIController.ZhiXingRuKu",$"执行失败：{ex.Message}",ex);
                return BadRequest(new{success=false,message=ex.Message});
            }
        }
    }
}
