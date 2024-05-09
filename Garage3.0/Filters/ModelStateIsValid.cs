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
