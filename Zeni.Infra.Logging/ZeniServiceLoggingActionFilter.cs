namespace Zeni.Infra.Logging
{
    public class ZeniServiceLoggingActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true);
                var logger = actionAttributes.FirstOrDefault(x => x.GetType() == typeof(ServiceLogAttribute));
                if (logger != null)
                {
                    var logAtt = (ServiceLogAttribute)logger;
                    if (logAtt._responseLog)
                    {

                        var objResult = context.Result as ObjectResult;
                        if(objResult != null && objResult.Value != null)
                        {
                            var serviceResponse = new ServiceResponse
                            {
                                Response = objResult.Value
                            };
                            var args = JsonConvert.SerializeObject(serviceResponse);
                            Log.Logger.Warning(args);
                        }
                    }
                }

            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true);
                var logger = actionAttributes.FirstOrDefault(x => x.GetType() == typeof(ServiceLogAttribute));
                if(logger != null)
                {
                    var logAtt = (ServiceLogAttribute)logger;
                    if (logAtt._requestLog)
                    {
                        var serviceRequest = new ServiceRequest
                        {
                            Request = context.ActionArguments.Values
                        };
                        var args = JsonConvert.SerializeObject(serviceRequest);
                        Log.Logger.Warning(args);
                    }
                }
                    
            }
            
        }
    }
}
