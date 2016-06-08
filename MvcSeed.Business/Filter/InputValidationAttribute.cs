namespace MvcSeed.Business.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Newtonsoft.Json;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InputValidationAttribute : ActionFilterAttribute
    {
        private readonly bool _isReturnJson;

        public InputValidationAttribute(bool isReturnJson = false)
        {
            _isReturnJson = isReturnJson;
        }

        public const int HTTPCODE_ACCESS_DENIED = 498;

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.Controller.ViewData.ModelState.IsValid)
            {
                IDictionary<string, IList<string>> errorMap = new Dictionary<string, IList<string>>();
                foreach (var key in actionContext.Controller.ViewData.ModelState.Keys)
                {
                    ModelState state;
                    if (actionContext.Controller.ViewData.ModelState.TryGetValue(key, out state) && state.Errors.Any())
                    {
                        var errors = state.Errors.Select(x => x.ErrorMessage).ToList();
                        errorMap.Add(key, errors);
                    }
                }

                if (_isReturnJson)
                {
                    actionContext.HttpContext.Response.Clear();
                    actionContext.HttpContext.Response.StatusCode = HTTPCODE_ACCESS_DENIED;
                    actionContext.HttpContext.Response.AppendHeader("request-validation", "invalid");
                    actionContext.Result = new JsonResult { Data = errorMap };
                }
                else
                {
                    throw new HttpException(HTTPCODE_ACCESS_DENIED, JsonConvert.SerializeObject(errorMap));
                }
            }
        }
    }
}
