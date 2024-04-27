using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GestorTask.Utilitys;

public class FilterValidationException : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.Where(v => v.Errors.Any())
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage.Replace("'", "").ToString(System.Globalization.CultureInfo.DefaultThreadCurrentCulture))
                    .ToList();

            var responseObj = $"{context.ActionArguments.FirstOrDefault().Key} 400 {new StringBuilder().AppendJoin(", ", errors).ToString()}";

            context.Result = new JsonResult(responseObj)
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ContentType = "application/json"
            };
        }
    }
}
