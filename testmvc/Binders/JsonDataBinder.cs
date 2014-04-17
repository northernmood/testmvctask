using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using log4net;

namespace testmvc.Binders
{
    public class JsonDataBinder<T> : IModelBinder
    {
        private static ILog log = LogManager.GetLogger(typeof(JsonDataBinder<T>));

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string jsonString = controllerContext.RequestContext.HttpContext.Request.Form.ToString();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string decoded = HttpUtility.UrlDecode(jsonString);
            try
            {
                return serializer.Deserialize<T>(decoded);
            }
            catch (InvalidOperationException ex)
            {
                log.Error(ex);
                return default(T);
            }
        }
    }
}