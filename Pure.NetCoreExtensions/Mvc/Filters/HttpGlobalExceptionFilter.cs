using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Pure.NetCoreExtensions
{
    /// <summary>
    /// ��������
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        public static ILogger Log = null;//��־��¼

        public HttpGlobalExceptionFilter(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            this._env = env;
            Log = loggerFactory.CreateLogger<HttpGlobalExceptionFilter>();
        }

        public ContentResult FailedMsg(string msg = null)
        {
            string retResult = "{\"status\":1,\"msg\":\"" + msg + "\"}";//, msg);
             
            return new ContentResult() { Content = retResult };
        }
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            //ִ�й��̳���δ�����쳣
            Exception ex = filterContext.Exception;
#if DEBUG
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                string msg = null;

                if (ex is Exception)
                {
                    msg = ex.Message;
                    filterContext.Result = this.FailedMsg(msg);
                    filterContext.ExceptionHandled = true;
                    return;
                }
            }

            this.LogException(filterContext);
            return;
#endif
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                string msg = null;

                if (ex is Exception)
                {
                    msg = ex.Message;
                }
                else
                {
                    this.LogException(filterContext);
                    msg = "����������";
                }

                filterContext.Result = this.FailedMsg(msg);
                filterContext.ExceptionHandled = true;
                return;
            }
            else
            {
                //���ڷ� ajax ����
                this.LogException(filterContext);
                return;
            }
        }
        /// <summary>
        /// ��¼��־
        /// </summary>
        /// <param name="filterContext"></param>
        private void LogException(ExceptionContext filterContext)
        {
            string mid = filterContext.HttpContext.Request.Query["mid"];//codding ��������ÿ��action��һ��id
            var areaName = (filterContext.RouteData.DataTokens["area"] == null ? "" : filterContext.RouteData.DataTokens["area"]).ToString().ToLower();
            var controllerName = (filterContext.RouteData.Values["controller"]).ToString().ToLower();
            var actionName = (filterContext.RouteData.Values["action"]).ToString().ToLower();

            #region --��¼��־ codding ���������Զ����ֶε���־���磺��¼Controller/action��ģ��ID��--
            Log.LogError(filterContext.Exception, "ȫ�ִ���areaName��" + areaName + ",controllerName��" + controllerName + ",action��" + actionName);
            #endregion
        }
    }
}
