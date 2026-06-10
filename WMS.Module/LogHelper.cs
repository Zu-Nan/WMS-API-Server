using DevExpress.ExpressApp;
using DevExpress.Xpo;
using WMS.Module.BusinessObjects.TongJi;

public static class LogHelper
{
    private static string Truncate(string str, int max = 100)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return str.Length > max ? str.Substring(0, max) + "..." : str;
    }

    //信息日志
    public static void WriteMessage(IObjectSpace objectSpace,string module,string message)
       => WriteLog(objectSpace, "信息", module, Truncate(message), null);
    public static void WriteMessage(Session session, string module, string message)
        => WriteLog(session, "信息", module, Truncate(message), null);

    //错误日志
    public static void WriteError(IObjectSpace objectSpace, string module, string message, Exception ex = null)
       => WriteLog(objectSpace, "错误", module, Truncate(message), Truncate(ex?.ToString()));
    public static void WriteError(Session session, string module, string message, Exception ex = null)
       => WriteLog(session, "错误", module, Truncate(message), Truncate(ex?.ToString()));
    
    //警告日志
    public static void WriteWarning(IObjectSpace objectSpace, string module, string message)
        =>WriteLog(objectSpace, "警告", module, Truncate(message), null);
    
    //给接口用
    private static void WriteLog(IObjectSpace objectSpace, string logShyurui, string module, string message, string exception)
    {
        var log = objectSpace.CreateObject<Log>();
        log.LogTime = DateTime.Now;
        log.LogShyurui = logShyurui;
        log.Module = module;
        log.Message = message;
        log.Exception=exception;

        objectSpace.CommitChanges();
    }

    //给XPO对象用
    private static void WriteLog(Session session, string logShyurui, string module, string message, string exception)
    {
        var log = new Log(session);
        log.LogTime = DateTime.Now;
        log.LogShyurui = logShyurui;
        log.Module = module;
        log.Message = message;
        log.Exception=exception;

        log.Save();
    }
}