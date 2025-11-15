using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace presentation.Attributes
{
    internal class CacheAttribute(int DurationInSeconds = 120) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Create Cache Key
            string CacheKey = CreateCacheKey(context.HttpContext.Request);
            // Search For Value With Cacke Key
           ICacheService cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var value= await cacheService.GetAsync(CacheKey);
            // Return Value if Not Null
            if(value is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = value,
                    ContentType= "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            // If Null
            // Invoke Next
             var executedContext= await next.Invoke();
            // Set Value(Response) with Cache Key
             if(executedContext.Result is OkObjectResult result)
            {
                await  cacheService.SetAsync(CacheKey, result, TimeSpan.FromSeconds(DurationInSeconds));
            }
            // Return Value
            return;

        }
        private string CreateCacheKey(HttpRequest httpRequest)
        {
            StringBuilder key= new StringBuilder();
            key.Append(httpRequest.Path);
            key.Append("?");
            foreach (var item in httpRequest.Query.OrderBy(Q => Q.Key))
            {
                key.Append($"{item.Key}={item.Value}&");
            }


            return key.ToString();
        }
    }
}
