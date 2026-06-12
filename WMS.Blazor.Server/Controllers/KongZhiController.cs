using DevExpress.ExpressApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                    rwService.WanCheng(renwu);
                }
                else//出库任务
                {
                    wlService.ChuKuWanCheng(renwu);
                    kwService.ChuKuWanCheng(renwu);
                    rwService.WanCheng(renwu);
                }

                return Ok(new{success=true,message="任务已完成"});
            }
            catch(Exception ex)
            {
                using var logSpace=objectSpaceFactory.CreateObjectSpace<Log>();
                LogHelper.WriteError(logSpace,"APIController.WanCheng",$"执行失败：{ex.Message}",ex);
                return BadRequest(new{success=false,message=ex.Message});
            }
        }
    }
}
