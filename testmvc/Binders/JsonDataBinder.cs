using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using testmvc.Models;

namespace testmvc.Binders
{
    public class JsonDataBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string jsonString = controllerContext.RequestContext.HttpContext.Request.Form.ToString();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string decoded = HttpUtility.UrlDecode(jsonString);
            return serializer.Deserialize<T>(decoded);
        }
    }
}