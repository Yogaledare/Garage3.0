using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Garage3._0.Filters;

public class ModelStateIsValid : ActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {
        if (context.ModelState.IsValid) return;

        context.Result = new ViewResult {
            StatusCode = 400,

            TempData = ((Controller) context.Controller).TempData,
            ViewData = ((Controller) context.Controller).ViewData,
        };
    }
}


// ViewName = context.ActionDescriptor.DisplayName, // Or provide the specific view name
// ViewData = new ViewDataDictionary(controller?.ViewData ?? new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState))
// {
//     Model = context.ActionArguments.Values.FirstOrDefault()
// },
// TempData = controller?.TempData


// public void OnActionExecuting(ActionExecutingContext filterContext) {
// throw new NotImplementedException();
// }

// public void OnActionExecuted(ActionExecutedContext filterContext) {
// throw new NotImplementedException();
// }