using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace EmbilyServices
{
    /*
     * the attribute is being used for APIExplorer Predicate(!) --
     * 
     */
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SwaggerTagAttribute : Attribute
    {
    }

    public class SwaggerTagFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {

            /*
             * this approach and the second on below do not work as i would want. They still leave out data models/contracts 
             * for other APIs, need to tell Swagger to ignore the API then related data models will not be generated! --
             * 
             */

            foreach (var contextApiDescription in context.ApiDescriptions)
            {
                var actionDescriptor = (ControllerActionDescriptor)contextApiDescription.ActionDescriptor;

                if (!actionDescriptor.ControllerTypeInfo.GetCustomAttributes<SwaggerTagAttribute>().Any() &&
                    !actionDescriptor.MethodInfo.GetCustomAttributes<SwaggerTagAttribute>().Any())
                {
                    var key = "/" + contextApiDescription.RelativePath.TrimEnd('/');
                    swaggerDoc.Paths.Remove(key);
                }
            }
        }

        private void removeByPath(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var pathsToRemove = swaggerDoc.Paths.Where(pathItem => !pathItem.Key.Contains("api/v2")).ToList();

            foreach (var item in pathsToRemove)
            {
                swaggerDoc.Paths.Remove(item.Key);
            }

        }
    }
}
