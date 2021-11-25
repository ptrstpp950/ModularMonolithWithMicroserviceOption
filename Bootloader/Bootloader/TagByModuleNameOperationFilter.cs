using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bootloader
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TagByModuleNameOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
                return;
            if (controllerActionDescriptor.ControllerTypeInfo.Assembly.FullName == null)
                return;
            var moduleName = controllerActionDescriptor.ControllerTypeInfo.Assembly.FullName.Split(',')
                .FirstOrDefault();
            operation.Tags = new List<OpenApiTag> { new OpenApiTag { Name = moduleName } };
        }
    }
}